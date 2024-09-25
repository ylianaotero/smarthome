using IBusinessLogic;
using DataAccess.Exceptions;
using Domain;
using Domain.Exceptions.GeneralExceptions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Out;
using WebApi.Models.In;

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
    public IActionResult GetDevices([FromHeader] Guid? authorization, [FromQuery] DeviceRequest request)
    {
        if (AuthorizationIsInvalid(authorization))
        {
            return Unauthorized(UnauthorizedMessage);
        }

        DevicesResponse devicesResponse = new DevicesResponse(_deviceService.GetDevicesByFilter(request.ToFilter()));
        
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

        DeviceResponse deviceResponse;
        
        try
        {
            deviceResponse = new DeviceResponse(_deviceService.GetDeviceById(id));
        }
        catch (ElementNotFoundException)
        {
            return NotFound(NotFoundMessage);
        }
        
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
        
        _deviceService.CreateDevice(request.ToEntity());

        return Created(CreatedMessage, "/devices/");
    }

    [HttpPost]
    [Route("security-cameras")]
    public IActionResult PostSecurityCameras([FromHeader] Guid? authorization, [FromBody] SecurityCameraRequest? request)
    {
        if (AuthorizationIsInvalid(authorization))
        {
            return Unauthorized(UnauthorizedMessage);
        }
        
        if (!UserHasPermissions(authorization))
        {
            return Forbid(BasicAuthSchema);
        }
        
        _deviceService.CreateDevice(request.ToEntity());
   
        return Created(CreatedMessage, "/devices/");
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
}