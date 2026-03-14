using Microsoft.AspNetCore.Mvc;
using SalonBooking.Api.Services.Interfaces;

namespace SalonBooking.Api.Controllers.Public;

[ApiController]
[Route("api/public/services")]
public class PublicServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public PublicServicesController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _serviceService.GetAllAsync();
        return Ok(result);
    }
}