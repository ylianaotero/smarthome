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
    private const string RoleWithPermissions = "CompanyOwner";

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
    [Route("{id}")]
    public IActionResult GetDeviceById([FromHeader] Guid? authorization, [FromRoute] long id)
    {
        if (authorization == null)
        {
            return Unauthorized("");
        }
        Device device = _deviceService.GetDeviceById(id);
        
        return Ok("");
    }
    
    [HttpGet]
    [Route("types")]
    public IActionResult GetDeviceTypes([FromHeader] Guid? authorization)
    {
        if (AuthorizationIsInvalid(authorization))
        {
            return Unauthorized("");
        }
        
        List<string> deviceTypes = _deviceService.GetDeviceTypes();
        
        DeviceTypesResponse deviceTypesResponse = GetDeviceTypesResponse(deviceTypes);
        
        return Ok(deviceTypesResponse);
    }
    
    [HttpPost]
    [Route("window-sensors")]
    public IActionResult PostWindowSensors([FromHeader] Guid? authorization, [FromBody] WindowSensorRequest request)
    {
        if (AuthorizationIsInvalid(authorization))
        {
            return Unauthorized("");
        }

        if (!UserHasPermissions(authorization))
        {
            return Forbid("Basic");
        }
        
        WindowSensor windowSensor = ParseWindowSensorRequest(request);
        
        _deviceService.CreateDevice(windowSensor);

        return Created("", "");
    }
    
    [HttpPost]
    [Route("security-cameras")]
    public IActionResult PostSecurityCameras([FromHeader] Guid? authorization, [FromBody] SecurityCameraRequest request)
    {
        if (AuthorizationIsInvalid(authorization))
        {
            return Unauthorized("");
        }
        
        if (!UserHasPermissions(authorization))
        {
            return Forbid("Basic");
        }
        
        SecurityCamera securityCamera = ParseSecurityCameraRequest(request);
        
        _deviceService.CreateDevice(securityCamera);

        return Created("", "");
    }

    private SecurityCamera ParseSecurityCameraRequest(SecurityCameraRequest request)
    {
        SecurityCamera securityCamera = new SecurityCamera()
        {
            Name = request.Name,
            Model = request.Model,
            PhotoURLs = request.PhotoUrls,
            Description = request.Description,
            LocationType = request.LocationType,
            Functionalities = request.Functionalities,
            Company = request.Company
        };
        
        return securityCamera;
    }
    
    private WindowSensor ParseWindowSensorRequest(WindowSensorRequest request)
    {
        WindowSensor windowSensor = new WindowSensor()
        {
            Name = request.Name,
            Model = request.Model,
            PhotoURLs = request.PhotoUrls,
            Description = request.Description,
            Functionalities = request.Functionalities,
            Company = request.Company
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
    
    private bool UserHasPermissions(Guid? authorization)
    {
        User user = _sessionService.GetUser(authorization.Value);
        return authorization != null && user != null && user.Roles != null &&
               _sessionService.GetUser(authorization.Value).Roles.Exists(r => RoleIsAdequate(r));
    }
    
    private bool RoleIsAdequate(Role role)
    {
        return role.GetType().Name == RoleWithPermissions;
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