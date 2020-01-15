using Rates.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Rates.API.DbContexts
{
    public class UserLibraryContext : DbContext
    {
        public UserLibraryContext(DbContextOptions<UserLibraryContext> options)
           : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Agreement> Agreements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // seed the database with dummy data
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 67812203006,
                    FirstName = "Goras",
                    LastName = "Trusevičius",
                },
                new User
                {
                    Id = 78706151287,
                    FirstName = "Dangė",
                    LastName = "Kulkavičiutė",
                }
                );
            modelBuilder.Entity<Agreement>().HasData(
                new Agreement
                {
                    Id = 1,
                    UserId = 67812203006,
                    Amount = 12000,
                    BaseRateCode = Enums.BaseRateCodes.VILIBOR3m.ToString(),
                    Margin = 1.6m,
                    Duration = 60
                },
                new Agreement
                {
                    Id = 2,
                    UserId = 78706151287,
                    Amount = 8000,
                    BaseRateCode = Enums.BaseRateCodes.VILIBOR1y.ToString(),
                    Margin = 2.2m,
                    Duration = 36
                },
                new Agreement
                {
                    Id =3,
                    UserId = 78706151287,
                    Amount = 1000,
                    BaseRateCode = Enums.BaseRateCodes.VILIBOR6m.ToString(),
                    Margin = 1.85m,
                    Duration = 24
                }
                );

           base.OnModelCreating(modelBuilder);
        }
    }
}
