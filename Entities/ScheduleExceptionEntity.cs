using SalonBooking.Api.Enums;

namespace SalonBooking.Api.Entities;

public class ScheduleExceptionEntity
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public ScheduleExceptionType Type { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public bool AllDay { get; set; }
    public string? Reason { get; set; }
}