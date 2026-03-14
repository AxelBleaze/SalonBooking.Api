namespace SalonBooking.Api.DTOs.Appointments;

public class AvailabilitySlotDto
{
    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }
    public bool Available { get; set; }
}