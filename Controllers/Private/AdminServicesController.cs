using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonBooking.Api.DTOs.Services;
using SalonBooking.Api.Services.Interfaces;

namespace SalonBooking.Api.Controllers.Private;

[ApiController]
[Authorize]
[Route("api/private/services")]
public class AdminServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public AdminServicesController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = true)
    {
        var result = await _serviceService.GetAllAsync(includeInactive);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceRequest request)
    {
        var result = await _serviceService.CreateAsync(request);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceRequest request)
    {
        var result = await _serviceService.UpdateAsync(id, request);
        return Ok(result);
    }
}