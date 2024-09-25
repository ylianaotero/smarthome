using Domain;
using Domain.Exceptions.GeneralExceptions;
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
    
    private const string UnauthorizedMessage = "Unauthorized access. Please provide valid credentials.";
    private const string CreatedMessage = "The resource was created successfully.";

    
    public HomeController(IHomeService homeService, ISessionService sessionService)
    {
        _homeService = homeService;
        _sessionService = sessionService;
    }
    
    [HttpGet]
    public IActionResult GetHomes( [FromQuery] string? street,  [FromQuery] string? doorNumber)
    {
        
        List<Home> homes = _homeService.GetAllHomes();
        if (street != null)
        {
            homes = homes.FindAll(h => h.Street == street);
        }

        if (doorNumber != null)
        {
            homes = homes.FindAll(h => h.DoorNumber.ToString() == doorNumber);
        }
        
        HomesResponse homesResponse = GetHomesResponse(homes);
        return Ok(homesResponse);
    }

    [HttpPost]
    public IActionResult PostHomes([FromHeader] Guid? authorization, [FromBody] CreateHomeRequest request)
    {
        if (AuthorizationIsInvalid(authorization))
        {
            return Unauthorized(UnauthorizedMessage);
        }
        
        Home home = ParseHomeRequest(request);
        _homeService.CreateHome(home);

        long homeId = home.Id;
        return Created("/homes/" + homeId,CreatedMessage);
    }

    private Home ParseHomeRequest(CreateHomeRequest request)
    {
        Home home = new Home()
        {
            Street = request.Street,
            DoorNumber = request.DoorNumber,
            Longitude = request.Longitude,
            Latitude = request.Latitude,
        };
        return home;
    }

    private HomesResponse GetHomesResponse(List<Home> homes)
    {
        List<HomeResponse> homeResponses = new List<HomeResponse>();

        foreach (Home home in homes)
        {
            homeResponses.Add(GetHomeResponse(home));
        }

        HomesResponse homesResponse = new HomesResponse()
        {
            Homes = homeResponses
        };
        return homesResponse;
    }

    private HomeResponse GetHomeResponse(Home home)
    {
        HomeResponse homeResponse = new HomeResponse()
        {
            Street = home.Street,
            DoorNumber = home.DoorNumber,
            Latitude = home.Latitude,
            Longitude = home.Longitude
        };
        return homeResponse;
    }
    
    private bool AuthorizationIsInvalid(Guid? authorization)
    {
        return authorization == null || !UserIsAuthenticated(authorization.Value);
    } 
    
    private bool UserIsAuthenticated(Guid authorization)
    {
        try 
        {
            _sessionService.GetUser(authorization);
            return true;
        }
        catch (CannotFindItemInList)
        {
            return false;
        }
    }
}