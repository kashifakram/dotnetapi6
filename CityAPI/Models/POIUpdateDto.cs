﻿using System.ComponentModel.DataAnnotations;

namespace CityAPI.Models;

public class PoiUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Description { get; set; }
}
