using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;

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

}