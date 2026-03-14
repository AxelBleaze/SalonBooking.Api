using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonBooking.Api.DTOs.Appointments;
using SalonBooking.Api.Services.Interfaces;

namespace SalonBooking.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/admin/appointments")]
public class AdminAppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AdminAppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int? status, [FromQuery] DateOnly? date)
    {
        var result = await _appointmentService.GetAppointmentsAsync(status, date);
        return Ok(result);
    }

    [HttpPost("{id:int}/accept")]
    public async Task<IActionResult> Accept(int id)
    {
        await _appointmentService.AcceptAppointmentAsync(id);
        return Ok(new { message = "Turno aceptado correctamente." });
    }

    [HttpPost("{id:int}/reject")]
    public async Task<IActionResult> Reject(int id, [FromBody] UpdateAppointmentStatusRequest request)
    {
        await _appointmentService.RejectAppointmentAsync(id, request.Reason);
        return Ok(new { message = "Turno rechazado correctamente." });
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, [FromBody] UpdateAppointmentStatusRequest request)
    {
        await _appointmentService.CancelAppointmentAsync(id, request.Reason);
        return Ok(new { message = "Turno cancelado correctamente." });
    }
}