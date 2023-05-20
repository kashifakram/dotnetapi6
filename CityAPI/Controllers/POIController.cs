using CityAPI.Models;
using CityAPI.Services;
using CityAPI.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityAPI.Controllers;

[ApiController]
[Route("api/cities/{cityId}/pois")]
[Authorize]
public class PoiController : ControllerBase
{
    private readonly ILogger<PoiController> _logger;
    private readonly IMailService _localMailService;
    private readonly CityDataStore _cityDataStore;

    private readonly List<CityDto> cities;

    public PoiController(ILogger<PoiController> logger, IMailService localMailService, CityDataStore cityDataStore)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));
        _cityDataStore = cityDataStore ?? throw new ArgumentNullException(nameof(cityDataStore));
        cities = _cityDataStore.Cities;
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

        var existingPoi = FindPoi(city, poiId);

        if (existingPoi == null) return NotFound();

        existingPoi.Name = poiUpdateDto.Name;
        existingPoi.Description = poiUpdateDto.Description;

        return NoContent();
    }

    [HttpPatch("{poiId}")]
    public ActionResult PartialUpdate(int cityId, int poiId, JsonPatchDocument<PoiUpdateDto> jsonPatchDocument)
    {
        var city = FindCity(cityId);

        if (city == null) return NotFound();

        var existingPoi = FindPoi(city, poiId);

        if (existingPoi == null) return NotFound();

        var newPoi = new PoiUpdateDto()
        {
            Name = existingPoi.Name,
            Description = existingPoi.Description
        };

        jsonPatchDocument.ApplyTo(newPoi, ModelState);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!TryValidateModel(newPoi)) return BadRequest(ModelState);

        existingPoi.Name = newPoi.Name;
        existingPoi.Description = newPoi.Description;

        return NoContent();
    }

    [HttpDelete("{poiId}")]
    public ActionResult DeletePoi(int cityId, int poiId)
    {
        var city = FindCity(cityId);

        if (city == null) return NotFound();

        var existingPoi = FindPoi(city, poiId);

        if (existingPoi == null) return NotFound();

        city.POI.Remove(existingPoi);

        _localMailService.SendMail($"POI {poiId} has been deleted", $"POI {poiId} has been deleted from {cityId}");

        return NoContent();
    }

    [NonAction]
    private CityDto? FindCity(int cityId) => cities.FirstOrDefault(c => c.Id == cityId) ?? null;


    [NonAction]
    private static PoiDto? FindPoi(CityDto city, int poiId) => city.POI.FirstOrDefault(p => p.Id == poiId) ?? null;

}