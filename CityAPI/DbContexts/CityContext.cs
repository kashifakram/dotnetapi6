using System;
using CityAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityAPI.DbContexts
{
	public class CityContext : DbContext
	{
		// Behind the scenes DbContext ensures that Cities and Poi are initialized, to avoid warning add null forgiving operator

		public List<City> Cities { get; set; } = null!;
        public List<Poi> Pois { get; set; } = null!;

        public CityContext(DbContextOptions<CityContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite();
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}

