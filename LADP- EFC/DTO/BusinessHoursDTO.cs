namespace LADP__EFC.DTO
{
    public class BusinessHoursDTO
    {
        public DayDTO Day { get; set; } = null!;
        public string? OpenTime { get; set; }
        public string? CloseTime { get; set; }
    }
}
