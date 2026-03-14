namespace SalonBooking.Api.Entities;

public class ClientEntity
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }

    public ICollection<AppointmentEntity> Appointments { get; set; } = new List<AppointmentEntity>();
}