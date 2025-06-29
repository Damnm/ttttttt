using EPAY.ETC.Core.API.Filters;
using EPAY.ETC.Core.API.Infrastructure.Models.Configs;
using EPAY.ETC.Core.API.Infrastructure.Models.Options;
using EPAY.ETC.Core.API.Infrastructure.Persistence;
using EPAY.ETC.Core.API.Models.Configs;
using EPAY.ETC.Core.API.Services;
using EPAY.ETC.Core.Models.Constants;
using EPAY.ETC.Core.Publisher.DependencyInjectionExtensions;
using EPAY.ETC.Core.RabbitMQ.DependencyInjectionExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using StackExchange.Redis;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

var environmentName = Environment.GetEnvironmentVariable(CoreConstant.ENVIRONMENT_BASE);
builder.Configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true);

// Config IOptions
builder.Services.Configure<List<PublisherConfigurationOption>>(builder.Configuration.GetSection("PublisherConfigurations"));
builder.Services.Configure<EPAY.ETC.Core.Models.UI.UIModel>(builder.Configuration.GetSection("UITemplate"));
builder.Services.Configure<JWTSettingsConfig>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));
builder.Services.Configure<WalletUrlOptions>(builder.Configuration.GetSection("WalletAPISettings"));
builder.Services.Configure<ConfigDetails>(builder.Configuration);

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
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
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
var multiplexer = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable(CoreConstant.ENVIRONMENT_REDIS) ?? builder.Configuration.GetSection("RedisSettings").GetValue<string>("ConnectionString") ?? "localhost:6379,abortConnect=False,connectRetry=2147483647");
builder.Services.AddSingleton(multiplexer.GetDatabase(builder.Configuration.GetSection("RedisSettings").GetValue<int?>("db") ?? -1));
builder.Services.AddSingleton(multiplexer.GetServer(multiplexer.GetEndPoints().FirstOrDefault()));
builder.Services.AddScoped<IRabbitMQPublisherService, RabbitMQPublisherService>();

// Config authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });

builder.Services.AddCors();

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

app.UseCors(policy => policy.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

/// <summary>
/// 
/// </summary>
public partial class Program { }

