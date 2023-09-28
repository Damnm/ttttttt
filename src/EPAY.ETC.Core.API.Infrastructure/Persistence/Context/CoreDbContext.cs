using EPAY.ETC.Core.API.Core.Extensions;
using EPAY.ETC.Core.API.Core.Models.Configs;
using EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes;
using EPAY.ETC.Core.API.Core.Models.Enum;
using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.API.Core.Models.Fees;
using EPAY.ETC.Core.API.Core.Models.FeeTypes;
using EPAY.ETC.Core.API.Core.Models.FeeVehicleCategories;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.Payment;
using EPAY.ETC.Core.API.Core.Models.PaymentStatus;
using EPAY.ETC.Core.API.Core.Models.TimeBlockFees;
using EPAY.ETC.Core.API.Core.Models.Transaction;
using EPAY.ETC.Core.API.Core.Models.TransactionLog;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Models.VehicleCategories;
using EPAY.ETC.Core.API.Core.Models.VehicleGroups;
using EPAY.ETC.Core.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Context
{
    [ExcludeFromCodeCoverage]
    public class CoreDbContext : DbContext
    {
        public CoreDbContext() { }
        public CoreDbContext(DbContextOptions options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public virtual DbSet<VehicleModel> Vehicles { get; set; }
        public virtual DbSet<VehiclePaymentTransaction> VehiclePaymentTransactions { get; set; }
        public virtual DbSet<VehicleTransactionModel> VehicleTransactionModels { get; set; }
        public virtual DbSet<LaneInCameraTransactionLog> LaneInCameraTransactionLogs { get; set; }
        public virtual DbSet<LaneInRFIDTransactionLog> LaneInRFIDTransactionLogs { get; set; }
        public virtual DbSet<FusionModel> Fusions { get; set; }
        public virtual DbSet<CustomVehicleTypeModel> CustomVehicleTypes { get; set; }
        public virtual DbSet<FeeTypeModel> FeeTypes { get; set; }
        public virtual DbSet<VehicleCategoryModel> VehicleCategories { get; set; }
        public virtual DbSet<VehicleGroupModel> VehicleGroups { get; set; }
        public virtual DbSet<TimeBlockFeeModel> TimeBlockFees { get; set; }
        public virtual DbSet<TimeBlockFeeFormulaModel> TimeBlockFeeFormulas { get; set; }
        public virtual DbSet<FeeVehicleCategoryModel> FeeVehicleCategories { get; set; }
        public virtual DbSet<FeeModel> Fees { get; set; }
        public virtual DbSet<PaymentStatusModel> PaymentStatuses { get; set; }
        public virtual DbSet<PaymentModel> Payments { get; set; }
        public virtual DbSet<ETCCheckoutDataModel> ETCCheckOuts { get; set; }
        public virtual DbSet<AppConfigModel> AppConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FusionModel>().Ignore(x => x.CreatedDate);

            #region Custom vehicle type configuration
            modelBuilder.Entity<CustomVehicleTypeModel>().HasKey(x => x.Id);
            modelBuilder.Entity<CustomVehicleTypeModel>()
                .Property(x => x.Name)
                .HasMaxLength(50)
                .HasConversion(new EnumToStringConverter<CustomVehicleTypeEnum>());

            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            modelBuilder.Entity<CustomVehicleTypeModel>().HasData(
                new CustomVehicleTypeModel()
                {
                    Id = new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = CustomVehicleTypeEnum.Type1,
                    Desc = CustomVehicleTypeEnum.Type1.ToEnumMemberAttrValue()
                },
                new CustomVehicleTypeModel()
                {
                    Id = new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = CustomVehicleTypeEnum.Type2,
                    Desc = CustomVehicleTypeEnum.Type2.ToEnumMemberAttrValue()
                },
                new CustomVehicleTypeModel()
                {
                    Id = new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = CustomVehicleTypeEnum.Type3,
                    Desc = CustomVehicleTypeEnum.Type3.ToEnumMemberAttrValue()
                },
                new CustomVehicleTypeModel()
                {
                    Id = new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = CustomVehicleTypeEnum.Type4,
                    Desc = CustomVehicleTypeEnum.Type4.ToEnumMemberAttrValue()
                });
            #endregion

            #region Vehicle category configuration
            modelBuilder.Entity<VehicleCategoryModel>().HasKey(x => x.Id);

            modelBuilder.Entity<VehicleCategoryModel>().HasData(
                new VehicleCategoryModel()
                {
                    Id = new Guid("70884a61-39f3-4e8e-b936-d5b18652d3ac"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = "Xe nhượng quyền"
                },
                new VehicleCategoryModel()
                {
                    Id = new Guid("82f143d3-b2ed-40d6-a59e-4fc980a24450"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = "Xe nhượng quyền TCP"
                },
                new VehicleCategoryModel()
                {
                    Id = new Guid("2b0557d0-cc6b-4fc2-a0b3-08788c9fd8c7"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = "Xe ưu tiên theo tháng"
                },
                new VehicleCategoryModel()
                {
                    Id = new Guid("bd4e670d-8cae-46fa-8bac-d77ac139a044"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = "Xe ưu tiên theo quý"
                },
                new VehicleCategoryModel()
                {
                    Id = new Guid("ac9b71a5-0541-4d2e-a358-6afac6d6c525"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = "Xe ưu tiên theo năm"
                });
            #endregion

            #region Fee type configuration
            modelBuilder.Entity<FeeTypeModel>().HasKey(x => x.Id);
            modelBuilder.Entity<FeeTypeModel>()
                .Property(x => x.Name)
                .HasMaxLength(50)
                .HasConversion(new EnumToStringConverter<FeeTypeEnum>());

            modelBuilder.Entity<FeeTypeModel>().HasData(
                new FeeTypeModel()
                {
                    Id = new Guid("30ee8597-aa3e-43e7-a1f1-559ee2d4b85e"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = FeeTypeEnum.Free,
                    Desc = FeeTypeEnum.Free.ToEnumMemberAttrValue(),
                    Amount = 0
                },
                new FeeTypeModel()
                {
                    Id = new Guid("46b26ea4-abfd-4b9f-bdf4-ec0e434d9ffc"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = FeeTypeEnum.Fixed,
                    Desc = FeeTypeEnum.Fixed.ToEnumMemberAttrValue(),
                    Amount = 15000
                },
                new FeeTypeModel()
                {
                    Id = new Guid("1143d8c3-22e2-4bd5-a690-89ca0c47b3c9"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = FeeTypeEnum.TimeBlock,
                    Desc = FeeTypeEnum.TimeBlock.ToEnumMemberAttrValue()
                },
                new FeeTypeModel()
                {
                    Id = new Guid("04595036-c8a8-4800-9513-c4015b98da3b"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = FeeTypeEnum.DayBlock,
                    Desc = FeeTypeEnum.DayBlock.ToEnumMemberAttrValue()
                });
            #endregion

            #region Vehicle Group configuration
            modelBuilder.Entity<VehicleGroupModel>().HasKey(x => x.Id);

            if (isDevelopment)
                modelBuilder.Entity<VehicleGroupModel>().HasData(
                    new VehicleGroupModel()
                    {
                        Id = new Guid("efbe78bc-290b-4a01-a596-bbc62f60f5f3"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        Name = "Taxi Mai Linh"
                    },
                    new VehicleGroupModel()
                    {
                        Id = new Guid("1fc5fc58-94e4-4169-a576-3cd9ecf8eb96"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        Name = "Taxi Xanh"
                    },
                    new VehicleGroupModel()
                    {
                        Id = new Guid("ec2a686b-8adc-4053-9e2e-4942cab0168d"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        Name = "Công ty vận tải hành khách"
                    });

            #endregion

            #region Time Block Fee configuration
            modelBuilder.Entity<TimeBlockFeeModel>().HasKey(x => x.Id);
            modelBuilder.Entity<TimeBlockFeeModel>()
                .HasOne(x => x.CustomVehicleType)
                .WithMany(x => x.TimeBlockFees)
                .HasForeignKey(x => x.CustomVehicleTypeId);
            modelBuilder.Entity<TimeBlockFeeModel>().HasIndex(x => x.CustomVehicleTypeId);
            modelBuilder.Entity<TimeBlockFeeModel>().HasIndex("FromSecond", "ToSecond");

            if (isDevelopment)
                modelBuilder.Entity<TimeBlockFeeModel>().HasData(
                    // Xe loại 1
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("ad21057b-6071-4e56-8949-ce60bf54f75b"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"),
                        BlockDurationInSeconds = 600,
                        FromSecond = 0,
                        ToSecond = 599,
                        Amount = 9000,
                        BlockNumber = 0
                    },
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("2e8ab3f8-8d72-4f42-831f-b0100f814a23"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"),
                        BlockDurationInSeconds = 3000,
                        FromSecond = 600,
                        ToSecond = 3599,
                        Amount = 14000,
                        BlockNumber = 1
                    },
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("0c8d860a-c5ba-473c-a3f6-95aafd295a70"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"),
                        BlockDurationInSeconds = 1800,
                        FromSecond = 3600,
                        ToSecond = 5399,
                        Amount = 21000,
                        BlockNumber = 2
                    },
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("e3772040-a29c-4a40-bd65-17d8be7211bb"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"),
                        BlockDurationInSeconds = 1800,
                        FromSecond = 5400,
                        ToSecond = 7199,
                        Amount = 28000,
                        BlockNumber = 3
                    },

                    // Xe loại 2
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("f8d3b541-2f77-4f14-bbe6-8a3028fccd07"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"),
                        BlockDurationInSeconds = 600,
                        FromSecond = 0,
                        ToSecond = 599,
                        Amount = 14000,
                        BlockNumber = 0
                    },
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("df059c09-28aa-4134-919a-e3b3041213a4"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"),
                        BlockDurationInSeconds = 3000,
                        FromSecond = 600,
                        ToSecond = 3599,
                        Amount = 19000,
                        BlockNumber = 1
                    },
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("120fd104-b6e4-403f-87d7-811ccb1c61e4"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"),
                        BlockDurationInSeconds = 1800,
                        FromSecond = 3600,
                        ToSecond = 5399,
                        Amount = 28000,
                        BlockNumber = 2
                    },
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("9302c9e0-12c2-437c-bd2d-92ed4c159e9f"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"),
                        BlockDurationInSeconds = 1800,
                        FromSecond = 5400,
                        ToSecond = 7199,
                        Amount = 37000,
                        BlockNumber = 3
                    },

                    // Xe loại 3
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("8b000abd-8e74-47a3-8a90-299dc37fac4d"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"),
                        BlockDurationInSeconds = 600,
                        FromSecond = 0,
                        ToSecond = 599,
                        Amount = 14000,
                        BlockNumber = 0
                    },
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("8585a134-fa8f-467e-8e66-f37e75444a65"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"),
                        BlockDurationInSeconds = 3000,
                        FromSecond = 600,
                        ToSecond = 3599,
                        Amount = 24000,
                        BlockNumber = 1
                    },
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("b3b643cb-488d-48f3-a167-ea9531db75ca"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"),
                        BlockDurationInSeconds = 1800,
                        FromSecond = 3600,
                        ToSecond = 5399,
                        Amount = 38000,
                        BlockNumber = 2
                    },
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("9e891bfc-2f03-4382-8b7e-6306c2757963"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"),
                        BlockDurationInSeconds = 1800,
                        FromSecond = 5400,
                        ToSecond = 7199,
                        Amount = 52000,
                        BlockNumber = 3
                    },

                    // Xe loại 4
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("f8d9683b-7cb4-4ce6-985c-5aa0a8f944e0"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"),
                        BlockDurationInSeconds = 600,
                        FromSecond = 0,
                        ToSecond = 599,
                        Amount = 24000,
                        BlockNumber = 0
                    },
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("8a8040ae-479c-4824-a0c2-3b4277d0ea9c"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"),
                        BlockDurationInSeconds = 3000,
                        FromSecond = 600,
                        ToSecond = 3599,
                        Amount = 24000,
                        BlockNumber = 1
                    },
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("adda0b07-7bd5-470b-9f89-77bb6b5cbfb2"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"),
                        BlockDurationInSeconds = 1800,
                        FromSecond = 3600,
                        ToSecond = 5399,
                        Amount = 38000,
                        BlockNumber = 2
                    },
                    new TimeBlockFeeModel()
                    {
                        Id = new Guid("3ae0b8be-525b-4ee4-9d49-d2889c6998c3"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"),
                        BlockDurationInSeconds = 1800,
                        FromSecond = 5400,
                        ToSecond = 7199,
                        Amount = 52000,
                        BlockNumber = 3
                    });
            #endregion

            #region Time Block Fee Formula configuration
            modelBuilder.Entity<TimeBlockFeeFormulaModel>().HasKey(x => x.Id);
            modelBuilder.Entity<TimeBlockFeeFormulaModel>()
                .HasOne(x => x.CustomVehicleType)
                .WithMany(x => x.TimeBlockFeeFormulas)
                .HasForeignKey(x => x.CustomVehicleTypeId);
            modelBuilder.Entity<TimeBlockFeeFormulaModel>().HasIndex(x => x.CustomVehicleTypeId);

            if (isDevelopment)
                modelBuilder.Entity<TimeBlockFeeFormulaModel>().HasData(
                    // Xe loại 1
                    new TimeBlockFeeFormulaModel()
                    {
                        Id = new Guid("667b13b4-088e-4a1a-bd36-ec15e795109b"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"),
                        FromBlockNumber = 2,
                        IntervalInSeconds = 1800,
                        Amount = 7000,
                        ApplyDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    },

                    // Xe loại 2
                    new TimeBlockFeeFormulaModel()
                    {
                        Id = new Guid("98c39b48-1248-4471-ae72-22e51e456307"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"),
                        FromBlockNumber = 2,
                        IntervalInSeconds = 1800,
                        Amount = 9000,
                        ApplyDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    },

                    // Xe loại 3
                    new TimeBlockFeeFormulaModel()
                    {
                        Id = new Guid("41369f70-ab4d-4199-a1b3-f7746fa0ff88"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"),
                        FromBlockNumber = 2,
                        IntervalInSeconds = 1800,
                        Amount = 14000,
                        ApplyDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    },

                    // Xe loại 4
                    new TimeBlockFeeFormulaModel()
                    {
                        Id = new Guid("8376b7a6-4330-4133-9e47-afd0d3f7c921"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        CustomVehicleTypeId = new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"),
                        FromBlockNumber = 2,
                        IntervalInSeconds = 1800,
                        Amount = 14000,
                        ApplyDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    });
            #endregion

            #region Fee vehicle Fee configuration
            modelBuilder.Entity<FeeVehicleCategoryModel>().HasKey(x => x.Id);
            modelBuilder.Entity<FeeVehicleCategoryModel>()
                .HasOne(x => x.VehicleCategory)
                .WithMany(x => x.FeeVehicleCategories)
                .HasForeignKey(x => x.VehicleCategoryId);
            modelBuilder.Entity<FeeVehicleCategoryModel>()
                .HasOne(x => x.VehicleGroup)
                .WithMany(x => x.FeeVehicleCategories)
                .HasForeignKey(x => x.VehicleGroupId);
            modelBuilder.Entity<FeeVehicleCategoryModel>()
                .HasOne(x => x.CustomVehicleType)
                .WithMany(x => x.FeeVehicleCategories)
                .HasForeignKey(x => x.CustomVehicleTypeId);
            modelBuilder.Entity<FeeVehicleCategoryModel>()
                .HasOne(x => x.FeeType)
                .WithMany(x => x.FeeVehicleCategories)
                .HasForeignKey(x => x.FeeTypeId);
            modelBuilder.Entity<FeeVehicleCategoryModel>().HasIndex(x => x.VehicleCategoryId);
            modelBuilder.Entity<FeeVehicleCategoryModel>().HasIndex(x => x.VehicleGroupId);
            modelBuilder.Entity<FeeVehicleCategoryModel>().HasIndex(x => x.CustomVehicleTypeId);
            modelBuilder.Entity<FeeVehicleCategoryModel>().HasIndex(x => x.FeeTypeId);
            modelBuilder.Entity<FeeVehicleCategoryModel>().HasIndex(x => x.RFID);
            modelBuilder.Entity<FeeVehicleCategoryModel>().HasIndex(x => x.PlateNumber);

            if (isDevelopment)
                modelBuilder.Entity<FeeVehicleCategoryModel>().HasData(
                    new FeeVehicleCategoryModel()
                    {
                        Id = new Guid("a15041a9-1d57-4ae3-b070-2d96aaa041ec"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        FeeTypeId = new Guid("46b26ea4-abfd-4b9f-bdf4-ec0e434d9ffc"),
                        VehicleCategoryId = new Guid("70884a61-39f3-4e8e-b936-d5b18652d3ac"),
                        VehicleGroupId = new Guid("efbe78bc-290b-4a01-a596-bbc62f60f5f3"),
                        CustomVehicleTypeId = new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"),
                        PlateNumber = "51A3268",
                        RFID = "843206065135832015",
                        ValidFrom = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        ValidTo = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                    },
                    new FeeVehicleCategoryModel()
                    {
                        Id = new Guid("1d6603bb-d361-4111-aa45-e780f50b6974"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        FeeTypeId = new Guid("46b26ea4-abfd-4b9f-bdf4-ec0e434d9ffc"),
                        VehicleCategoryId = new Guid("70884a61-39f3-4e8e-b936-d5b18652d3ac"),
                        VehicleGroupId = new Guid("1fc5fc58-94e4-4169-a576-3cd9ecf8eb96"),
                        CustomVehicleTypeId = new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"),
                        PlateNumber = "50A3008",
                        RFID = "840326156843215625",
                        ValidFrom = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        ValidTo = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                    },
                    new FeeVehicleCategoryModel()
                    {
                        Id = new Guid("a743e3e1-d6aa-49c5-a63f-28ba262bc2b8"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        FeeTypeId = new Guid("30ee8597-aa3e-43e7-a1f1-559ee2d4b85e"),
                        VehicleCategoryId = new Guid("70884a61-39f3-4e8e-b936-d5b18652d3ac"),
                        VehicleGroupId = new Guid("ec2a686b-8adc-4053-9e2e-4942cab0168d"),
                        CustomVehicleTypeId = new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"),
                        PlateNumber = "51A0968",
                        ValidFrom = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        ValidTo = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                    },
                    new FeeVehicleCategoryModel()
                    {
                        Id = new Guid("b780afae-6c9e-4730-a054-8ab8a876dffe"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        FeeTypeId = new Guid("30ee8597-aa3e-43e7-a1f1-559ee2d4b85e"),
                        VehicleCategoryId = new Guid("2b0557d0-cc6b-4fc2-a0b3-08788c9fd8c7"),
                        VehicleGroupId = new Guid("ec2a686b-8adc-4053-9e2e-4942cab0168d"),
                        CustomVehicleTypeId = new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"),
                        PlateNumber = "29A3268",
                        ValidFrom = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        ValidTo = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                    });
            #endregion


            #region Fee configuration
            modelBuilder.Entity<FeeModel>().HasKey(x => x.Id);
            modelBuilder.Entity<FeeModel>()
                .HasOne(x => x.CustomVehicleType)
                .WithMany(x => x.Fees)
                .HasForeignKey(x => x.CustomVehicleTypeId);
            modelBuilder.Entity<FeeModel>().HasIndex(x => x.CustomVehicleTypeId);
            modelBuilder.Entity<FeeModel>().HasIndex(x => x.PlateNumber);
            modelBuilder.Entity<FeeModel>().HasIndex(x => x.TicketId);
            modelBuilder.Entity<FeeModel>().HasIndex(x => x.RFID);
            modelBuilder.Entity<FeeModel>().HasIndex(x => x.LaneInId);
            modelBuilder.Entity<FeeModel>().HasIndex(x => x.LaneInDate);
            modelBuilder.Entity<FeeModel>().HasIndex(x => x.LaneInEpoch);
            modelBuilder.Entity<FeeModel>().HasIndex(x => x.LaneOutId);
            modelBuilder.Entity<FeeModel>().HasIndex(x => x.LaneOutDate);
            modelBuilder.Entity<FeeModel>().HasIndex(x => x.LaneOutEpoch);
            #endregion

            #region PaymentStatus configuration
            modelBuilder.Entity<PaymentStatusModel>().HasKey(x => x.Id);
            modelBuilder.Entity<PaymentStatusModel>()
                .HasOne(x => x.Payment)
                .WithMany(x => x.PaymentStatuses)
                .HasForeignKey(x => x.PaymentId);
            modelBuilder.Entity<PaymentStatusModel>().HasIndex(x => x.PaymentId);
            modelBuilder.Entity<PaymentStatusModel>().HasIndex(x => x.TransactionId);
            modelBuilder.Entity<PaymentStatusModel>()
               .Property(x => x.PaymentMethod)
               .HasMaxLength(50)
               .HasConversion(new EnumToStringConverter<PaymentMethodEnum>());
            modelBuilder.Entity<PaymentStatusModel>()
               .Property(x => x.Status)
               .HasMaxLength(50)
               .HasConversion(new EnumToStringConverter<PaymentStatusEnum>());
            #endregion

            #region Payment configuration
            modelBuilder.Entity<PaymentModel>().HasKey(x => x.Id);
            modelBuilder.Entity<PaymentModel>().HasIndex(x => x.FeeId);
            modelBuilder.Entity<PaymentModel>().HasIndex(x => x.LaneInId);
            modelBuilder.Entity<PaymentModel>().HasIndex(x => x.LaneOutId);
            modelBuilder.Entity<PaymentModel>().HasIndex(x => x.RFID);
            modelBuilder.Entity<PaymentModel>().HasIndex(x => x.PlateNumber);
            modelBuilder.Entity<PaymentModel>()
                .HasOne(x => x.CustomVehicleType)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.CustomVehicleTypeId);
            modelBuilder.Entity<PaymentModel>()
                .HasOne(x => x.Fee)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.FeeId);
            #endregion

            #region ETCCheckout configuration
            modelBuilder.Entity<ETCCheckoutDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<ETCCheckoutDataModel>()
                .HasOne(x => x.Payment)
                .WithMany(x => x.ETCCheckOuts)
                .HasForeignKey(x => x.PaymentId);
            modelBuilder.Entity<ETCCheckoutDataModel>().HasIndex(x => x.PaymentId);
            modelBuilder.Entity<ETCCheckoutDataModel>().HasIndex(x => x.TransactionId);
            modelBuilder.Entity<ETCCheckoutDataModel>().HasIndex("TransactionId", "RFID", "PlateNumber");
            modelBuilder.Entity<ETCCheckoutDataModel>()
               .Property(x => x.ServiceProvider)
               .HasMaxLength(50)
               .HasConversion(new EnumToStringConverter<ETCServiceProviderEnum>());
            modelBuilder.Entity<ETCCheckoutDataModel>()
               .Property(x => x.TransactionStatus)
               .HasMaxLength(50)
               .HasConversion(new EnumToStringConverter<TransactionStatusEnum>());
            #endregion

            #region AppConfig configuration
            modelBuilder.Entity<AppConfigModel>().HasIndex(x => x.IsApply);

            modelBuilder.Entity<AppConfigModel>().HasData(
                new AppConfigModel()
                {
                    Id = new Guid("2C0F4A72-0C59-4A76-A379-4BE0BC5EBD08"),
                    CreatedDate = DateTimeOffset.FromUnixTimeSeconds(1695800086).DateTime,
                    AppName = "Default app config",
                    IsApply = true,
                    HeaderHeading = "Cảng hàng không quốc tế Tân Sơn Nhất",
                    HeaderSubHeading = "CN tổng Công ty hàng không việt - CTCP",
                    HeaderLine1 = "ĐC: 58 Trường Sơn, Phường 2, Quận Tân Bình, TP. HCM",
                    HeaderLine2 = "ĐT: 123456789 MST: 0312451145112",
                    FooterLine1 = "TP HCM, ",
                    FooterLine2 = "Người nộp",
                    StationCode = "03"
                });
            #endregion
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}

