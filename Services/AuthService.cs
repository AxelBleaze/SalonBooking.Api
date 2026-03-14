using Microsoft.EntityFrameworkCore;
using SalonBooking.Api.Data;
using SalonBooking.Api.DTOs.Auth;
using SalonBooking.Api.Helpers;
using SalonBooking.Api.Services.Interfaces;

namespace SalonBooking.Api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthService(AppDbContext context, JwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.AdminUsers
            .FirstOrDefaultAsync(x => x.Username == request.Username);

        if (user == null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
            throw new Exception("Credenciales inválidas.");

        return new LoginResponse
        {
            Token = _jwtTokenGenerator.Generate(user.Username)
        };
    }
}