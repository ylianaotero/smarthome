using BusinessLogic.IServices;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/v1/devices")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    
    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }
    
    [HttpGet]
    public IActionResult GetDevices()
    {
        return Ok();
    }
}