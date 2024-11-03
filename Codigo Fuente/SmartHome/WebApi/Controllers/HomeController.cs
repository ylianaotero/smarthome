using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/homes")]
[ApiController]
public class HomeController(IHomeService homeService) : ControllerBase
{
    private const string RoleWithPermissionToUpdateHome = "HomeOwner";
    private const string RoleWithPermissionToGetAllHomes = "Administrator";
    private const string UpdatedHomeMessage = "The home was updated successfully.";
    private const string ResourceNotFoundMessage = "The requested resource was not found.";
    private const string HomeOwnerNotFoundMessage = "The home owner was not found.";
    private const string SourceAlreadyExistsMessage = "Source Already Exists";
    private const string UpdatedDeviceNameMessage = "The device custom name was updated successfully.";
    
    private const int PreconditionFailedStatusCode = 412;

    [HttpGet]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult GetHomes([FromQuery] GetHomeRequest request)
    {
        GetHomesResponse getHomesResponse = new GetHomesResponse(homeService.GetHomesByFilter(request.ToFilter()));
        
        return Ok(getHomesResponse);
    }
    
    
    [HttpPatch]
    [Route("{id}/members")]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult ChangeNotificationPermission([FromRoute] long id, [FromBody] PatchHomeMemberRequest request)
    {
        try
        {
            homeService.UpdateMemberNotificationPermission(request.ToEntity(), id);
            return Ok(UpdatedHomeMessage);
        }
        catch (ElementNotFound)
        {
            return NotFound(ResourceNotFoundMessage);
        }
    }

    [HttpPost]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult PostHomes([FromBody] PostHomeRequest request)
    {
        try
        {
            homeService.CreateHome(homeService.AddOwnerToHome(request.OwnerId, request.ToEntity()));
        }
        catch (ElementNotFound)
        {
            return NotFound(HomeOwnerNotFoundMessage);
        }
        catch (CannotAddItem e)
        {
            return StatusCode(StatusCodes.Status412PreconditionFailed, e.Message);
        }
        
        GetHomeResponse getHomeResponse = new GetHomeResponse(request.ToEntity());
        
        return CreatedAtAction(nameof(PostHomes), getHomeResponse);
    }
    
    [HttpGet]
    [Route("{id}/members")]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult GetMembersFromHome([FromRoute] long id)
    {
        try
        {
            return Ok(new GetMembersResponse(homeService.GetMembersFromHome(id)));
        }
        catch (ElementNotFound)
        {
            return NotFound(ResourceNotFoundMessage);
        }
    }
    
    [HttpGet]
    [Route("{id}")]
    [RolesWithPermissions(RoleWithPermissionToGetAllHomes)]
    public IActionResult GetHomeById([FromRoute] long id)
    {
        GetHomeResponse getHomeResponse;
        
        try
        {
            getHomeResponse = new GetHomeResponse(homeService.GetHomeById(id));
        }
        catch (ElementNotFound)
        {
            return NotFound(ResourceNotFoundMessage);
        }
        
        return Ok(getHomeResponse);
    }

    [HttpPost]
    [Route("{id}/devices")]
    [RestrictToPrivilegedMembers(false, true)]
    public IActionResult AddDevicesToHome([FromRoute] long id, [FromBody] PostHomeDevicesRequest request)
    {
        try
        {
            homeService.AddDevicesToHome(id, request.ToEntity());
            return Ok(UpdatedHomeMessage);
        }
        catch (ElementNotFound)
        {
            return NotFound(ResourceNotFoundMessage);
        }
    }
    
    [HttpPost]
    [Route("{id}/members")]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult AddMemberToHome([FromRoute] long id, [FromBody] PostHomeMemberRequest request)
    {
        try
        {
            homeService.AddMemberToHome(id, request.ToEntity());
            return Ok(UpdatedHomeMessage);
        }
        catch (ElementNotFound)
        {
            return NotFound(ResourceNotFoundMessage);
        }
        catch (ElementAlreadyExist)
        {
            return Conflict(SourceAlreadyExistsMessage);
        }
        catch (CannotAddItem e)
        {
            return StatusCode(PreconditionFailedStatusCode, e.Message);
        }
    }
    
    [HttpPost]
    [Route("{id}/rooms")]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult AddRoomToHome([FromRoute] long id, [FromBody] PostHomeRoomRequest request)
    {
        try
        {
            homeService.AddRoomToHome(id, request.ToEntity());
        
            return Ok(UpdatedHomeMessage);
        }
        catch (ElementNotFound)
        {
            return NotFound(ResourceNotFoundMessage);
        }
    }
    
    [HttpPatch]
    [Route("{id}/devices")]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult UpdateDeviceConnectionStatus([FromRoute] long id, 
        [FromBody] PatchDeviceRequest request)
    {
        try
        {
            homeService.UpdateDeviceConnectionStatus(id, request.ToEntity());
            return Ok(UpdatedHomeMessage);
        }
        catch (ElementNotFound)
        {
            return NotFound(ResourceNotFoundMessage);
        }
    }
    
    [HttpGet]
    [Route("{id}/devices")]
    [RestrictToPrivilegedMembers(true, false)]
    public IActionResult GetDevicesFromHome([FromRoute] int id)
    {
        GetDeviceUnitsResponse getDeviceUnitsResponse;
        try
        {
            getDeviceUnitsResponse = new GetDeviceUnitsResponse(homeService.GetDevicesFromHome(id));
            return Ok(getDeviceUnitsResponse);
        }
        catch (ElementNotFound)
        {
            return NotFound(ResourceNotFoundMessage);
        }
    }
    
    [HttpPatch]
    [Route("{id}")]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult UpdateHomeAlias([FromRoute] long id, [FromBody] PatchHomeRequest request)
    {
        try
        {
            homeService.UpdateHomeAlias(id, request.Alias);
            return Ok(UpdatedHomeMessage);
        }
        catch (ElementNotFound)
        {
            return NotFound(ResourceNotFoundMessage);
        }
    }
    
    [HttpPatch]
    [Route("{homeId}/devices/{deviceId}")]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult UpdateCustomDeviceName([FromRoute] long homeId, [FromRoute] Guid deviceId, [FromBody] PatchDeviceUnitRequest request)
    {
        try
        {
            homeService.UpdateDeviceCustomName(homeId, request.ToEntity(), deviceId);
            return Ok(UpdatedDeviceNameMessage);
        }
        catch (ElementNotFound)
        {
            return NotFound(ResourceNotFoundMessage);
        }
    }

}