using SalonBooking.Api.DTOs.Appointments;

namespace SalonBooking.Api.Services.Interfaces;

public interface IAppointmentService
{
    Task<int> CreateAppointmentAsync(CreateAppointmentRequest request);
    Task AcceptAppointmentAsync(int appointmentId);
    Task RejectAppointmentAsync(int appointmentId, string? reason);
    Task CancelAppointmentAsync(int appointmentId, string? reason);
    Task<AppointmentListItemDto> GetAppointmentByIdAsync(int appointmentId);
    Task<List<AppointmentListItemDto>> GetAppointmentsAsync(int? status, DateOnly? date);
}