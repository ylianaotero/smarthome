using System.Net;
using BusinessLogic.IServices;
using Domain;
using Microsoft.AspNetCore.Mvc;
using WebApi.Out;

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
        List<Device> devices = _deviceService.GetAllDevices();

        List<DeviceResponse> deviceResponses = new List<DeviceResponse>();
        
        foreach (Device device in devices)
        {
            DeviceResponse deviceResponse = new DeviceResponse()
            {
                Name = device.Name,
                Model = device.Model,
                PhotoUrl = device.PhotoURLs.First(),
                CompanyName = device.Company.Name
            };
            
            deviceResponses.Add(deviceResponse);
        }
        
        DevicesResponse devicesResponse = new DevicesResponse()
        {
            Devices = deviceResponses
        };
        
        return Ok(devicesResponse);
    }
    
    [HttpGet]
    public IActionResult GetDeviceTypes()
    {
        return Ok("");
    }
}