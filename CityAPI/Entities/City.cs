using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityAPI.Entities;

public class City
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(100)]    
    public string? Description { get; set; }

    public ICollection<Poi> POI { get; set; } = new List<Poi>();

    public City(string name)
    {
        Name = name;
    }
}   
