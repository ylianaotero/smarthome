using BusinessLogic.IServices;
using Domain.Exceptions.GeneralExceptions;
using Microsoft.AspNetCore.Mvc;
using WebApi.In;
using WebApi.Out;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/v1/login")]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    [HttpPost]
    public IActionResult LogIn([FromBody] LoginRequest request)
    {
        try
        {
            var session = _sessionService.LogIn(request.Email, request.Password);
                
            var response = new LoginResponse
            {
                Token = session.Id 
            };

            return Ok(response);
        }
        catch (CannotFindItemInList ex) 
        {
            return StatusCode(404, "Not found");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error.");
        }
    }

}