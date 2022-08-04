using CarSharing.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CarSharing.Models
{
    public class CarSharingContext : DbContext
    {

        public CarSharingContext(DbContextOptions<CarSharingContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Relation>().HasKey(cr => new
            //{
            //    cr.carId,
            //    cr.reservationId,
            //    cr.customerId
            //});

            modelBuilder.Entity<Relation>().HasOne(c => c.car).WithMany(cr => cr.relations).HasForeignKey(crk => crk.carId);
            modelBuilder.Entity<Relation>().HasOne(re => re.reservation).WithOne(rre => rre.relation);
            modelBuilder.Entity<Relation>().HasOne(cu => cu.customer).WithMany(cur => cur.relations).HasForeignKey(curk => curk.customerId);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Relation> Relations { get; set; }

        public string DbPath { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("CarSharing");
            optionsBuilder.UseSqlServer(connectionString);
        }

        
    }

}
