using CityAPI.Models;
using CityAPI.Stores;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CityAPI.Controllers;

[ApiController]
[Route("api/cities/{cityId}/pois")]
public class PoiController : ControllerBase
{
    private readonly List<CityDto> cities = CityDataStore.Current.Cities;
    private readonly ILogger<PoiController> _logger;
    public PoiController(ILogger<PoiController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public ActionResult<IEnumerable<PoiDto>> GetPoIs(int cityId)
    {
        var city = FindCity(cityId);

        _logger.LogInformation($"city {cityId}");

        return city == null ? NotFound() : Ok(city.POI);
    }

    [HttpGet("{poiId}", Name = nameof(GetPoi))]
    public ActionResult<PoiDto> GetPoi(int cityId, int poiId)
    {
        try
        {
            var city = FindCity(cityId);

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
        var city = FindCity(cityId);

        if (city == null) return NotFound();

        var lastPoiId = cities.Max(c => c.Id);

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

    [HttpPut("{poiId}")]
    public ActionResult UpdatePoi(int cityId, int poiId, PoiUpdateDto poiUpdateDto)
    {
        var city = FindCity(cityId);

        if (city == null) return NotFound();

        var existingPoi = city.POI.FirstOrDefault(p => p.Id == poiId);

        if (existingPoi == null) return NotFound();

        existingPoi.Name = poiUpdateDto.Name;
        existingPoi.Description = poiUpdateDto.Description;

        return NoContent();
    }

    [NonAction]
    private CityDto? FindCity(int cityId) => cities.FirstOrDefault(c => c.Id == cityId) ?? null;


}