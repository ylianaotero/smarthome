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
    private const string UserIsNotHomeOwnerMessage = "Member is not a home owner";
    private const string SourceAlreadyExistsMessage = "Source Already Exists";
    
    private const int PreconditionFailedStatusCode = 412;

    [HttpGet]
    [RolesWithPermissions(RoleWithPermissionToGetAllHomes)]
    public IActionResult GetHomes([FromQuery] HomeRequest request)
    {
        HomesResponse homesResponse = new HomesResponse(homeService.GetHomesByFilter(request.ToFilter()));
        
        return Ok(homesResponse);
    }
    
    [HttpPatch]
    [Route("{id}/members")]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult ChangeNotificationPermission([FromRoute] long id, [FromBody] ChangePermissionsRequest request)
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
    public IActionResult PostHomes([FromBody] CreateHomeRequest request)
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
            if (e.Message == UserIsNotHomeOwnerMessage)
            {
                return StatusCode(StatusCodes.Status412PreconditionFailed, UserIsNotHomeOwnerMessage);
            }
        }
        
        HomeResponse homeResponse = new HomeResponse(request.ToEntity());
        
        return CreatedAtAction(nameof(PostHomes), homeResponse);
    }
    
    [HttpGet]
    [Route("{id}/members")]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult GetMembersFromHome([FromRoute] long id)
    {
        try
        {
            return Ok(new MembersResponse(homeService.GetMembersFromHome(id)));
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
        HomeResponse homeResponse;
        
        try
        {
            homeResponse = new HomeResponse(homeService.GetHomeById(id));
        }
        catch (ElementNotFound)
        {
            return NotFound(ResourceNotFoundMessage);
        }
        
        return Ok(homeResponse);
    }

    [HttpPut]
    [Route("{id}/devices")]
    [RestrictToPrivilegedMembers(false, true)]
    public IActionResult PutDevicesInHome([FromRoute] long id, [FromBody] PutHomeDevicesRequest request)
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
    
    [HttpPut]
    [Route("{id}/members")]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult AddMemberToHome([FromRoute] long id, [FromBody] MemberRequest request)
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
    
    [HttpPatch]
    [Route("{id}/devices")]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult UpdateDeviceConnectionStatus([FromRoute] long id, 
        [FromBody] UpdateDeviceConnectionStatusRequest request)
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
    public IActionResult GetDevicesFromHome([FromRoute] int id)
    {
        DevicesUnitResponse devicesUnitResponse;
        try
        {
            devicesUnitResponse = new DevicesUnitResponse(homeService.GetDevicesFromHome(id));
            return Ok(devicesUnitResponse);
        }
        catch (ElementNotFound)
        {
            return NotFound(ResourceNotFoundMessage);
        }
    }
}