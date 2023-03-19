namespace CityAPI.Models;

public class CityDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int TotalPoIs => POI.Count;

    public ICollection<PoiDto> POI { get; set; } = new List<PoiDto>();
}   
