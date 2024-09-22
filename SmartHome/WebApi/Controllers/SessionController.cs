using Domain.Exceptions.GeneralExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.In;
using WebApi.Models.Out;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/v1/login")]
public class SessionController : ControllerBase
{
    private const string ErrorMessageUnexpectedException =  "An unexpected error occurred. Please try again later.";
    private const string ErrorMessageBadRequest =  "Email and password are required.";
    
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    [HttpPost]
    public IActionResult LogIn([FromBody] LoginRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(ErrorMessageBadRequest);
        }
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
            return StatusCode(404, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ErrorMessageUnexpectedException);
        }
    }

}