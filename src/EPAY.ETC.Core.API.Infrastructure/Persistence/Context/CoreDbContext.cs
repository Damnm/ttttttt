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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}

