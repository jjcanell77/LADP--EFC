using System;
using System.Collections.Generic;

namespace LADP__EFC.Models;

public partial class FoodResource
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Area { get; set; }

    public string StreetAddress { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public int Zipcode { get; set; }

    public string? Country { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? Phone { get; set; }

    public string? Website { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<BusinessHour> BusinessHours { get; set; } = new List<BusinessHour>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
