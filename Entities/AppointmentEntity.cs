using SalonBooking.Api.Enums;

namespace SalonBooking.Api.Entities;

public class AppointmentEntity
{
    public int Id { get; set; }

    public int ServiceId { get; set; }
    public ServiceEntity Service { get; set; } = null!;

    public int ClientId { get; set; }
    public ClientEntity Client { get; set; } = null!;

    public int BlockNumber { get; set; }

    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

    public string? Notes { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DecisionAt { get; set; }
    public string? CancelReason { get; set; }
}