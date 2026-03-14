using Microsoft.EntityFrameworkCore;
using SalonBooking.Api.Data;
using SalonBooking.Api.DTOs.Services;
using SalonBooking.Api.Entities;
using SalonBooking.Api.Services.Interfaces;

namespace SalonBooking.Api.Services;

public class ServiceService : IServiceService
{
    private readonly AppDbContext _context;

    public ServiceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ServiceDto>> GetAllAsync(bool includeInactive = false)
    {
        var query = _context.Services.AsQueryable();

        if (!includeInactive)
            query = query.Where(x => x.IsActive);

        return await query
            .OrderBy(x => x.Name)
            .Select(x => new ServiceDto
            {
                Id = x.Id,
                Name = x.Name,
                DurationMinutes = x.DurationMinutes,
                Price = x.Price,
                IsActive = x.IsActive
            })
            .ToListAsync();
    }

    public async Task<ServiceDto> CreateAsync(CreateServiceRequest request)
    {
        var entity = new ServiceEntity
        {
            Name = request.Name,
            DurationMinutes = request.DurationMinutes,
            Price = request.Price,
            IsActive = true
        };

        _context.Services.Add(entity);
        await _context.SaveChangesAsync();

        return new ServiceDto
        {
            Id = entity.Id,
            Name = entity.Name,
            DurationMinutes = entity.DurationMinutes,
            Price = entity.Price,
            IsActive = entity.IsActive
        };
    }

    public async Task<ServiceDto> UpdateAsync(int id, UpdateServiceRequest request)
    {
        var entity = await _context.Services.FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception("Servicio no encontrado.");

        entity.Name = request.Name;
        entity.DurationMinutes = request.DurationMinutes;
        entity.Price = request.Price;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync();

        return new ServiceDto
        {
            Id = entity.Id,
            Name = entity.Name,
            DurationMinutes = entity.DurationMinutes,
            Price = entity.Price,
            IsActive = entity.IsActive
        };
    }
}