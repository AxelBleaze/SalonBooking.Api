using SalonBooking.Api.DTOs.Appointments;

namespace SalonBooking.Api.Services.Interfaces;

public interface IAvailabilityService
{
    Task<List<AvailabilitySlotDto>> GetAvailableSlotsAsync(DateOnly date, int serviceId);
}