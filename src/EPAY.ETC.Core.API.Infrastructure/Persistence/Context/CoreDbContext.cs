using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Models.Transaction;
using EPAY.ETC.Core.API.Core.Models.TransactionLog;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Context
{
    public class CoreDbContext: DbContext
    {
        public CoreDbContext() { }
        public CoreDbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<VehicleModel> Vehicles { get; set; }
        public virtual DbSet<VehiclePaymentTransaction> VehiclePaymentTransactions { get; set; }
        public virtual DbSet<VehicleTransactionModel> VehicleTransactionModels { get; set; }
        public virtual DbSet<LaneInCameraTransactionLog> LaneInCameraTransactionLogs { get; set; }
        public virtual DbSet<LaneInRFIDTransactionLog> LaneInRFIDTransactionLogs { get; set; }
        public virtual DbSet<VehicleRequestModel> VehicleRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}

