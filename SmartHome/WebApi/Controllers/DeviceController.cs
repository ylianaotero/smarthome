using IBusinessLogic;
using DataAccess.Exceptions;
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
    private readonly ISessionService _sessionService;
    
    private const string RoleWithPermissions = "CompanyOwner";
    private const string UnauthorizedMessage = "Unauthorized access. Please provide valid credentials.";
    private const string BasicAuthSchema = "Basic";
    private const string NotFoundMessage = "The requested resource was not found.";
    private const string CreatedMessage = "The resource was created successfully.";

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
            return Unauthorized(UnauthorizedMessage);
        }
        
        List<Device> devices = _deviceService.GetAllDevices();

        devices = FilterDevices(devices, name, model, company, type);

        DevicesResponse devicesResponse = GetDevicesResponse(devices);
        
        return Ok(devicesResponse);
    }
    
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetDeviceById([FromHeader] Guid? authorization, [FromRoute] long id)
    {
        if (AuthorizationIsInvalid(authorization))
        {
            return Unauthorized(UnauthorizedMessage);
        }

        Device device;
        
        try
        {
            device = _deviceService.GetDeviceById(id);
        }
        catch (ElementNotFoundException)
        {
            return NotFound(NotFoundMessage);
        }
        
        DeviceResponse deviceResponse = GetDeviceResponse(device);
        
        return Ok(deviceResponse);
    }
    
    [HttpGet]
    [Route("types")]
    public IActionResult GetDeviceTypes([FromHeader] Guid? authorization)
    {
        if (AuthorizationIsInvalid(authorization))
        {
            return Unauthorized(UnauthorizedMessage);
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
            return Unauthorized(UnauthorizedMessage);
        }

        if (!UserHasPermissions(authorization))
        {
            return Forbid(BasicAuthSchema);
        }
        
        WindowSensor windowSensor = ParseWindowSensorRequest(request);
        windowSensor.Company = GetUserCompany(authorization);
        
        _deviceService.CreateDevice(windowSensor);
        
        long windowSensorId = windowSensor.Id;

        return Created(CreatedMessage, "/devices/" + windowSensorId);
    }

    [HttpPost]
    [Route("security-cameras")]
    public IActionResult PostSecurityCameras([FromHeader] Guid? authorization, [FromBody] SecurityCameraRequest request)
    {
        if (AuthorizationIsInvalid(authorization))
        {
            return Unauthorized(UnauthorizedMessage);
        }
        
        if (!UserHasPermissions(authorization))
        {
            return Forbid(BasicAuthSchema);
        }
        
        SecurityCamera securityCamera = ParseSecurityCameraRequest(request);
        securityCamera.Company = GetUserCompany(authorization);
        
        _deviceService.CreateDevice(securityCamera);
        
        long securityCameraId = securityCamera.Id;

        return Created(CreatedMessage, "/devices/" + securityCameraId);
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
    
    private Company? GetUserCompany(Guid? authorization)
    {
        User user = _sessionService.GetUser(authorization!.Value);
        CompanyOwner role = (CompanyOwner) user.Roles.Find(RoleIsAdequate)!;
        
        return role!.Company;
    }
    
    private bool UserHasPermissions(Guid? authorization)
    {
        User user = _sessionService.GetUser(authorization!.Value);
        return _sessionService.GetUser(authorization.Value).Roles.Exists(r => RoleIsAdequate(r));
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
    
    private List<Device> FilterDevices(List<Device> devices, string? name, string? model, string? company, string? type)
    {
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

        return devices;
    }
}