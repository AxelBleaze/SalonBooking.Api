using SalonBooking.Api.Enums;

namespace SalonBooking.Api.DTOs.Appointments;

public class AppointmentListItemDto
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ClientPhone { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? Notes { get; set; }
    public string? CancelReason { get; set; }
}