namespace SalonBooking.Api.Entities;

public class WorkingDayScheduleEntity
{
    public int Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsEnabled { get; set; } = true;
}