namespace CityAPI.Models;

public interface ICityDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }
}   
