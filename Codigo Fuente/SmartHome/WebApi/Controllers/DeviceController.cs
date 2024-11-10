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
    private const string ModelIsInvalidMessage = "Model is not valid" ;

    [HttpGet] 
    public IActionResult GetDevices([FromQuery] GetDeviceRequest request, [FromQuery] PageDataRequest pageDataRequest)
    {
        int count = deviceService.GetDevicesByFilter(request.ToFilter(), null).Count; 
        GetDevicesResponse getDevicesResponse = new GetDevicesResponse
            (deviceService.GetDevicesByFilter(request.ToFilter(), pageDataRequest.ToPageData()), count);
        
        return Ok(getDevicesResponse);
    }
    
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetDeviceById([FromRoute] long id)
    {
        GetDeviceResponse getDeviceResponse;
        
        try
        {
            getDeviceResponse = new GetDeviceResponse(deviceService.GetDeviceById(id));
        }
        catch (ElementNotFound)
        {
            return NotFound(NotFoundMessage);
        }
        
        return Ok(getDeviceResponse);
    }
    
    [HttpGet]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client)]
    [Route("types")]
    public IActionResult GetDeviceTypes()
    {
        List<string> deviceTypes = deviceService.GetDeviceTypes();
        
        GetDeviceTypesResponse getDeviceTypesResponse = GetDeviceTypesResponse(deviceTypes);
        
        return Ok(getDeviceTypesResponse);
    }
    
    [HttpPost]
    [Route("window-sensors")]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult PostWindowSensors([FromBody] PostWindowSensorRequest request)
    {
        try
        {
            deviceService.CreateDevice(companyService.AddCompanyToDevice(request.Company, request.ToEntity()));
        } 
        catch (ElementNotFound)
        {
            return NotFound(CompanyNotFoundMessage);
        } 
        catch (InputNotValid)
        {
            return BadRequest(ModelIsInvalidMessage);
        }
        
        return CreatedAtAction(nameof(PostWindowSensors), request);
    }

    [HttpPost]
    [Route("security-cameras")]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult PostSecurityCameras([FromBody] PostSecurityCameraRequest request)
    {
        try
        {
            deviceService.CreateDevice(companyService.AddCompanyToDevice(request.Company, request.ToEntity()));
        }
        catch (ElementNotFound)
        {
            return NotFound(CompanyNotFoundMessage);
        }
        catch (InputNotValid)
        {
            return BadRequest(ModelIsInvalidMessage);
        }
        
        return CreatedAtAction(nameof(PostSecurityCameras), request);
    }
    
    [HttpPost]
    [Route("motion-sensors")]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult PostMotionSensors([FromBody] PostMotionSensorRequest request)
    {
        try
        {
            deviceService.CreateDevice(companyService.AddCompanyToDevice(request.Company, request.ToEntity()));
        }
        catch (ElementNotFound)
        {
            return NotFound(CompanyNotFoundMessage);
        }
        catch (InputNotValid)
        {
            return BadRequest(ModelIsInvalidMessage);
        }
        
        return CreatedAtAction(nameof(PostMotionSensors), request);
    }
    
    [HttpPost]
    [Route("smart-lamps")]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult PostSmartLamps([FromBody] PostSmartLampRequest request)
    {
        try
        {
            deviceService.CreateDevice(companyService.AddCompanyToDevice(request.Company, request.ToEntity()));
        }
        catch (ElementNotFound)
        {
            return NotFound(CompanyNotFoundMessage);
        }
        
        return CreatedAtAction(nameof(PostSmartLamps), request);
    }
    
    private GetDeviceTypesResponse GetDeviceTypesResponse(List<string> deviceTypes)
    {
        GetDeviceTypesResponse getDeviceTypesResponse = new GetDeviceTypesResponse()
        {
            DeviceTypes = deviceTypes
        };
        
        return getDeviceTypesResponse;
    }
}