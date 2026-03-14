using SalonBooking.Api.DTOs.Services;

namespace SalonBooking.Api.Services.Interfaces;

public interface IServiceService
{
    Task<List<ServiceDto>> GetAllAsync(bool includeInactive = false);
    Task<ServiceDto> CreateAsync(CreateServiceRequest request);
    Task<ServiceDto> UpdateAsync(int id, UpdateServiceRequest request);
}