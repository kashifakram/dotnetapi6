using System.ComponentModel.DataAnnotations;
using CityAPI.Models;

namespace CityAPI.ValidationAttributes;

public class CityNameMustBeDifferentThanCityDesc : ValidationAttribute
{
    public CityNameMustBeDifferentThanCityDesc()
    {
        
    }
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is not CityWithoutPioDto city)
        {
            throw new Exception(
                $"Attribute {nameof(CityNameMustBeDifferentThanCityDesc)} must be applied to a {nameof(CityWithoutPioDto)} or derived type.");
        }

        return city.Name.ToLower().Equals(city.Description?.ToLower()) ? new ValidationResult("City Name should be different same from City Description", new[] {nameof(CityWithoutPioDto)}) : ValidationResult.Success;
    }
}