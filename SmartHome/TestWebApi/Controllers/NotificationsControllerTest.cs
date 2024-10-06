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

public class NotificationsControllerTest
{
    private const string CannotFindItemInListMessage = "The requested resource was not found.";
    private const string CreatedMessage = "The resource was created successfully.";
    private NotificationsController _notificationController;
    private Mock<INotificationService> _mockINotificationService;
    private List<Notification> _listOfNotifications;
    private const string EventName = "Event";
    private const int CreatedStatusCode = 201;
    private const int NotFoundStatusCode = 404;
    
    [TestInitialize]
    public void TestInitialize()
    {
        SetupNotificationController();
        _listOfNotifications = new List<Notification>();
    }

    [TestMethod]
    public void TestGetNotification()
    {
        _mockINotificationService.Setup(x => x.GetNotificationsByFilter(It.IsAny<Func<Notification, bool>>(),null)).Returns(_listOfNotifications);
        NotificationsRequest request = new NotificationsRequest();
        NotificationsResponse notificationResponse = new NotificationsResponse(_listOfNotifications);
        
        ObjectResult result = _notificationController.GetNotifications(request) as OkObjectResult;
        NotificationsResponse response = result.Value as NotificationsResponse;
        
        _mockINotificationService.Verify();
        
        Assert.AreEqual(notificationResponse, response);
    }

    [TestMethod]
    public void TestGetNotificationNotFound()
    {
        _mockINotificationService.Setup(x => x.GetNotificationsByFilter(It.IsAny<Func<Notification, bool>>(),null)).Throws(new CannotFindItemInList(CannotFindItemInListMessage));
        NotificationsRequest request = new NotificationsRequest();
        
        ObjectResult result = _notificationController.GetNotifications(request) as NotFoundObjectResult;
        
        _mockINotificationService.Verify();
        
        Assert.AreEqual(CannotFindItemInListMessage, result.Value);
    }
    
    [TestMethod]
    public void TestCreateNotification()
    {
        CreateNotificationRequest request = new CreateNotificationRequest();
        NotificationDTO notification = new NotificationDTO();
        _mockINotificationService.Setup(x => x.SendNotifications(notification));
        
        CreatedAtActionResult result = _notificationController.CreateNotification(request) as CreatedAtActionResult;
        
        _mockINotificationService.Verify();
        
        Assert.AreEqual(nameof(NotificationsController.CreateNotification), result.ActionName);
    }
    
    [TestMethod]
    public void TestCreateNotificationStatusCode()
    {
        CreateNotificationRequest request = new CreateNotificationRequest();
        NotificationDTO notification = new NotificationDTO();
        _mockINotificationService.Setup(x => x.SendNotifications(notification));
        
        ObjectResult result = _notificationController.CreateNotification(request) as CreatedAtActionResult;
        
        Assert.AreEqual(CreatedStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestCreateNotificationNotFoundReponse()
    {
        CreateNotificationRequest request = new CreateNotificationRequest();
        _mockINotificationService.Setup(x => x.SendNotifications(It.IsAny<NotificationDTO>())).Throws(new CannotFindItemInList(CannotFindItemInListMessage));

        ObjectResult result = _notificationController.CreateNotification(request) as NotFoundObjectResult;
        
        Assert.AreEqual(CannotFindItemInListMessage, result.Value);
    }
    
    [TestMethod]
    public void TestCreateNotificationNotFoundStatusCode()
    {
        CreateNotificationRequest request = new CreateNotificationRequest();
        _mockINotificationService
            .Setup(x => x.SendNotifications(It.IsAny<NotificationDTO>()))
            .Throws(new CannotFindItemInList(CannotFindItemInListMessage));

        ObjectResult result = _notificationController.CreateNotification(request) as NotFoundObjectResult;

        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
    
    private void SetupNotificationController()
    {

        _mockINotificationService = new Mock<INotificationService>();
        _notificationController = new NotificationsController(_mockINotificationService.Object);
    }
}