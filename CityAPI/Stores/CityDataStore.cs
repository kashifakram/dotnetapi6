using CityAPI.Models;

namespace CityAPI.Stores
{
    public class CityDataStore
    {
        public List<CityDto> Cities { get; set; }

        public static CityDataStore Current { get; } = new CityDataStore();

        public CityDataStore()
        {
            Cities = new List<CityDto>
            {
                new CityDto
                {
                    Id = 1,
                    Name = "Faisalabad",
                    Description = "Third in Pakistan",
                    POI = new List<PoiDto>
                    {
                        new PoiDto { Id = 1, Name = "Central Park", Description = "Most visited park" },
                        new PoiDto { Id = 2, Name = "Clock Tower", Description = "City Center" },
                    }
                },
                new CityDto
                {
                    Id = 2,
                    Name = "Lahore",
                    Description = "Second in Pakistan",
                    POI = new List<PoiDto>
                    {
                        new PoiDto { Id = 1, Name = "Pakistan Tower", Description = "Most visited tower" },
                        new PoiDto { Id = 2, Name = "Red for", Description = "City Entrance" },
                    }
                }
            };
        }
    }
}
