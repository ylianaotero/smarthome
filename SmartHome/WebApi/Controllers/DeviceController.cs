using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/devices")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    
    private const string RoleWithPermissions = "CompanyOwner";
    private const string NotFoundMessage = "The requested resource was not found.";
    private const string CreatedMessage = "The resource was created successfully.";

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }
    
    [HttpGet]
    public IActionResult GetDevices([FromQuery] DeviceRequest request)
    {
        DevicesResponse devicesResponse = new DevicesResponse(_deviceService.GetDevicesByFilter(request.ToFilter()));
        
        return Ok(devicesResponse);
    }
    
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetDeviceById([FromRoute] long id)
    {
        DeviceResponse deviceResponse;
        
        try
        {
            deviceResponse = new DeviceResponse(_deviceService.GetDeviceById(id));
        }
        catch (ElementNotFound)
        {
            return NotFound(NotFoundMessage);
        }
        
        return Ok(deviceResponse);
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
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult PostWindowSensors([FromBody] WindowSensorRequest request)
    {
        _deviceService.CreateDevice(request.ToEntity());

        return Created(CreatedMessage, "/devices/");
    }

    [HttpPost]
    [Route("security-cameras")]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult PostSecurityCameras([FromBody] SecurityCameraRequest? request)
    {
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
}