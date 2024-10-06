using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;

namespace WebApi.Controllers;

[Route("api/v1/notifications")]
[ApiController]
public class NotificationsController(INotificationService notificationService) : ControllerBase
{
    private const string ResourceNotFoundMessage = "The requested resource was not found.";

    [HttpGet]
    public IActionResult GetNotifications([FromQuery] NotificationsRequest request)
    {
        NotificationsResponse notificationsResponse;
        try
        {
            notificationsResponse = new NotificationsResponse
                (notificationService.GetNotificationsByFilter(request.ToFilter(), null));
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
            notificationService.SendNotifications(request.ToEntity());
        }
        catch (CannotFindItemInList)
        {
            return NotFound(ResourceNotFoundMessage);
        }

        return CreatedAtAction(nameof(CreateNotification), request);
    }
}