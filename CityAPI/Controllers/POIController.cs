using Microsoft.AspNetCore.Mvc;

namespace CityAPI.Controllers;

[ApiController]
[Route("api/cities/{cityid}/pois")]
public class POIController : ControllerBase
{
    private readonly ILogger<POIController> _logger;
    public POIController(ILogger<POIController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public ActionResult<IEnumerable<POIDto>> GetPOIs(int cityid)
    {
        var city = CityDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityid);

        _logger.LogInformation($"city {cityid}");

        return city == null ? NotFound() : Ok(city.POI);
    }

    [HttpGet("{poiId}")]
    public ActionResult<POIDto> GetPOI(int cityid, int poiId)
    {
        try
        {
            var city = CityDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityid);

            if (city != null)
            {
                var poi = city.POI.FirstOrDefault(poi => poi.Id == poiId);
                return poi == null ? NotFound() : Ok(poi);
            }

            _logger.LogInformation($"POI {poiId} in city {cityid} not found");
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"There was exception with {cityid} and {poiId}", ex);
            return StatusCode(500, "There was internal server error");
        }
        
    }
}