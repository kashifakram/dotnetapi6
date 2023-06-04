using System;
using CityAPI.Entities;
using CityAPI.Helpers;
using CityAPI.ResourceParameters;

namespace CityAPI.Services
{
	public interface ICityRepo
	{
		Task<IEnumerable<City>> GetAsyncCities();
		Task<PagedList<City>> GetAsyncCities(CitiesResourceParameters citiesResourceParameters);
		Task<City?> GetAsyncCity(int cityId, bool includePois);
		Task<IEnumerable<Poi>> GetAsyncPois(int cityId);
		Task<Poi?> GetAsyncPoi(int cityId, int poiId);
        Task<bool> CityNameMatchesCityId(string? cityName, int cityId);

    }
}
