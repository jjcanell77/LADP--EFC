using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LADP__EFC.Models;

public class BusinessHours
{
    public int BusinessHourID { get; set; }
    public int FoodResourceID { get; set; }
    public FoodResource FoodResource { get; set; }
    public int DayId { get; set; }
    public Days Day { get; set; }
    public string OpenTime { get; set; }
    public string CloseTime { get; set; }
}
