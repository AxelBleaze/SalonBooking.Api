using Microsoft.AspNetCore.Mvc;
using SalonBooking.Api.DTOs.Appointments;
using SalonBooking.Api.Services.Interfaces;

namespace SalonBooking.Api.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IAvailabilityService _availabilityService;

    public AppointmentsController(
        IAppointmentService appointmentService,
        IAvailabilityService availabilityService)
    {
        _appointmentService = appointmentService;
        _availabilityService = availabilityService;
    }

    [HttpGet("availability")]
    public async Task<IActionResult> GetAvailability([FromQuery] DateOnly date, [FromQuery] int serviceId)
    {
        var result = await _availabilityService.GetAvailableSlotsAsync(date, serviceId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentRequest request)
    {
        var id = await _appointmentService.CreateAppointmentAsync(request);
        return Ok(new { appointmentId = id, message = "Reserva creada correctamente." });
    }
}