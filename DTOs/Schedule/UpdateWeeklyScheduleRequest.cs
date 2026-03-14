namespace SalonBooking.Api.DTOs.Schedule;

public class UpdateWeeklyScheduleRequest
{
    public DayOfWeek DayOfWeek { get; set; }
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
}