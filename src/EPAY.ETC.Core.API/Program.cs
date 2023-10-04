using EPAY.ETC.Core.API.Filters;
using EPAY.ETC.Core.API.Infrastructure.Persistence;
using EPAY.ETC.Core.API.Models.Configs;
using EPAY.ETC.Core.API.Services;
using EPAY.ETC.Core.Publisher.DependencyInjectionExtensions;
using EPAY.ETC.Core.RabbitMQ.DependencyInjectionExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using StackExchange.Redis;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

// Config IOptions
builder.Services.Configure<List<PublisherConfigurationOption>>(builder.Configuration.GetSection("PublisherConfigurations"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped<ValidationFilterAttribute>();

// Add services to the container.
builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add(new ValidationFilterAttribute());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Configuration for swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EPAY ETC Core API",
        Version = "v1"
    });
    // To Enable authorization using Swagger (JWT)    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()

                    }
                });
    c.IncludeXmlComments(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty, "EPAY.ETC.Core.API.xml"), true);
});
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "EPAYPolicy",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();
builder.Services.AddInfrastructure(config);

// RabbitMQ: Setup RabbitMQ for Dependency injection
builder.Services
    .AddRabbitMQCore(builder.Configuration)
    .AddRabbitMQPublisher();

// Init instance Redis
var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration.GetSection("RedisSettings").GetValue<string>("ConnectionString") ?? "localhost:6379");
builder.Services.AddSingleton(multiplexer.GetDatabase(builder.Configuration.GetSection("RedisSettings").GetValue<int?>("db") ?? -1));
builder.Services.AddScoped<IRabbitMQPublisherService, RabbitMQPublisherService>();

var app = builder.Build();

// Configuration for NLog
var configuringNLogFileName = "nlog.config";
var configuringNLogEnvironmentFileName = $"nlog.{app.Environment.EnvironmentName}.config";

if (File.Exists(configuringNLogEnvironmentFileName))
{
    configuringNLogFileName = configuringNLogEnvironmentFileName;
}

LogManager.Setup().LoadConfigurationFromFile(configuringNLogFileName);

// Configuration for swagger
_ = app.UseSwagger();
_ = app.UseSwaggerUI(c =>
{
    if (builder.Environment.IsDevelopment()) // config to save authorization when restart api on dev env
    {
        c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
    }
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EPAY ETC Core API");
});



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

/// <summary>
/// 
/// </summary>
public partial class Program { }

