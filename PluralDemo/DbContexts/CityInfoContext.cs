using Microsoft.EntityFrameworkCore;
using PluralDemo.Entities;

namespace PluralDemo.DbContexts {
    public class CityInfoContext: DbContext {

        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointOfInterests { get; set; } = null!;

        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options) { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {


            modelBuilder.Entity<City>()
                .HasData(
                    new City("Athens") {
                        Id = 1,
                        Description = "Trash Everywhere"
                    },
                    new City("New York") {
                        Id = 2,
                        Description = "Would Love to Visit"
                    },
                    new City("Paris") {
                        Id = 3,
                        Description = "Been there done that"
                    });

            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                    new PointOfInterest("Empire State Building") {
                        Id = 1,
                        Description = "Big Building",
                        CityId = 2
                    },
                    new PointOfInterest("The Louvre") {
                        Id = 2,
                        CityId = 3,
                        Description = "Nice Museum"
                    },
                    new PointOfInterest("The Eifel Tower") {
                        Id = 3,
                        CityId  = 3,
                        Description = "Cool Tower"
                    }
                    );

            base.OnModelCreating(modelBuilder);

        }
    }
}
