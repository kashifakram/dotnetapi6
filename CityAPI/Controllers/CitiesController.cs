using CityAPI.Models;
using CityAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityAPI.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    private readonly ICityRepo _cityRepo; 

    public CitiesController(ICityRepo cityRepo)
    {
        _cityRepo = cityRepo ?? throw new ArgumentNullException(nameof(cityRepo));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPioDto>>> GetCities()
    {
        var cities = await _cityRepo.GetAsyncCities();
        if (!cities.Any()) return NotFound();

        var results = new List<CityWithoutPioDto>();
        foreach (var city in cities)
        {
            results.Add(new CityWithoutPioDto
            {
                Id = city.Id,
                Name = city.Name,
                Description = city.Description
            });
        }
        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ICityDto>> GetCity(int id)
    {
        var iswith = Request.Query["needpoi"].ToString();

        var city = await _cityRepo.GetAsyncCity(id, false);
        if (city == null) return NotFound();

        var result = new CityDto
        {
            Id = city.Id,
            Name = city.Name,
            Description = city.Description
        };

        if (!string.IsNullOrEmpty(iswith))
        {
            foreach (var poi in city.POI)
            {
                result.POI.Add(new PoiDto
                {
                    Id = poi.Id,
                    Name = poi.Name,
                    Description = poi.Description
                });
            }

        }

        return Ok(result);

    }
}