using CityAPI.Models;
using CityAPI.Stores;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CityAPI.Controllers;

[ApiController]
[Route("api/cities/{cityId}/pois")]
public class PoiController : ControllerBase
{
    private readonly ILogger<PoiController> _logger;
    public PoiController(ILogger<PoiController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public ActionResult<IEnumerable<PoiDto>> GetPoIs(int cityId)
    {
        var city = CityDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

        _logger.LogInformation($"city {cityId}");

        return city == null ? NotFound() : Ok(city.POI);
    }

    [HttpGet("{poiId}", Name = nameof(GetPoi))]
    public ActionResult<PoiDto> GetPoi(int cityId, int poiId)
    {
        try
        {
            var city = CityDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city != null)
            {
                var poi = city.POI.FirstOrDefault(poi => poi.Id == poiId);
                return poi == null ? NotFound() : Ok(poi);
            }

            _logger.LogInformation($"POI {poiId} in city {cityId} not found");
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"There was exception with {cityId} and {poiId}", ex);
            return StatusCode(500, "There was internal server error");
        }

    }

    [HttpPost]
    public ActionResult<PoiDto> CreateCity(int cityId, [FromBody] PoiCreateDto modelDto)
    {
        var cities = CityDataStore.Current.Cities;
        var city = cities.FirstOrDefault(c => c.Id == cityId);

        if (city == null) return NotFound();

        var lastPoiId = cities.MaxBy(c => c.Id)!.Id;

        var newPoi = new PoiDto()
        {
            Id = ++lastPoiId,
            Name = modelDto.Name,
            Description = modelDto.Description
        };

        city.POI.Add(newPoi);

        return CreatedAtRoute(nameof(GetPoi), new
        {
            cityId,
            poiId = newPoi.Id
        }, newPoi);
    }
}