using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LADP__EFC.Models;

public class Days
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<BusinessHours> BusinessHours { get; set; }
}