using CityAPI.Models;
using CityAPI.Stores;
using Microsoft.AspNetCore.Mvc;

namespace CityAPI.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<CityDto>> GetCities()
    {
        return CityDataStore.Current.Cities.Count > 0 ? Ok(CityDataStore.Current.Cities) : NotFound();
    }

    [HttpGet("{id}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        var city = CityDataStore.Current.Cities.FirstOrDefault(city => city.Id == id);

        return city == null ? NotFound() : Ok(city);
    }


}