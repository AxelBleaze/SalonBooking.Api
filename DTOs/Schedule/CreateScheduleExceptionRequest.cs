using SalonBooking.Api.Enums;

namespace SalonBooking.Api.DTOs.Schedule;

public class CreateScheduleExceptionRequest
{
    public DateOnly Date { get; set; }
    public ScheduleExceptionType Type { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public bool AllDay { get; set; }
    public string? Reason { get; set; }
}