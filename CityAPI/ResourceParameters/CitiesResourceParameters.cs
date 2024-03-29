﻿namespace CityAPI.ResourceParameters;

public class CitiesResourceParameters
{
    private const int maxPageSize = 20;
    private int _pageSize = 10;
    public string? MainCategory { get; set; } = "";
    public string? SearchQuery { get; set; } = "";
    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > maxPageSize ? maxPageSize : value;
    }
}