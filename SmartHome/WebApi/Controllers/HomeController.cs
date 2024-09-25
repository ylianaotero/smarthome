using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.In;
using WebApi.Models.Out;

namespace WebApi.Controllers;

[Route("api/v1/homes")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly IHomeService _homeService;
    private readonly ISessionService _sessionService;
    
    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    [HttpGet]
    public IActionResult GetHomes([FromQuery] HomeRequest? request)
    {
        HomesResponse homesResponse = new HomesResponse(_homeService.GetHomesByFilter(request.ToFilter()));
        
        return Ok(homesResponse);
    }
}