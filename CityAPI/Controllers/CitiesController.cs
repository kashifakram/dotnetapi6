using CityAPI.Models;
using CityAPI.Stores;
using Microsoft.AspNetCore.Mvc;

namespace CityAPI.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    private readonly CityDataStore _cityDataStore;

    private readonly List<CityDto> cities;

    public CitiesController(CityDataStore cityDataStore)
    {
        _cityDataStore = cityDataStore ?? throw new ArgumentNullException(nameof(cityDataStore));

        cities = _cityDataStore.Cities ?? new List<CityDto>();
    }

    [HttpGet]
    public ActionResult<IEnumerable<CityDto>> GetCities()
    {
        return cities.Count > 0 ? Ok(cities) : NotFound();
    }

    [HttpGet("{id}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        var city = cities.FirstOrDefault(city => city.Id == id);

        return city == null ? NotFound() : Ok(city);
    }


}