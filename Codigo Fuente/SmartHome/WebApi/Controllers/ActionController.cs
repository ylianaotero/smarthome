using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;

namespace WebApi.Controllers;

[Route("api/v1/actions")]
[ApiController]
public class ActionController : ControllerBase
{
    private readonly IActionService _actionService;
    
    public ActionController(IActionService actionService)
    {
        this._actionService = actionService;
    }
    
    [HttpPost]
    public IActionResult PostAction([FromBody] PostActionRequest request)
    {
        string result = _actionService.PostAction(request.HomeId, request.HardwareId, request.Functionality);
        
        return Ok(result);
    }
    
    
}