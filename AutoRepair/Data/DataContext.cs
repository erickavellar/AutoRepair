using AutoRepair.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<District> Districts { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<VatRate> VatRates { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleDetail> ScheduleDetails { get; set; }
        public DbSet<ScheduleDetailTemp> ScheduleDetailsTemp { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<District>()
               .HasIndex(c => c.Name)
               .IsUnique();

            
            modelBuilder.Entity<Service>().Property(s => s.CostPrice).HasColumnType("decimal(18,2");
            modelBuilder.Entity<Service>().Property(s => s.SalePrice).HasColumnType("decimal(18,2");




            base.OnModelCreating(modelBuilder);
        }



        
    }
}
