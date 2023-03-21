namespace CityAPI.Models;

public class CityWithoutPioDto : ICityDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}   
