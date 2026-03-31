using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonBooking.Api.DTOs.Schedule;
using SalonBooking.Api.Services.Interfaces;

namespace SalonBooking.Api.Controllers.Private;

[ApiController]
[Authorize]
[Route("api/private/schedule")]
public class AdminScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public AdminScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet("blocks")]
    public async Task<IActionResult> GetAllBlocks()
    {
        var result = await _scheduleService.GetAllBlocksAsync();
        return Ok(result);
    }

    [HttpGet("blocks/id/{id:int}")]
    public async Task<IActionResult> GetBlockById(int id)
    {
        var result = await _scheduleService.GetBlockByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("blocks/{dayOfWeek:int}")]
    public async Task<IActionResult> GetBlocksByDay(int dayOfWeek)
    {
        var result = await _scheduleService.GetBlocksByDayAsync((DayOfWeek)dayOfWeek);
        return Ok(result);
    }

    [HttpPut("blocks")]
    public async Task<IActionResult> UpdateDayBlocks([FromBody] UpdateDayBlocksRequest request)
    {
        var result = await _scheduleService.UpdateDayBlocksAsync(request);
        return Ok(result);
    }
}
