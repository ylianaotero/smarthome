using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
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

    private void SetupNotificationController()
    {

        _mockINotificationService = new Mock<INotificationService>();
        _notificationController = new NotificationsController(_mockINotificationService.Object);
    }
}