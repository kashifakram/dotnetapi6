using CityAPI.DbContexts;
using CityAPI.Entities;
using CityAPI.Helpers;
using CityAPI.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace CityAPI.Services;

public class CityRepo : ICityRepo
{
    private readonly CityContext _cityContext;
    public CityRepo(CityContext cityContext)
    {
        _cityContext = cityContext ?? throw new ArgumentNullException(nameof(cityContext));
    }

    public async Task<IEnumerable<City>> GetAsyncCities() => await _cityContext.Cities.OrderBy(c => c.Name).ToListAsync();

    public async Task<PagedList<City>> GetAsyncCities(CitiesResourceParameters citiesResourceParameters)
    {
        if (citiesResourceParameters == null) throw new ArgumentNullException(nameof(citiesResourceParameters));

        var citiesCollection = _cityContext.Cities as IQueryable<City>;

        if (!string.IsNullOrEmpty(citiesResourceParameters.SearchQuery))
        {
            var query = citiesResourceParameters.SearchQuery.Trim();
            citiesCollection = citiesCollection.Where(c =>
                c.Description != null && (c.Name.Contains(query) || c.Description.Contains(query)));
        }

        return await PagedList<City>.CreateAsync(citiesCollection, citiesResourceParameters.PageNumber,
            citiesResourceParameters.PageSize);
    }

    public async Task<City?> GetAsyncCity(int cityId, bool includePois) => !includePois ? await _cityContext.Cities.FirstOrDefaultAsync(c => c.Id == cityId) : await _cityContext.Cities.Include(c => c.POI).FirstOrDefaultAsync();

    public async Task<Poi?> GetAsyncPoi(int cityId, int poiId) => await _cityContext.Pois.FirstOrDefaultAsync(p => p.Id == poiId && p.CityId == cityId) ?? null;
    public async Task<bool> CityNameMatchesCityId(string? cityName, int cityId)
    {
        return await _cityContext.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
    }

    public async Task<IEnumerable<Poi>> GetAsyncPois(int cityId) => await _cityContext.Pois.Where(p => p.CityId == cityId).ToListAsync(); 
}