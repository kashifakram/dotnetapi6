using AutoMapper;
using CityAPI.Helpers;
using CityAPI.Models;
using CityAPI.ResourceParameters;
using CityAPI.Services;
using Microsoft.AspNetCore.Mvc;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CityAPI.Controllers;

[ApiController]
[Route("api/cities")]
[ResponseCache(Duration = 60)]
//[Authorize]
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
    public async Task<ActionResult<IEnumerable<CityWithoutPioDto>>> GetCities(
        [FromQuery] CitiesResourceParameters citiesResourceParameters)
    {
        var cities = await _cityRepo.GetAsyncCities(citiesResourceParameters);

        if (!cities.Any()) return NotFound();

        var previousLink = cities.HasPrevious
            ? CreateCitiesResourceUri(citiesResourceParameters, ResourceUriType.PreviousPage)
            : null;

        var nextLink = cities.HasNext
            ? CreateCitiesResourceUri(citiesResourceParameters, ResourceUriType.NextPage)
            : null;

        var paginationMetadata = new
        {
            totalCount = cities.Count,
            pageSize = cities.PageSize,
            currentPage = cities.CurrentPage,
            totalPages = cities.TotalPages,
            previousLink,
            nextLink,
        };

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

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

    [NonAction]
    private string? CreateCitiesResourceUri(CitiesResourceParameters citiesResourceParameters,
        ResourceUriType resourceUriType)
    {
        return resourceUriType switch
        {
            ResourceUriType.NextPage => Url.Link(nameof(GetCities),
                new
                {
                    pageNumber = citiesResourceParameters.PageNumber + 1,
                    pageSize = citiesResourceParameters.PageSize,
                    searchQuery = citiesResourceParameters.SearchQuery
                }),
            ResourceUriType.PreviousPage => Url.Link(nameof(GetCities),
                new
                {
                    pageNumber = citiesResourceParameters.PageNumber - 1,
                    pageSize = citiesResourceParameters.PageSize,
                    searchQuery = citiesResourceParameters.SearchQuery
                }),
            _ => Url.Link(nameof(GetCities),
                new
                {
                    pageNumber = citiesResourceParameters.PageNumber,
                    pageSize = citiesResourceParameters.PageSize,
                    searchQuery = citiesResourceParameters.SearchQuery
                })
        };
    }
}