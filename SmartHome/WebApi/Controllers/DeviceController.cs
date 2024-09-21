using BusinessLogic.IServices;
using Domain;
using Domain.Exceptions.GeneralExceptions;
using Microsoft.AspNetCore.Mvc;
using WebApi.In;
using WebApi.Out;

namespace WebApi.Controllers;

[Route("api/v1/devices")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private ISessionService _sessionService;

    public DeviceController(IDeviceService deviceService, ISessionService sessionService)
    {
        _deviceService = deviceService;
        _sessionService = sessionService;
    }
    
    [HttpGet]
    public IActionResult GetDevices([FromHeader] Guid? authorization, [FromQuery] string? name, 
        [FromQuery] string? model, [FromQuery] string? company, [FromQuery] string? type)
    {
        if (AuthorizationIsInvalid(authorization))
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
            devices = devices.FindAll(device => device.Kind == type);
        }

        DevicesResponse devicesResponse = GetDevicesResponse(devices);
        
        
        return Ok(devicesResponse);
    }
    
    [HttpGet]
    [Route("types")]
    public IActionResult GetDeviceTypes()
    {
        List<string> deviceTypes = _deviceService.GetDeviceTypes();
        
        DeviceTypesResponse deviceTypesResponse = GetDeviceTypesResponse(deviceTypes);
        
        return Ok(deviceTypesResponse);
    }
    
    [HttpPost]
    [Route("window-sensors")]
    public IActionResult PostWindowSensors([FromBody] WindowSensorRequest request)
    {
        WindowSensor windowSensor = ParseWindowSensorRequest(request);
        
        _deviceService.CreateDevice(windowSensor);

        return Created("", "");
    }
    
    private WindowSensor ParseWindowSensorRequest(WindowSensorRequest request)
    {
        WindowSensor windowSensor = new WindowSensor()
        {
            Name = request.Name,
            Model = request.Model,
            PhotoURLs = request.PhotoUrls,
            Description = request.Description
        };
        
        return windowSensor;
    }
    
    private DevicesResponse GetDevicesResponse(List<Device> devices)
    {
        List<DeviceResponse> deviceResponses = new List<DeviceResponse>();

        foreach (Device device in devices)
        {
            DeviceResponse response = GetDeviceResponse(device);
            if (response != null)
            {
                deviceResponses.Add(response);
            }
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
            PhotoUrl = device.PhotoURLs?.FirstOrDefault(),
            CompanyName = device.Company?.Name
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
    
    private bool AuthorizationIsInvalid(Guid? authorization)
    {
        return authorization == null || !UserIsAuthenticated(authorization.Value);
    } 
    
    private bool UserIsAuthenticated(Guid authorization)
    {
        try 
        {
            _sessionService.GetUser(authorization);
            return true;
        }
        catch (CannotFindItemInList)
        {
            return false;
        }
    }
}