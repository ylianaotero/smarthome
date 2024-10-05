using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/v1/login")]
[AllowAnonymous]
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
            LoginResponse response = new LoginResponse(_sessionService.LogIn(request.Email, request.Password));

            return Ok(response);
        }
        catch (CannotFindItemInList ex) 
        {
            return NotFound(ex.Message);
        }
    }

}