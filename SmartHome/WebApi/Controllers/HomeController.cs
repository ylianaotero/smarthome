using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult GetHomes([FromQuery] string? street,  [FromQuery] string? doorNumber)
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
}