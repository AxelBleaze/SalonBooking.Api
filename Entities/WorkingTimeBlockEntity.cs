namespace SalonBooking.Api.Entities;

public class WorkingTimeBlockEntity
{
    public int Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public int BlockNumber { get; set; } // 1, 2, 3
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsEnabled { get; set; } = true;
}