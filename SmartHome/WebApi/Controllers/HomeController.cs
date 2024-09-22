using BusinessLogic.IServices;
using Domain;
using Microsoft.AspNetCore.Mvc;
using WebApi.Out;

namespace WebApi.Controllers;

[Route("api/v1/homes")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly IHomeService _homeService;
    
    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    [HttpGet]
    public IActionResult GetHomes([FromQuery] string? street,  [FromQuery] string? doorNumber, [FromQuery] string? latitude, [FromQuery] string? longitude)
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
        if (latitude != null)
        {
            homes = homes.FindAll(h => h.Latitude.ToString() == latitude);
        }

        if (longitude != null)
        {
            homes = homes.FindAll(h => h.Longitude.ToString() == longitude);
        }
        
        HomesResponse homesResponse = GetHomesResponse(homes);
        return Ok(homesResponse);
    }
    
    [HttpGet("{id}/members")]
    public IActionResult GetMembersByHomeId(int id)
    {
        try
        {
            List<Member> members = _homeService.GetMembersByHomeId(id);
        
            List<UserResponse> memberResponses = members.Select(m => new UserResponse(m)).ToList();

            return Ok(new { Members = memberResponses });
        }
        catch (Exception ex)
        {
            return NotFound(new { Message = ex.Message });
        }
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