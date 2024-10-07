using CustomExceptions;
using Domain.Concrete;
using Domain.DTO;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using Moq;
using WebApi.Controllers;

namespace TestWebApi.Controllers;

[TestClass]

public class NotificationControllerTest
{
    private const string CannotFindItemInListMessage = "The requested resource was not found.";
    private NotificationController _notificationController;
    private Mock<INotificationService> _mockINotificationService;
    private List<Notification> _listOfNotifications;
    private const string? Kind = "alert";
    private const bool Read = true;
    private const string EventName = "Event";
    private const int CreatedStatusCode = 201;
    private const int NotFoundStatusCode = 404;
    private const int id = 1;
    
    private const string Event = "Door Opened";
    
    [TestInitialize]
    public void TestInitialize()
    {
        SetupNotificationController();
        _listOfNotifications = new List<Notification>();

        WindowSensor windowSensor = new WindowSensor();
        
        DeviceUnit deviceUnit = new DeviceUnit()
        {
            Device = windowSensor
        };

        Notification notification = new Notification(Event)
        {
            DeviceUnit = deviceUnit
        };

        GetNotificationResponse getNotificationResponse = new GetNotificationResponse(notification);
    }

    [TestMethod]
    public void TestGetNotification()
    {
        _mockINotificationService.Setup(x => x.GetNotificationsByFilter(It.IsAny<Func<Notification, bool>>(),null)).Returns(_listOfNotifications);
        GetNotificationsRequest request = new GetNotificationsRequest()
        {
            HomeId = id,
            Kind = Kind,
            Read = Read,
            UserId = id
        };
        GetNotificationsResponse getNotificationResponse = new GetNotificationsResponse(_listOfNotifications);
        
        ObjectResult result = _notificationController.GetNotifications(request) as OkObjectResult;
        GetNotificationsResponse response = result.Value as GetNotificationsResponse;
        
        _mockINotificationService.Verify();
        
        Assert.AreEqual(getNotificationResponse, response);
    }

    [TestMethod]
    public void TestGetNotificationNotFound()
    {
        _mockINotificationService.Setup(x => x.GetNotificationsByFilter(It.IsAny<Func<Notification, bool>>(),null)).Throws(new CannotFindItemInList(CannotFindItemInListMessage));
        GetNotificationsRequest request = new GetNotificationsRequest();
        
        ObjectResult result = _notificationController.GetNotifications(request) as NotFoundObjectResult;
        
        _mockINotificationService.Verify();
        
        Assert.AreEqual(CannotFindItemInListMessage, result.Value);
    }
    
    [TestMethod]
    public void TestCreateNotification()
    {
        PostNotificationRequest request = new PostNotificationRequest()
        {
            Event = EventName,
            HardwareId = id,
            HomeId = id,
        };
        NotificationDTO notification = new NotificationDTO();
        _mockINotificationService.Setup(x => x.SendNotifications(notification));
        
        CreatedAtActionResult result = _notificationController.CreateNotification(request) as CreatedAtActionResult;
        
        _mockINotificationService.Verify();
        
        Assert.AreEqual(nameof(NotificationController.CreateNotification), result.ActionName);
    }
    
    [TestMethod]
    public void TestCreateNotificationStatusCode()
    {
        PostNotificationRequest request = new PostNotificationRequest();
        NotificationDTO notification = new NotificationDTO();
        _mockINotificationService.Setup(x => x.SendNotifications(notification));
        
        ObjectResult result = _notificationController.CreateNotification(request) as CreatedAtActionResult;
        
        Assert.AreEqual(CreatedStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestCreateNotificationNotFoundReponse()
    {
        PostNotificationRequest request = new PostNotificationRequest();
        _mockINotificationService.Setup(x => x.SendNotifications(It.IsAny<NotificationDTO>())).Throws(new CannotFindItemInList(CannotFindItemInListMessage));

        ObjectResult result = _notificationController.CreateNotification(request) as NotFoundObjectResult;
        
        Assert.AreEqual(CannotFindItemInListMessage, result.Value);
    }
    
    [TestMethod]
    public void TestCreateNotificationNotFoundStatusCode()
    {
        PostNotificationRequest request = new PostNotificationRequest();
        _mockINotificationService
            .Setup(x => x.SendNotifications(It.IsAny<NotificationDTO>()))
            .Throws(new CannotFindItemInList(CannotFindItemInListMessage));

        ObjectResult result = _notificationController.CreateNotification(request) as NotFoundObjectResult;

        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
    
    private void SetupNotificationController()
    {

        _mockINotificationService = new Mock<INotificationService>();
        _notificationController = new NotificationController(_mockINotificationService.Object);
    }
}