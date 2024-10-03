using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.Out;
using Moq;
using WebApi.Controllers;

namespace TestWebApi.Controllers;

[TestClass]

public class NotificationsControllerTest
{
    private NotificationsController _notificationController;
    private Mock<INotificationService> _mockINotificationService;
    
    [TestInitialize]
    public void TestInitialize()
    {
        SetupNotificationController();
    }

    [TestMethod]
    public void TestGetNotificationByIdOkResponse()
    {
        string title = "Title";
        var notification = new Notification(title)
        {
            Id = 1
        };
        _mockINotificationService.Setup(x => x.GetNotificationById(1)).Returns(notification);
        NotificationResponse expectedResponse = DefaultNotificationResponse();
        
        ObjectResult? result = _notificationController.GetNotificationById(1) as OkObjectResult;
        NotificationResponse response = result!.Value as NotificationResponse;
        
        Assert.AreEqual(expectedResponse, response);
    }

    private NotificationResponse DefaultNotificationResponse()
    {
        Notification notification = new Notification("Title")
        {
            Id = 1
        };

        return new NotificationResponse(notification);
    }

    private void SetupNotificationController()
    {

        _mockINotificationService = new Mock<INotificationService>();
        _notificationController = new NotificationsController(_mockINotificationService.Object);
    }
}