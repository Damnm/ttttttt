﻿// <auto-generated />
using System;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    [DbContext(typeof(CoreDbContext))]
    [Migration("20230905033749_databse")]
    partial class databse
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EPAY.ETC.Core.API.Core.Models.Common.VehicleRequestModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Make")
                        .HasColumnType("text");

                    b.Property<string>("PlateColor")
                        .HasColumnType("text");

                    b.Property<string>("PlateNumber")
                        .HasColumnType("text");

                    b.Property<string>("RFID")
                        .HasColumnType("text");

                    b.Property<int?>("Seat")
                        .HasColumnType("integer");

                    b.Property<string>("VehicleType")
                        .HasColumnType("text");

                    b.Property<int?>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("VehicleRequests");
                });

            modelBuilder.Entity("EPAY.ETC.Core.API.Core.Models.Fusion.FusionModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Cam1")
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<string>("Cam2")
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<float>("Epoch")
                        .HasColumnType("real");

                    b.Property<bool>("Loop1")
                        .HasColumnType("boolean");

                    b.Property<bool>("Loop2")
                        .HasColumnType("boolean");

                    b.Property<bool>("Loop3")
                        .HasColumnType("boolean");

                    b.Property<bool>("RFID")
                        .HasColumnType("boolean");

                    b.Property<bool>("ReversedLoop1")
                        .HasColumnType("boolean");

                    b.Property<bool>("ReversedLoop2")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Fusions");
                });

            modelBuilder.Entity("EPAY.ETC.Core.API.Core.Models.Transaction.VehiclePaymentTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsSuccessful")
                        .HasColumnType("boolean");

                    b.Property<string>("PaymentType")
                        .HasColumnType("text");

                    b.Property<Guid>("VehicleTransactionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("VehiclePaymentTransactions");
                });

            modelBuilder.Entity("EPAY.ETC.Core.API.Core.Models.Transaction.VehicleTransactionModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<Guid>("ExternalEmployeeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LaneInDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("LaneInId")
                        .HasColumnType("uuid");

                    b.Property<string>("LaneInPlateNumberPhotoURL")
                        .HasColumnType("text");

                    b.Property<string>("LaneInVehiclePhotoURL")
                        .HasColumnType("text");

                    b.Property<DateTime>("LaneOutDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("LaneOutId")
                        .HasColumnType("uuid");

                    b.Property<string>("LaneOutPlateNumberPhotoURL")
                        .HasColumnType("text");

                    b.Property<string>("LaneOutVehiclePhotoURL")
                        .HasColumnType("text");

                    b.Property<string>("PaymentMethod")
                        .HasColumnType("text");

                    b.Property<string>("PlateNumber")
                        .HasColumnType("text");

                    b.Property<string>("RFID")
                        .HasColumnType("text");

                    b.Property<Guid>("ShiftId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("VehicleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("VehicleTransactionModels");
                });

            modelBuilder.Entity("EPAY.ETC.Core.API.Core.Models.TransactionLog.LaneInCameraTransactionLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CameraReaderIPAddr")
                        .HasColumnType("text");

                    b.Property<string>("CameraReaderMacAddr")
                        .HasColumnType("text");

                    b.Property<double>("ConfidenceScore")
                        .HasColumnType("double precision");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Epoch")
                        .HasColumnType("double precision");

                    b.Property<Guid>("LaneInId")
                        .HasColumnType("uuid");

                    b.Property<string>("Make")
                        .HasColumnType("text");

                    b.Property<string>("Model")
                        .HasColumnType("text");

                    b.Property<string>("PlateColour")
                        .HasColumnType("text");

                    b.Property<string>("PlateNumber")
                        .HasColumnType("text");

                    b.Property<string>("PlateNumberPhotoUrl")
                        .HasColumnType("text");

                    b.Property<string>("RFID")
                        .HasColumnType("text");

                    b.Property<int?>("Seat")
                        .HasColumnType("integer");

                    b.Property<string>("VehiclePhotoUrl")
                        .HasColumnType("text");

                    b.Property<string>("VehicleType")
                        .HasColumnType("text");

                    b.Property<int?>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("LaneInCameraTransactionLogs");
                });

            modelBuilder.Entity("EPAY.ETC.Core.API.Core.Models.TransactionLog.LaneInRFIDTransactionLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("ConfidenceScore")
                        .HasColumnType("double precision");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Epoch")
                        .HasColumnType("double precision");

                    b.Property<Guid>("LaneInId")
                        .HasColumnType("uuid");

                    b.Property<string>("Make")
                        .HasColumnType("text");

                    b.Property<string>("Model")
                        .HasColumnType("text");

                    b.Property<string>("PlateColour")
                        .HasColumnType("text");

                    b.Property<string>("PlateNumber")
                        .HasColumnType("text");

                    b.Property<string>("PlateNumberPhotoUrl")
                        .HasColumnType("text");

                    b.Property<string>("RFID")
                        .HasColumnType("text");

                    b.Property<string>("RFIDReaderIPAddr")
                        .HasColumnType("text");

                    b.Property<string>("RFIDReaderMacAddr")
                        .HasColumnType("text");

                    b.Property<int>("Seat")
                        .HasColumnType("integer");

                    b.Property<string>("VehiclePhotoUrl")
                        .HasColumnType("text");

                    b.Property<string>("VehicleType")
                        .HasColumnType("text");

                    b.Property<int>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("LaneInRFIDTransactionLogs");
                });

            modelBuilder.Entity("EPAY.ETC.Core.API.Core.Models.Vehicle.VehicleModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Make")
                        .HasColumnType("text");

                    b.Property<string>("PlateColor")
                        .HasColumnType("text");

                    b.Property<string>("PlateNumber")
                        .HasColumnType("text");

                    b.Property<string>("RFID")
                        .HasColumnType("text");

                    b.Property<int?>("Seat")
                        .HasColumnType("integer");

                    b.Property<string>("VehicleType")
                        .HasColumnType("text");

                    b.Property<int?>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Vehicles");
                });
#pragma warning restore 612, 618
        }
    }
}
