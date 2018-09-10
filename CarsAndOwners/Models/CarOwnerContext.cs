using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CarsAndOwners.Models
{
    public class CarOwnerContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public CarOwnerContext() : base("OwnerDb") { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Owner>().HasMany(o => o.Cars).WithMany(c => c.Owners)
                .Map(m =>
                {
                    m.ToTable("CarOwners");
                    m.MapLeftKey("Owners");
                    m.MapRightKey("Cars");
                });
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}