namespace SalonBooking.Api.DTOs.Appointments;

public class CreateAppointmentRequest
{
    public int ServiceId { get; set; }
    public DateOnly Date { get; set; }
    public int BlockNumber { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Notes { get; set; }
}