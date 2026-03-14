namespace SalonBooking.Api.DTOs.Schedule;

public class WorkingTimeBlockDto
{
    public int Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public int BlockNumber { get; set; }
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
}