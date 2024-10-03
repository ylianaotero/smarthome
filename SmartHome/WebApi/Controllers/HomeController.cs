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
    private readonly ISessionService _sessionService;
    
    private const string RoleWithPermissionToUpdateHome = "HomeOwner";
    private const string RoleWithPermissionToGetAllHomes = "Administrator";
    private const string UpdatedHomeMessage = "The home was updated successfully.";
    private const string ResourceNotFoundMessage = "The requested resource was not found.";

    
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

    [HttpPost]
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult PostHomes([FromBody] CreateHomeRequest request)
    {
        _homeService.CreateHome(request.ToEntity());
        
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
            var members = _homeService.GetMembersFromHome(id);
            return Ok(new MembersResponse(members));
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
    [RolesWithPermissions(RoleWithPermissionToUpdateHome)]
    public IActionResult PutDevicesInHome([FromRoute] long id, [FromBody] HomeDevicesRequest request)
    {
        try
        {
            _homeService.PutDevicesInHome(id, request.ToEntity());
            return Ok(UpdatedHomeMessage);
        }
        catch (ElementNotFound)
        {
            return BadRequest();
        }
    }
}