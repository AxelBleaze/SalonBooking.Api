using Microsoft.EntityFrameworkCore;
using SalonBooking.Api.Data;
using SalonBooking.Api.DTOs.Schedule;
using SalonBooking.Api.Entities;
using SalonBooking.Api.Services.Interfaces;

namespace SalonBooking.Api.Services;

public class ScheduleService : IScheduleService
{
    private readonly AppDbContext _context;

    public ScheduleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkingTimeBlockDto>> GetAllBlocksAsync()
    {
        return await _context.WorkingTimeBlocks
            .OrderBy(x => x.DayOfWeek)
            .ThenBy(x => x.BlockNumber)
            .Select(x => new WorkingTimeBlockDto
            {
                Id = x.Id,
                DayOfWeek = x.DayOfWeek,
                BlockNumber = x.BlockNumber,
                StartTime = x.StartTime.ToString("HH:mm"),
                EndTime = x.EndTime.ToString("HH:mm"),
                IsEnabled = x.IsEnabled
            })
            .ToListAsync();
    }

    public async Task<WorkingTimeBlockDto> GetBlockByIdAsync(int id)
    {
        var block = await _context.WorkingTimeBlocks
            .Where(x => x.Id == id)
            .Select(x => new WorkingTimeBlockDto
            {
                Id = x.Id,
                DayOfWeek = x.DayOfWeek,
                BlockNumber = x.BlockNumber,
                StartTime = x.StartTime.ToString("HH:mm"),
                EndTime = x.EndTime.ToString("HH:mm"),
                IsEnabled = x.IsEnabled
            })
            .FirstOrDefaultAsync()
            ?? throw new Exception("Bloque horario no encontrado.");

        return block;
    }

    public async Task<List<WorkingTimeBlockDto>> GetBlocksByDayAsync(DayOfWeek dayOfWeek)
    {
        return await _context.WorkingTimeBlocks
            .Where(x => x.DayOfWeek == dayOfWeek)
            .OrderBy(x => x.BlockNumber)
            .Select(x => new WorkingTimeBlockDto
            {
                Id = x.Id,
                DayOfWeek = x.DayOfWeek,
                BlockNumber = x.BlockNumber,
                StartTime = x.StartTime.ToString("HH:mm"),
                EndTime = x.EndTime.ToString("HH:mm"),
                IsEnabled = x.IsEnabled
            })
            .ToListAsync();
    }

    public async Task<List<WorkingTimeBlockDto>> UpdateDayBlocksAsync(UpdateDayBlocksRequest request)
    {
        if (request.Blocks.Count != 3)
            throw new Exception("Debes enviar exactamente 3 bloques.");

        var duplicated = request.Blocks
            .GroupBy(x => x.BlockNumber)
            .Any(g => g.Count() > 1);

        if (duplicated)
            throw new Exception("No se pueden repetir números de bloque.");

        var validBlockNumbers = new[] { 1, 2, 3 };
        if (request.Blocks.Any(x => !validBlockNumbers.Contains(x.BlockNumber)))
            throw new Exception("Los números de bloque válidos son 1, 2 y 3.");

        var dbBlocks = await _context.WorkingTimeBlocks
            .Where(x => x.DayOfWeek == request.DayOfWeek)
            .ToListAsync();

        foreach (var blockRequest in request.Blocks)
        {
            var startTime = TimeOnly.Parse(blockRequest.StartTime);
            var endTime = TimeOnly.Parse(blockRequest.EndTime);

            if (startTime >= endTime)
                throw new Exception($"El bloque {blockRequest.BlockNumber} tiene un rango inválido.");

            var block = dbBlocks.FirstOrDefault(x => x.BlockNumber == blockRequest.BlockNumber);

            if (block == null)
            {
                block = new WorkingTimeBlockEntity
                {
                    DayOfWeek = request.DayOfWeek,
                    BlockNumber = blockRequest.BlockNumber,
                    StartTime = startTime,
                    EndTime = endTime,
                    IsEnabled = blockRequest.IsEnabled
                };

                _context.WorkingTimeBlocks.Add(block);
            }
            else
            {
                block.StartTime = startTime;
                block.EndTime = endTime;
                block.IsEnabled = blockRequest.IsEnabled;
            }
        }

        var enabledBlocks = request.Blocks
            .Where(x => x.IsEnabled)
            .Select(x => new
            {
                x.BlockNumber,
                Start = TimeOnly.Parse(x.StartTime),
                End = TimeOnly.Parse(x.EndTime)
            })
            .OrderBy(x => x.Start)
            .ToList();

        for (int i = 0; i < enabledBlocks.Count - 1; i++)
        {
            var current = enabledBlocks[i];
            var next = enabledBlocks[i + 1];

            if (current.End > next.Start)
                throw new Exception("Los bloques horarios habilitados no pueden solaparse entre sí.");
        }

        await _context.SaveChangesAsync();

        return await GetBlocksByDayAsync(request.DayOfWeek);
    }
}
