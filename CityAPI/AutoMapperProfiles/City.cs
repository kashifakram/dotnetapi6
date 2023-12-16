using AutoMapper;

namespace CityAPI.AutoMapperProfiles;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<Entities.City, Models.CityWithoutPioDto>();
        CreateMap<Entities.Poi, Models.PoiDto>();
    }
}