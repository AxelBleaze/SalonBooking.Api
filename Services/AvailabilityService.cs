using Microsoft.EntityFrameworkCore;
using SalonBooking.Api.Data;
using SalonBooking.Api.DTOs.Appointments;
using SalonBooking.Api.Enums;
using SalonBooking.Api.Services.Interfaces;

namespace SalonBooking.Api.Services;

public class AvailabilityService : IAvailabilityService
{
    private readonly AppDbContext _context;
    private const int SlotStepMinutes = 30;

    public AvailabilityService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AvailabilitySlotDto>> GetAvailableSlotsAsync(DateOnly date, int serviceId)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(x => x.Id == serviceId && x.IsActive);

        if (service == null)
            return new List<AvailabilitySlotDto>();

        var blocks = await _context.WorkingTimeBlocks
            .Where(x => x.DayOfWeek == date.DayOfWeek && x.IsEnabled)
            .OrderBy(x => x.BlockNumber)
            .ToListAsync();

        if (!blocks.Any())
            return new List<AvailabilitySlotDto>();

        var exception = await _context.ScheduleExceptions
            .FirstOrDefaultAsync(x => x.Date == date);

        if (exception != null && exception.Type == ScheduleExceptionType.Blocked && exception.AllDay)
            return new List<AvailabilitySlotDto>();

        var dayStart = new DateTimeOffset(date.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
        var dayEnd = dayStart.AddDays(1);

        var appointments = await _context.Appointments
            .Where(x =>
                x.StartAt >= dayStart &&
                x.StartAt < dayEnd &&
                (x.Status == AppointmentStatus.Pending || x.Status == AppointmentStatus.Accepted))
            .ToListAsync();

        var result = new List<AvailabilitySlotDto>();
        var duration = TimeSpan.FromMinutes(service.DurationMinutes);

        foreach (var block in blocks)
        {
            var blockStart = new DateTimeOffset(date.ToDateTime(block.StartTime), TimeSpan.Zero);
            var blockEnd = new DateTimeOffset(date.ToDateTime(block.EndTime), TimeSpan.Zero);

            for (var current = blockStart; current.Add(duration) <= blockEnd; current = current.AddMinutes(SlotStepMinutes))
            {
                var currentEnd = current.Add(duration);

                var overlapsAppointment = appointments.Any(a =>
                    current < a.EndAt && currentEnd > a.StartAt);

                var overlapsBlockedRange = false;

                if (exception != null &&
                    exception.Type == ScheduleExceptionType.Blocked &&
                    !exception.AllDay &&
                    exception.StartTime.HasValue &&
                    exception.EndTime.HasValue)
                {
                    var blockedStart = new DateTimeOffset(date.ToDateTime(exception.StartTime.Value), TimeSpan.Zero);
                    var blockedEnd = new DateTimeOffset(date.ToDateTime(exception.EndTime.Value), TimeSpan.Zero);

                    overlapsBlockedRange = current < blockedEnd && currentEnd > blockedStart;
                }

                if (!overlapsAppointment && !overlapsBlockedRange)
                {
                    result.Add(new AvailabilitySlotDto
                    {
                        StartAt = current,
                        EndAt = currentEnd,
                        Available = true
                    });
                }
            }
        }

        return result.OrderBy(x => x.StartAt).ToList();
    }
}