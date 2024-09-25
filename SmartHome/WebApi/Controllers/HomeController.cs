using CustomExceptions;
using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;
using WebApi.Models.In;
using WebApi.Models.Out;

namespace WebApi.Controllers;

[Route("api/v1/homes")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly IHomeService _homeService;
    private readonly ISessionService _sessionService;
    
    private const string RoleWithPermissions = "CompanyOwner";
    
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
            return NotFound("The requested resource was not found.");
        }
        
        return Ok(homeResponse);
    }
}