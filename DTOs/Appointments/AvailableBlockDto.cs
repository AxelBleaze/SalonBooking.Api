namespace SalonBooking.Api.DTOs.Appointments;

public class AvailableBlockDto
{
    public int BlockNumber { get; set; }
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public bool Available { get; set; }
}