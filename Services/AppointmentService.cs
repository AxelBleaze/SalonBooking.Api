using Microsoft.EntityFrameworkCore;
using SalonBooking.Api.Data;
using SalonBooking.Api.DTOs.Appointments;
using SalonBooking.Api.Entities;
using SalonBooking.Api.Enums;
using SalonBooking.Api.Services.Interfaces;

namespace SalonBooking.Api.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _context;

    public AppointmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAppointmentAsync(CreateAppointmentRequest request)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(x => x.Id == request.ServiceId && x.IsActive)
            ?? throw new Exception("El servicio no existe.");

        if (request.BlockNumber < 1 || request.BlockNumber > 3)
            throw new Exception("El número de bloque debe ser 1, 2 o 3.");

        var block = await _context.WorkingTimeBlocks
            .FirstOrDefaultAsync(x =>
                x.DayOfWeek == request.Date.DayOfWeek &&
                x.BlockNumber == request.BlockNumber &&
                x.IsEnabled);

        if (block == null)
            throw new Exception("El bloque seleccionado no está disponible para ese día.");

        var exception = await _context.ScheduleExceptions
            .FirstOrDefaultAsync(x => x.Date == request.Date);

        if (exception != null && exception.Type == ScheduleExceptionType.Blocked && exception.AllDay)
            throw new Exception("La fecha seleccionada no está disponible.");

        var startAt = new DateTimeOffset(
            request.Date.ToDateTime(block.StartTime),
            TimeSpan.Zero);

        var endAt = new DateTimeOffset(
            request.Date.ToDateTime(block.EndTime),
            TimeSpan.Zero);

        if (exception != null &&
            exception.Type == ScheduleExceptionType.Blocked &&
            !exception.AllDay &&
            exception.StartTime.HasValue &&
            exception.EndTime.HasValue)
        {
            var blockedStart = new DateTimeOffset(
                request.Date.ToDateTime(exception.StartTime.Value),
                TimeSpan.Zero);

            var blockedEnd = new DateTimeOffset(
                request.Date.ToDateTime(exception.EndTime.Value),
                TimeSpan.Zero);

            var overlapsBlockedRange = startAt < blockedEnd && endAt > blockedStart;

            if (overlapsBlockedRange)
                throw new Exception("El bloque seleccionado está bloqueado para esa fecha.");
        }

        var dayStart = new DateTimeOffset(
            request.Date.ToDateTime(TimeOnly.MinValue),
            TimeSpan.Zero);

        var dayEnd = dayStart.AddDays(1);

        var blockAlreadyTaken = await _context.Appointments.AnyAsync(x =>
            x.BlockNumber == request.BlockNumber &&
            x.StartAt >= dayStart &&
            x.StartAt < dayEnd &&
            (x.Status == AppointmentStatus.Pending || x.Status == AppointmentStatus.Accepted));

        if (blockAlreadyTaken)
            throw new Exception("Ese bloque ya fue solicitado por otro usuario.");

        var client = await _context.Clients
            .FirstOrDefaultAsync(x => x.Phone == request.Phone);

        if (client == null)
        {
            client = new ClientEntity
            {
                FullName = request.FullName,
                Phone = request.Phone
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }
        else
        {
            client.FullName = request.FullName;
        }

        var appointment = new AppointmentEntity
        {
            ServiceId = request.ServiceId,
            ClientId = client.Id,
            BlockNumber = request.BlockNumber,
            StartAt = startAt,
            EndAt = endAt,
            Notes = request.Notes,
            Status = AppointmentStatus.Pending,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        return appointment.Id;
    }

    public async Task AcceptAppointmentAsync(int appointmentId)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(x => x.Id == appointmentId)
            ?? throw new Exception("Turno no encontrado.");

        if (appointment.Status != AppointmentStatus.Pending)
            throw new Exception("Solo se pueden aceptar turnos pendientes.");

        var hasConflict = await _context.Appointments.AnyAsync(x =>
            x.Id != appointment.Id &&
            x.BlockNumber == appointment.BlockNumber &&
            x.StartAt == appointment.StartAt &&
            x.EndAt == appointment.EndAt &&
            x.Status == AppointmentStatus.Accepted);

        if (hasConflict)
            throw new Exception("Este bloque ya fue aceptado para otro turno.");

        appointment.Status = AppointmentStatus.Accepted;
        appointment.DecisionAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task RejectAppointmentAsync(int appointmentId, string? reason)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(x => x.Id == appointmentId)
            ?? throw new Exception("Turno no encontrado.");

        if (appointment.Status != AppointmentStatus.Pending)
            throw new Exception("Solo se pueden rechazar turnos pendientes.");

        appointment.Status = AppointmentStatus.Rejected;
        appointment.DecisionAt = DateTimeOffset.UtcNow;
        appointment.CancelReason = reason;

        await _context.SaveChangesAsync();
    }

    public async Task CancelAppointmentAsync(int appointmentId, string? reason)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(x => x.Id == appointmentId)
            ?? throw new Exception("Turno no encontrado.");

        if (appointment.Status != AppointmentStatus.Accepted)
            throw new Exception("Solo se pueden cancelar turnos aceptados.");

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.DecisionAt = DateTimeOffset.UtcNow;
        appointment.CancelReason = reason;

        await _context.SaveChangesAsync();
    }

    public async Task<AppointmentListItemDto> GetAppointmentByIdAsync(int appointmentId)
    {
        var appointment = await _context.Appointments
            .Include(x => x.Client)
            .Include(x => x.Service)
            .Where(x => x.Id == appointmentId)
            .Select(x => new AppointmentListItemDto
            {
                Id = x.Id,
                ClientName = x.Client.FullName,
                ClientPhone = x.Client.Phone,
                ServiceName = x.Service.Name,
                StartAt = x.StartAt,
                EndAt = x.EndAt,
                Status = x.Status,
                Notes = x.Notes,
                CancelReason = x.CancelReason
            })
            .FirstOrDefaultAsync()
            ?? throw new Exception("Turno no encontrado.");

        return appointment;
    }

    public async Task<List<AppointmentListItemDto>> GetAppointmentsAsync(int? status, DateOnly? date)
    {
        var query = _context.Appointments
            .Include(x => x.Client)
            .Include(x => x.Service)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(x => (int)x.Status == status.Value);

        if (date.HasValue)
        {
            var start = new DateTimeOffset(date.Value.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
            var end = start.AddDays(1);

            query = query.Where(x => x.StartAt >= start && x.StartAt < end);
        }

        return await query
            .OrderBy(x => x.StartAt)
            .Select(x => new AppointmentListItemDto
            {
                Id = x.Id,
                ClientName = x.Client.FullName,
                ClientPhone = x.Client.Phone,
                ServiceName = x.Service.Name,
                StartAt = x.StartAt,
                EndAt = x.EndAt,
                Status = x.Status,
                Notes = x.Notes,
                CancelReason = x.CancelReason
            })
            .ToListAsync();
    }
}