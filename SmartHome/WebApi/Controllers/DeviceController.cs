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