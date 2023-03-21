﻿using System;
using CityAPI.Entities;

namespace CityAPI.Services
{
	public interface ICityRepo
	{
		Task<IEnumerable<City>> GetAsyncCities();
		Task<City?> GetAsyncCity(int cityId, bool includePois);
		Task<IEnumerable<Poi>> GetAsyncPois(int cityId);
		Task<Poi?> GetAsyncPoi(int cityId, int poiId);
    }
}
