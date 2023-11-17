using EPAY.ETC.Core.API.Core.Models.Barcode;
using EPAY.ETC.Core.API.Core.Models.Configs;
using EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes;
using EPAY.ETC.Core.API.Core.Models.ErrorResponse;
using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.API.Core.Models.Fees;
using EPAY.ETC.Core.API.Core.Models.FeeTypes;
using EPAY.ETC.Core.API.Core.Models.FeeVehicleCategories;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.ManualBarrierControl;
using EPAY.ETC.Core.API.Core.Models.Payment;
using EPAY.ETC.Core.API.Core.Models.PrintLog;
using EPAY.ETC.Core.API.Core.Models.TicketType;
using EPAY.ETC.Core.API.Core.Models.TimeBlockFees;
using EPAY.ETC.Core.API.Core.Models.TransactionLog;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Models.VehicleCategories;
using EPAY.ETC.Core.API.Core.Models.VehicleGroups;
using EPAY.ETC.Core.Models.Constants;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics.CodeAnalysis;
using FeeTypeEnum = EPAY.ETC.Core.API.Core.Models.Enum.FeeTypeEnum;
using PaymentStatusModel = EPAY.ETC.Core.API.Core.Models.PaymentStatus.PaymentStatusModel;

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
        public virtual DbSet<ManualBarrierControlModel> ManualBarrierControls { get; set; }
        public virtual DbSet<BarcodeModel> Barcodes { get; set; }
        public virtual DbSet<ErrorResponseModel> ErrorResponses { get; set; }
        public virtual DbSet<TicketTypeModel> TicketTypes { get; set; }
        public virtual DbSet<PrintLogModel> PrintLogs { get; set; }

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

            bool isDevelopment = Environment.GetEnvironmentVariable(CoreConstant.ENVIRONMENT_BASE) == "Development";

            modelBuilder.Entity<CustomVehicleTypeModel>().HasData(
                new CustomVehicleTypeModel()
                {
                    Id = new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = CustomVehicleTypeEnum.Type1,
                    Desc = CustomVehicleTypeEnum.Type1.ToEnumMemberAttrValue(),
                    ExternalId = "030101"
                },
                new CustomVehicleTypeModel()
                {
                    Id = new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = CustomVehicleTypeEnum.Type2,
                    Desc = CustomVehicleTypeEnum.Type2.ToEnumMemberAttrValue(),
                    ExternalId = "030102"
                },
                new CustomVehicleTypeModel()
                {
                    Id = new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = CustomVehicleTypeEnum.Type3,
                    Desc = CustomVehicleTypeEnum.Type3.ToEnumMemberAttrValue(),
                    ExternalId = "030103"
                },
                new CustomVehicleTypeModel()
                {
                    Id = new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = CustomVehicleTypeEnum.Type4,
                    Desc = CustomVehicleTypeEnum.Type4.ToEnumMemberAttrValue(),
                    ExternalId = "030104"
                });
            #endregion

            #region Vehicle category configuration
            modelBuilder.Entity<VehicleCategoryModel>().HasKey(x => x.Id);

            modelBuilder.Entity<VehicleCategoryModel>().HasData(
                new VehicleCategoryModel()
                {
                    Id = new Guid("70884a61-39f3-4e8e-b936-d5b18652d3ac"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    VehicleCategoryName = "Xe nhượng quyền"
                },
                new VehicleCategoryModel()
                {
                    Id = new Guid("82f143d3-b2ed-40d6-a59e-4fc980a24450"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    VehicleCategoryName = "Xe nhượng quyền TCP"
                },
                new VehicleCategoryModel()
                {
                    Id = new Guid("2b0557d0-cc6b-4fc2-a0b3-08788c9fd8c7"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    VehicleCategoryName = "Xe ưu tiên theo tháng"
                },
                new VehicleCategoryModel()
                {
                    Id = new Guid("bd4e670d-8cae-46fa-8bac-d77ac139a044"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    VehicleCategoryName = "Xe ưu tiên theo quý"
                },
                new VehicleCategoryModel()
                {
                    Id = new Guid("ac9b71a5-0541-4d2e-a358-6afac6d6c525"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    VehicleCategoryName = "Xe ưu tiên theo năm"
                });
            #endregion

            #region Fee type configuration
            modelBuilder.Entity<FeeTypeModel>().HasKey(x => x.Id);
            modelBuilder.Entity<FeeTypeModel>()
                .Property(x => x.FeeName)
                .HasMaxLength(50)
                .HasConversion(new EnumToStringConverter<FeeTypeEnum>());

            modelBuilder.Entity<FeeTypeModel>().HasData(
                new FeeTypeModel()
                {
                    Id = new Guid("30ee8597-aa3e-43e7-a1f1-559ee2d4b85e"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    FeeName = FeeTypeEnum.Free,
                    Desc = FeeTypeEnum.Free.ToEnumMemberAttrValue(),
                    Amount = 0
                },
                new FeeTypeModel()
                {
                    Id = new Guid("46b26ea4-abfd-4b9f-bdf4-ec0e434d9ffc"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    FeeName = FeeTypeEnum.Fixed,
                    Desc = FeeTypeEnum.Fixed.ToEnumMemberAttrValue(),
                    Amount = 15000
                },
                new FeeTypeModel()
                {
                    Id = new Guid("1143d8c3-22e2-4bd5-a690-89ca0c47b3c9"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    FeeName = FeeTypeEnum.TimeBlock,
                    Desc = FeeTypeEnum.TimeBlock.ToEnumMemberAttrValue()
                },
                new FeeTypeModel()
                {
                    Id = new Guid("04595036-c8a8-4800-9513-c4015b98da3b"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    FeeName = FeeTypeEnum.DayBlock,
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
                        GroupName = "Taxi Mai Linh"
                    },
                    new VehicleGroupModel()
                    {
                        Id = new Guid("1fc5fc58-94e4-4169-a576-3cd9ecf8eb96"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        GroupName = "Taxi Xanh"
                    },
                    new VehicleGroupModel()
                    {
                        Id = new Guid("ec2a686b-8adc-4053-9e2e-4942cab0168d"),
                        CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                        GroupName = "Công ty vận tải hành khách"
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
            modelBuilder.Entity<FeeModel>()
                .HasOne(x => x.TicketType)
                .WithMany(x => x.Fees)
                .HasForeignKey(x => x.TicketTypeId);
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
            modelBuilder.Entity<FeeModel>().HasIndex(x => x.TicketTypeId);
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
                    CreatedDate = new DateTime(2023, 9, 27, 7, 34, 46, 0),
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

            #region Barcode configuration
            modelBuilder.Entity<BarcodeModel>().HasKey(x => x.Id);
            modelBuilder.Entity<BarcodeModel>()
               .Property(x => x.EmployeeId)
               .HasMaxLength(50);
            modelBuilder.Entity<BarcodeModel>()
               .Property(x => x.BarcodeAction)
               .HasMaxLength(50)
               .HasConversion(new EnumToStringConverter<BarcodeActionEnum>());

            modelBuilder.Entity<BarcodeModel>().HasData(
                new BarcodeModel()
                {
                    Id = new Guid("224874BF-0B78-41F5-A827-7DF9F3AE2412"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    ActionCode = "W6FDEZ",
                    ActionDesc = "Barcode đăng nhập cho nhân viên",
                    BarcodeAction = BarcodeActionEnum.ControlUIAccess,
                    EmployeeId = "030001"
                },
                new BarcodeModel()
                {
                    Id = new Guid("6458F220-28E0-4D6B-9367-BE6F5B6F2F2F"),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    ActionCode = "K6GRG7",
                    ActionDesc = "Barcode điều khiển barrier",
                    BarcodeAction = BarcodeActionEnum.ControlBarrier,
                    EmployeeId = "030001"
                });
            #endregion

            #region ManualBarrier configuration
            modelBuilder.Entity<ManualBarrierControlModel>().HasKey(x => x.Id);
            modelBuilder.Entity<ManualBarrierControlModel>()
               .Property(x => x.Action)
               .HasMaxLength(50)
               .HasConversion(new EnumToStringConverter<BarrierActionEnum>());
            #endregion

            #region ErrorResponse
            modelBuilder.Entity<ErrorResponseModel>().HasKey(x => x.Id);

            modelBuilder.Entity<ErrorResponseModel>().HasData(
            #region VETC
                // Checkin
                new ErrorResponseModel()
                {
                    Id = new Guid("1105a445-09cd-48f2-97f9-1cc6b9be7672"),
                    Source = "VETC",
                    Function = "Checkin",
                    Code = "400",
                    Status = "BAD_REQUEST",
                    ErrorCode = string.Empty,
                    EpayCode = "301",
                    EpayMessage = "Thiếu tham số đầu vào",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("1432aea7-f727-4282-9707-23dfbe417d53"),
                    Source = "VETC",
                    Function = "Checkin",
                    Code = "500",
                    Status = "INTERNAL_SERVER_ERROR",
                    ErrorCode = "ETAG_NOTFOUND",
                    EpayCode = "302",
                    EpayMessage = "Không tồn tại mã RFID",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("1432aea7-f727-4282-9707-23dfbe417d54"),
                    Source = "VETC",
                    Function = "Checkin",
                    Code = "500",
                    Status = "INTERNAL_SERVER_ERROR",
                    ErrorCode = "VEHICAL_NOTFOUND",
                    EpayCode = "303",
                    EpayMessage = "Không tìm thấy phương tiện trong bảng giá",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },

                // checkout
                new ErrorResponseModel()
                {
                    Id = new Guid("25cf2789-c3f8-48c1-9392-920b3ea5a0a4"),
                    Source = "VETC",
                    Function = "Checkout",
                    Code = "400",
                    Status = "BAD_REQUEST",
                    ErrorCode = string.Empty,
                    EpayCode = "301",
                    EpayMessage = "Thiếu tham số đầu vào",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("25ee8f5e-c899-4b55-a894-805dc3333023"),
                    Source = "VETC",
                    Function = "Checkout",
                    Code = "500",
                    Status = "INTERNAL_SERVER_ERROR",
                    ErrorCode = "ETAG_NOTFOUND",
                    EpayCode = "302",
                    EpayMessage = "Không tồn tại mã RFID",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("25ee8f5e-c899-4b55-a894-805dc3333024"),
                    Source = "VETC",
                    Function = "Checkout",
                    Code = "500",
                    Status = "INTERNAL_SERVER_ERROR",
                    ErrorCode = "ETAG_NOTFOUND",
                    EpayCode = "302",
                    EpayMessage = "Không tồn tại mã RFID",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("25ee8f5e-c899-4b55-a894-805dc3333025"),
                    Source = "VETC",
                    Function = "Checkout",
                    Code = "500",
                    Status = "INTERNAL_SERVER_ERROR",
                    ErrorCode = "VEHICAL_NOTFOUND",
                    EpayCode = "303",
                    EpayMessage = "Không tìm thấy phương tiện trong bảng giá",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("25ee8f5e-c899-4b55-a894-805dc3333026"),
                    Source = "VETC",
                    Function = "Checkout",
                    Code = "500",
                    Status = "INTERNAL_SERVER_ERROR",
                    ErrorCode = "PAYMENT_ERROR",
                    EpayCode = "304",
                    EpayMessage = "Thanh toán thành công",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("25ee8f5e-c899-4b55-a894-805dc3333027"),
                    Source = "VETC",
                    Function = "Checkout",
                    Code = "500",
                    Status = "INTERNAL_SERVER_ERROR",
                    ErrorCode = "NOT_ENOUGH_MONEY",
                    EpayCode = "305",
                    EpayMessage = "Tài khoản không đủ tiền",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                // Commit
                new ErrorResponseModel()
                {
                    Id = new Guid("2c1ad42f-9c67-4ed3-a2f1-f8b912acc396"),
                    Source = "VETC",
                    Function = "Commit",
                    Code = "400",
                    Status = "BAD_REQUEST",
                    ErrorCode = string.Empty,
                    EpayCode = "301",
                    EpayMessage = "Thiếu tham số đầu vào",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("378a72d0-999e-49e9-bab3-9f68bb591de9"),
                    Source = "VETC",
                    Function = "Commit",
                    Code = "500",
                    Status = "INTERNAL_SERVER_ERROR",
                    ErrorCode = "TRANSACTION_NOTFOUND",
                    EpayCode = "306",
                    EpayMessage = "Không tìm thấy giao dịch",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("378a72d0-999e-49e9-bab3-9f68bb591de8"),
                    Source = "VETC",
                    Function = "Commit",
                    Code = "500",
                    Status = "INTERNAL_SERVER_ERROR",
                    ErrorCode = "PAYMENT_ERROR",
                    EpayCode = "307",
                    EpayMessage = "Commit giao dịch không thành công",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                // Rollback
                new ErrorResponseModel()
                {
                    Id = new Guid("7fe27592-d680-41a0-a8a6-0ea9441495a0"),
                    Source = "VETC",
                    Function = "Rollback",
                    Code = "400",
                    Status = "BAD_REQUEST",
                    ErrorCode = string.Empty,
                    EpayCode = "301",
                    EpayMessage = "Thiếu tham số đầu vào",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("9ee064f5-3053-4367-822f-ecf7e2d230b0"),
                    Source = "VETC",
                    Function = "Rollback",
                    Code = "500",
                    Status = "INTERNAL_SERVER_ERROR",
                    ErrorCode = "PAYMENT_ERROR",
                    EpayCode = "308",
                    EpayMessage = "Roll back không thành công",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("9ee064f5-3053-4367-822f-ecf7e2d230b1"),
                    Source = "VETC",
                    Function = "Rollback",
                    Code = "500",
                    Status = "INTERNAL_SERVER_ERROR",
                    ErrorCode = "TRANSACTION_NOTFOUND",
                    EpayCode = "306",
                    EpayMessage = "Không tìm thấy giao dịch",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
            #endregion

            #region VDTC
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d64"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "2",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "201",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d65"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "3",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "202",
                    EpayMessage = "Vé đã tồn tại",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d66"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "4",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "203",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d67"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "5",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "204",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d68"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "6",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "205",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d70"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "7",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "206",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d69"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "6",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "205",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d71"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "7",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "206",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d72"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "8",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "207",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d46d72"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "9",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "208",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d73"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "10",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "209",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d74"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "11",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "210",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d75"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "12",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "211",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf80d45d76"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "13",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "212",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d76"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "14",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "213",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d77"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "15",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "214",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d78"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "16",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "215",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d79"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "17",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "216",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d80"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "18",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "217",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d81"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "19",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "218",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d82"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "20",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "219",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d83"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "21",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "220",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d84"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "22",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "221",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d85"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "23",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "222",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d86"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "24",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "223",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d88"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "25",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "224",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d89"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "26",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "225",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d90"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "27",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "226",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d91"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "28",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "227",
                    EpayMessage = "Không cho phép sử dụng dịch vụ",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d92"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "29",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "228",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d93"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "30",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "229",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d94"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "31",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "230",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d95"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "32",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "231",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d96"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "33",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "232",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d97"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "34",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "233",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d98"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "35",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "234",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d99"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "36",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "235",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45c10"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "37",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "236",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45c11"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "38",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "237",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45c12"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "0",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "238",
                    EpayMessage = "Tài khoản không đủ tiền",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },

            #endregion

            #region POS
                new ErrorResponseModel()
                {
                    Id = new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a62f"),
                    Source = "POS",
                    Function = string.Empty,
                    Code = "1",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "501",
                    EpayMessage = "Khách hàng hủy thanh toán",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("f8de22ef-2f65-43fc-afd5-defb86f9a63f"),
                    Source = "POS",
                    Function = string.Empty,
                    Code = "2",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "502",
                    EpayMessage = "Giao dịch thất bại",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a63f"),
                    Source = "POS",
                    Function = string.Empty,
                    Code = "3",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "503",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a64f"),
                    Source = "POS",
                    Function = string.Empty,
                    Code = "4",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "504",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a65f"),
                    Source = "POS",
                    Function = string.Empty,
                    Code = "5",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "505",
                    EpayMessage = "Tạo giao dịch bị lỗi",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a66f"),
                    Source = "POS",
                    Function = string.Empty,
                    Code = "6",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "506",
                    EpayMessage = "Lỗi đọc thẻ",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a67f"),
                    Source = "POS",
                    Function = string.Empty,
                    Code = "7",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "507",
                    EpayMessage = "Yêu cầu không hợp lệ",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a68f"),
                    Source = "POS",
                    Function = string.Empty,
                    Code = "8",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "508",
                    EpayMessage = "Lỗi hệ thống",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a69f"),
                    Source = "POS",
                    Function = string.Empty,
                    Code = "9",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "509",
                    EpayMessage = "Quá nhiều thẻ",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a70f"),
                    Source = "POS",
                    Function = string.Empty,
                    Code = "11",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "511",
                    EpayMessage = "Số trace không hợp lệ",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a71f"),
                    Source = "POS",
                    Function = string.Empty,
                    Code = "12",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "512",
                    EpayMessage = "Không được phép hủy",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("fdf50f73-ede5-4db2-82e1-5e0aa08b6c0e"),
                    Source = "VDTC",
                    Function = string.Empty,
                    Code = "13",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "513",
                    EpayMessage = "Đầy bộ nhớ",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a73f"),
                    Source = "POS",
                    Function = string.Empty,
                    Code = "14",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "514",
                    EpayMessage = "Sai định dạng",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a74f"),
                    Source = "POS",
                    Function = string.Empty,
                    Code = "15",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "515",
                    EpayMessage = "Thẻ hết hạn",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                },
                new ErrorResponseModel()
                {
                    Id = new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a75f"),
                    Source = "POS",
                    Function = string.Empty,
                    Code = "16",
                    Status = string.Empty,
                    ErrorCode = string.Empty,
                    EpayCode = "516",
                    EpayMessage = "Lỗi đọc file",
                    CreatedDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
                }
                #endregion
            );

            #endregion

            #region Ticket type
            modelBuilder.Entity<TicketTypeModel>().HasKey(x => x.Id);

            modelBuilder.Entity<TicketTypeModel>().HasData(
                new TicketTypeModel()
                {
                    Id = new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"),
                    Code = ManualBarrierTypeEnum.OneTimePass.ToString(),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = ManualBarrierTypeEnum.OneTimePass.ToDescriptionString()
                },
                new TicketTypeModel()
                {
                    Id = new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"),
                    Code = ManualBarrierTypeEnum.Priority.ToString(),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = ManualBarrierTypeEnum.Priority.ToDescriptionString()
                },
                new TicketTypeModel()
                {
                    Id = new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"),
                    Code = ManualBarrierTypeEnum.FreeEntry.ToString(),
                    CreatedDate = new DateTime(2023, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Name = ManualBarrierTypeEnum.FreeEntry.ToDescriptionString()
                });
            #endregion

            #region PrintLog configuration
            modelBuilder.Entity<PrintLogModel>().HasKey(x => x.Id);
                 modelBuilder.Entity<PrintLogModel>()
                .Property(x => x.PrintType)
                .HasConversion(new EnumToStringConverter<PrintLogEnum>());
            #endregion
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}

