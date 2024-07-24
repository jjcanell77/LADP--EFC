using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LADP__EFC.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<FoodResource> FoodResource { get; } = [];  // Many-to-One relationship with ResourceTags
}