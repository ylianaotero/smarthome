using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Moq;
using WebApi.Controllers;

namespace TestWebApi.Controllers;

[TestClass]
public class ActionControllerTest
{
    [TestMethod]
    public void TestPostActionOkStatusCode()
    {
        Mock<IActionService> actionServiceMock = new Mock<IActionService>();
        ActionController controller = new ActionController(actionServiceMock.Object);

        PostActionRequest request = new PostActionRequest()
        {
            HomeId = 1,
            HardwareId = Guid.NewGuid(),
            Functionality = "OnOff"
        };
        
        actionServiceMock.Setup(x => x.PostAction(request.HomeId, request.HardwareId, request.Functionality)).Returns("Success");

        ObjectResult result = controller.PostAction(request) as OkObjectResult;
        
        Assert.AreEqual(200, result.StatusCode);
    }
}