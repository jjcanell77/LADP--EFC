using System;
using System.Collections.Generic;

namespace LADP__EFC.Models;

public partial class Day
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BusinessHour> BusinessHours { get; set; } = new List<BusinessHour>();
}
