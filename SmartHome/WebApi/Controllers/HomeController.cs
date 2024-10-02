using System.ComponentModel;
using CustomExceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;
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
    
    private const string RoleWithPermissions = "CompanyOwner";
    private const string UpdatedHomeMessage = "The home was updated successfully.";
    private const string ResourceNotFoundMessage = "The requested resource was not found.";
    
    private const string UnauthorizedMessage = "Unauthorized access. Please provide valid credentials.";
    private const string CreatedMessage = "The resource was created successfully.";

    
    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    [HttpGet]
    public IActionResult GetHomes([FromQuery] HomeRequest request)
    {
        HomesResponse homesResponse = new HomesResponse(_homeService.GetHomesByFilter(request.ToFilter()));
        
        return Ok(homesResponse);
    }

    [HttpPost]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult PostHomes([FromBody] CreateHomeRequest request)
    {
        _homeService.CreateHome(request.ToEntity());
        return Created(CreatedMessage, "/homes/");
    }
    
    [HttpGet]
    [Route("{id}/members")]
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
    [RolesWithPermissions(RoleWithPermissions)]
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