using AutoMapper;
using CityAPI.Models;
using CityAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityAPI.Controllers;

[ApiController]
[Route("api/cities")]
[Authorize]
public class CitiesController : ControllerBase
{
    private readonly ICityRepo _cityRepo;
    private readonly IMapper _mapper;

    public CitiesController(ICityRepo cityRepo, IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
        var pois = await _cityRepo.GetAsyncPois(id);

        if (city == null) return NotFound();

        if (!string.IsNullOrEmpty(iswith))
        {
            var result = new CityDto
            {
                Id = city.Id,
                Name = city.Name,
                Description = city.Description
            };
            if (pois.Any())
                foreach (var poi in pois)
                {
                    result.POI.Add(new PoiDto
                    {
                        Id = poi.Id,
                        Name = poi.Name,
                        Description = poi.Description
                    });
                }
            return Ok(result);
        }
        else
        {
            var result = new CityWithoutPioDto
            {
                Id = city.Id,
                Name = city.Name,
                Description = city.Description
            };

            return Ok(result);
        }
    }
}