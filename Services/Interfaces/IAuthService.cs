using SalonBooking.Api.DTOs.Auth;

namespace SalonBooking.Api.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}