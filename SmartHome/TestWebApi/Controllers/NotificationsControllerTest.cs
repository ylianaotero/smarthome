using CustomExceptions;
using Domain;
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
    private const string EventName = "Event";
    private const string CannotFindItemInListMessage = "The requested resource was not found.";
    
    private NotificationsController _notificationController;
    private Mock<INotificationService> _mockINotificationService;
    private List<Notification> _listOfNotifications;
    
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
        
        var result = _notificationController.GetNotifications(request) as OkObjectResult;
        NotificationsResponse response = result.Value as NotificationsResponse;
        
        _mockINotificationService.Verify();
        
        Assert.AreEqual(notificationResponse, response);
    }

    [TestMethod]
    public void TestGetNotificationNotFound()
    {
        _mockINotificationService.Setup(x => x.GetNotificationsByFilter(It.IsAny<Func<Notification, bool>>(),null)).Throws(new CannotFindItemInList(CannotFindItemInListMessage));
        NotificationsRequest request = new NotificationsRequest();
        
        var result = _notificationController.GetNotifications(request) as NotFoundObjectResult;
        
        _mockINotificationService.Verify();
        
        Assert.AreEqual("The requested resource was not found.", result.Value);
    }
    
    private void SetupNotificationController()
    {

        _mockINotificationService = new Mock<INotificationService>();
        _notificationController = new NotificationsController(_mockINotificationService.Object);
    }
}