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
    public IActionResult GetDevices([FromHeader] Guid? authorization, [FromQuery] string? name, [FromQuery] string? model, [FromQuery] string? company, [FromQuery] string? type)
    {
        if (authorization == null)
        {
            return Unauthorized("");
        }
        
        List<Device> devices = _deviceService.GetAllDevices();

        if (name != null)
        {
            devices = devices.FindAll(device => device.Name == name);
        }
        if (model != null)
        {
            devices = devices.FindAll(device => device.Model.ToString() == model);
        }
        if (company != null)
        {
            devices = devices.FindAll(device => device.Company.Name == company);
        }
        if (type != null)
        {
            devices = devices.FindAll(device => device.Type == type);
        }

        DevicesResponse devicesResponse = GetDevicesResponse(devices);
        
        return Ok(devicesResponse);
    }
    
    [HttpGet]
    public IActionResult GetDeviceTypes()
    {
        List<string> deviceTypes = _deviceService.GetDeviceTypes();
        
        DeviceTypesResponse deviceTypesResponse = GetDeviceTypesResponse(deviceTypes);
        
        return Ok(deviceTypesResponse);
    }
    
    private DevicesResponse GetDevicesResponse(List<Device> devices)
    {
        List<DeviceResponse> deviceResponses = new List<DeviceResponse>();
        
        foreach (Device device in devices)
        {
            deviceResponses.Add(GetDeviceResponse(device));
        }
        
        DevicesResponse devicesResponse = new DevicesResponse()
        {
            Devices = deviceResponses
        };
        
        return devicesResponse;
    }
    
    private DeviceResponse GetDeviceResponse(Device device)
    {
        DeviceResponse deviceResponse = new DeviceResponse()
        {
            Name = device.Name,
            Model = device.Model,
            PhotoUrl = device.PhotoURLs.First(),
            CompanyName = device.Company.Name
        };
        
        return deviceResponse;
    }
    
    private DeviceTypesResponse GetDeviceTypesResponse(List<string> deviceTypes)
    {
        DeviceTypesResponse deviceTypesResponse = new DeviceTypesResponse()
        {
            DeviceTypes = deviceTypes
        };
        
        return deviceTypesResponse;
    }
}