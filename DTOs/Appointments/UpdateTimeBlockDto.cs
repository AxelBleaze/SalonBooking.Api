namespace SalonBooking.Api.DTOs.Schedule;

public class UpdateTimeBlockDto
{
    public int BlockNumber { get; set; }
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
}