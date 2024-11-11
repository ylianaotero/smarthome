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
}