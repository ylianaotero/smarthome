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
    public IActionResult GetNotifications([FromQuery] GetNotificationsRequest request)
    {
        GetNotificationsResponse getNotificationsResponse;
        try
        {
            getNotificationsResponse = new GetNotificationsResponse
                (notificationService.GetNotificationsByFilter(request.ToFilter(), null));
        }
        catch (CannotFindItemInList)
        {
            return NotFound(ResourceNotFoundMessage);
        }

        return Ok(getNotificationsResponse);
    }
    
    [HttpPost]
    public IActionResult CreateNotification(PostNotificationRequest request)
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