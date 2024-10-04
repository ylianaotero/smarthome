using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;

namespace WebApi.Controllers;


[Route("api/v1/notifications")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private const string ResourceNotFoundMessage = "The requested resource was not found.";
    
    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    [HttpGet]
    public IActionResult GetNotifications(NotificationsRequest request)
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

}