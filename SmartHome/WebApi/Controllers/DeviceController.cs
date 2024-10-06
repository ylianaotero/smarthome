using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/devices")]
[ApiController]
public class DeviceController(IDeviceService deviceService, ICompanyService companyService)
    : ControllerBase
{
    private const string RoleWithPermissions = "CompanyOwner";
    private const string NotFoundMessage = "The requested resource was not found.";
    private const string CompanyNotFoundMessage = "The company was not found.";

    [HttpGet] 
    public IActionResult GetDevices([FromQuery] DeviceRequest request, [FromQuery] PageDataRequest pageDataRequest)
    {
        DevicesResponse devicesResponse = new DevicesResponse
            (deviceService.GetDevicesByFilter(request.ToFilter(), pageDataRequest.ToPageData()));
        
        return Ok(devicesResponse);
    }
    
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetDeviceById([FromRoute] long id)
    {
        DeviceResponse deviceResponse;
        
        try
        {
            deviceResponse = new DeviceResponse(deviceService.GetDeviceById(id));
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
        List<string> deviceTypes = deviceService.GetDeviceTypes();
        
        DeviceTypesResponse deviceTypesResponse = GetDeviceTypesResponse(deviceTypes);
        
        return Ok(deviceTypesResponse);
    }
    
    [HttpPost]
    [Route("window-sensors")]
    [RolesWithPermissions(RoleWithPermissions)]
    [AllowAnonymous]
    public IActionResult PostWindowSensors([FromBody] WindowSensorRequest request)
    {
        try
        {
            deviceService.CreateDevice(companyService.AddCompanyToDevice(request.Company, request.ToEntity()));
        } 
        catch (ElementNotFound)
        {
            return NotFound(CompanyNotFoundMessage);
        } 
        
        return CreatedAtAction(nameof(PostWindowSensors), request);
    }

    [HttpPost]
    [Route("security-cameras")]
    [RolesWithPermissions(RoleWithPermissions)]
    [AllowAnonymous]
    public IActionResult PostSecurityCameras([FromBody] SecurityCameraRequest request)
    {
        try
        {
            deviceService.CreateDevice(companyService.AddCompanyToDevice(request.Company, request.ToEntity()));
        }
        catch (ElementNotFound)
        {
            return NotFound(CompanyNotFoundMessage);
        }
        
        return CreatedAtAction(nameof(PostSecurityCameras), request);
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