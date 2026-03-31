using SalonBooking.Api.DTOs.Schedule;

namespace SalonBooking.Api.Services.Interfaces;

public interface IScheduleService
{
    Task<List<WorkingTimeBlockDto>> GetAllBlocksAsync();
    Task<WorkingTimeBlockDto> GetBlockByIdAsync(int id);
    Task<List<WorkingTimeBlockDto>> GetBlocksByDayAsync(DayOfWeek dayOfWeek);
    Task<List<WorkingTimeBlockDto>> UpdateDayBlocksAsync(UpdateDayBlocksRequest request);
}
