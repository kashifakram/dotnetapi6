using System;
using CityAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityAPI.DbContexts;

public class CityContext : DbContext
{
    // Behind the scenes DbContext ensures that Cities and Poi are initialized, to avoid warning add null forgiving operator

    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Poi> Pois { get; set; } = null!;

    public CityContext(DbContextOptions<CityContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasData(
            new City("Faisalabad")
            {
                Id = 1,
                Description = "Manchester of Pakistan"
            },
            new City("Lahore")
            {
                Id = 2,
                Description = "Second biggest city of Pakistan"
            },
            new City("Karachi")
            {
                Id = 3,
                Description = "Biggest city of Pakistan"
            });

        modelBuilder.Entity<Poi>().HasData(
            new Poi("Clock Tower")
            {
                Id = 1,
                Description = "Center of the city connecting 8 major centers",
                CityId = 1
            },
            new Poi("Madina Town")
            {
                Id = 2,
                Description = "Canal road's oldest subrub",
                CityId = 1
            });
        base.OnModelCreating(modelBuilder);
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlite();
    //    base.OnConfiguring(optionsBuilder);
    //}
}