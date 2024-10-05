using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;

namespace WebApi.Controllers;


[Route("api/v1/notifications")]
[ApiController]
[AllowAnonymous]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private const string ResourceNotFoundMessage = "The requested resource was not found.";
    private const string CreatedMessage = "The resource was created successfully.";
    
    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    [HttpGet]
    public IActionResult GetNotifications([FromQuery] NotificationsRequest request)
    {
        NotificationsResponse notificationsResponse;
        try
        {
            notificationsResponse = new NotificationsResponse(_notificationService.GetNotificationsByFilter(request.ToFilter(), null));
        }
        catch (CannotFindItemInList)
        {
            return NotFound(ResourceNotFoundMessage);
        }

        return Ok(notificationsResponse);
    }
    
    [HttpPost]
    public IActionResult CreateNotification(CreateNotificationRequest request)
    {
        try
        {
            _notificationService.SendNotifications(request.ToEntity());
        }
        catch (CannotFindItemInList)
        {
            return NotFound(ResourceNotFoundMessage);
        }

        return Created("/notifications",CreatedMessage);
    }

}