using System;
using System.Collections.Generic;

namespace LADP__EFC.Models;

public partial class BusinessHour
{
    public int BusinessHourId { get; set; }

    public int FoodResourceId { get; set; }

    public int? DayId { get; set; }

    public string? OpenTime { get; set; }

    public string? CloseTime { get; set; }

    public virtual Day? Day { get; set; }

    public virtual FoodResource FoodResource { get; set; } = null!;
}
