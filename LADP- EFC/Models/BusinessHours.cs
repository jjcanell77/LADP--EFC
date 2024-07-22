using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LADP__EFC.Models;

public class BusinessHours
{
    public int BusinessHourId { get; set; }
    public int FoodResourceId { get; set; }
    public int DayId { get; set; }
    public string? OpenTime { get; set; }
    public string? CloseTime { get; set; }

    public Day Day { get; set; } = null!; // Many-to-One relationship with Days
    public FoodResource FoodResource { get; set; } = null!; // Many-to-One relationship with FoodResource
}