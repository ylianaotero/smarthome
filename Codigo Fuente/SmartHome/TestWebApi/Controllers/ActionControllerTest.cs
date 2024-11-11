using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Moq;
using WebApi.Controllers;

namespace TestWebApi.Controllers;

[TestClass]
public class ActionControllerTest
{
    private const int OkCode = 200;
    private const int NotFoundCode = 404;
    
    private Mock<IActionService> _actionServiceMock;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _actionServiceMock = new Mock<IActionService>();
    }
    
    [TestMethod]
    public void TestPostActionOkStatusCode()
    {
        ActionController controller = new ActionController(_actionServiceMock.Object);

        PostActionRequest request = new PostActionRequest()
        {
            HomeId = 1,
            HardwareId = Guid.NewGuid(),
            Functionality = "OnOff"
        };
        
        _actionServiceMock
            .Setup(x => x.PostAction(request.HomeId, request.HardwareId, request.Functionality));

        ObjectResult result = controller.PostAction(request) as OkObjectResult;
        
        Assert.AreEqual(OkCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestPostActionNotFoundStatusCode()
    {
        ActionController controller = new ActionController(_actionServiceMock.Object);

        PostActionRequest request = new PostActionRequest()
        {
            HomeId = 1,
            HardwareId = Guid.NewGuid(),
            Functionality = "OnOff"
        };

        _actionServiceMock
            .Setup(x => x.PostAction(request.HomeId, request.HardwareId, request.Functionality))
            .Throws(new ElementNotFound("Device not found"));

        ObjectResult result = controller.PostAction(request) as NotFoundObjectResult;
        
        Assert.AreEqual(NotFoundCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestPostActionBadRequestStatusCode()
    {
        ActionController controller = new ActionController(_actionServiceMock.Object);

        PostActionRequest request = new PostActionRequest()
        {
            HomeId = 1,
            HardwareId = Guid.NewGuid(),
            Functionality = "OnOff"
        };

        _actionServiceMock
            .Setup(x => x.PostAction(request.HomeId, request.HardwareId, request.Functionality))
            .Throws(new InputNotValid("Functionality not supported for this device"));

        ObjectResult result = controller.PostAction(request) as NotFoundObjectResult;
        
        Assert.AreEqual(400, result.StatusCode);
    }
}