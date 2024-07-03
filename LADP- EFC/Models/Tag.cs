using System;
using System.Collections.Generic;

namespace LADP__EFC.Models;

public partial class Tag
{
    public int Id { get; set; }

    public string? Tag1 { get; set; }

    public virtual ICollection<FoodResource> FoodResources { get; set; } = new List<FoodResource>();
}
