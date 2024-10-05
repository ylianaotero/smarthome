using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/homes")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly IHomeService _homeService;
    
    private const string RoleWithPermissionToUpdateHome = "HomeOwner";
    private const string RoleWithPermissionToGetAllHomes = "Administrator";
    private const string UpdatedHomeMessage = "The home was updated successfully.";
    private const string ResourceNotFoundMessage = "The requested resource was not found.";
    private const string HomeOwnerNotFoundMessage = "The home owner was not found.";
    private const string UserIsNotHomeOwnerMessage = "Member is not a home owner";
    private const string SourceAlreadyExistsMessage = "Source Already Exists";
    
    private const int PreconditionFailedStatusCode = 412;
    
    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    [HttpGet]
    [RolesWithPermissions(RoleWithPermissionToGetAllHomes)]
    public IActionResult GetHomes([FromQuery] HomeRequest request)
    {
        HomesResponse homesResponse = new HomesResponse(_homeService.GetHomesByFilter(request.ToFilter()));
        
        return Ok(homesResponse);
    }
    
    [HttpPut]
    [Route("{id}/members")]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult ChangeNotificationPermission([FromRoute] long id, [FromBody] ChangePermissionsRequest request)
    {
        try
        {
            _homeService.ChangePermission(request.ToEntity(),id);
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
            _homeService.CreateHome(_homeService.AddOwnerToHome(request.OwnerId, request.ToEntity()));
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
            return Ok(new MembersResponse(_homeService.GetMembersFromHome(id)));
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
            homeResponse = new HomeResponse(_homeService.GetHomeById(id));
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
            _homeService.PutDevicesInHome(id, request.ToEntity());
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
            _homeService.AddMemberToHome(id, request.ToEntity());
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
    
}