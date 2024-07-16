using System;
using System.Collections.Generic;

namespace LADP__EFC.Models;

public class FoodResource
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Area { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zipcode { get; set; }
    public string? Country { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? Description { get; set; }
    public ICollection<ResourceTags> ResourceTags { get; set; }
    public ICollection<BusinessHours> BusinessHours { get; set; }
}
