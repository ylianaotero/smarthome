using CustomExceptions;
using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.In;
using WebApi.Models.Out;

namespace TestWebApi.Controllers;

[TestClass]

public class HomesControllerTest
{
    private HomeController _homeController;
    private Mock<IHomeService> _mockHomeService;
    private Mock<ISessionService> _mockSessionService;
    private Home _defaultHome;
    
    private const string HomeNotFoundExceptionMessage = "Home not found";
    
    private const string Street = "Calle del Sol";
    private const int DoorNumber = 23;
    private const double Latitude = 34.0207;
    private const double Longitude = -118.4912;
    private const long homeOwnerId = 1;

    [TestInitialize]
    public void TestInitialize()
    {
        SetupHomeController();
        SetupDefaultObjects();
    }

    [TestMethod]
    public void TestGetAllHomesOkStatusCode()
    {
        List<Home> homes = new List<Home>
        {
            new Home(1,"Calle del Sol", 23, 34.0207, -118.4912),
            new Home(2,"Avenida Siempre Viva", 742, 34.0522, -118.2437)
        };
        _mockHomeService.Setup(service => service.GetHomesByFilter(It.IsAny<Func<Home, bool>>())).Returns(homes);

        HomeRequest request = new HomeRequest();

        ObjectResult? result = _homeController.GetHomes(request) as OkObjectResult;
        
        Assert.AreEqual(200, result!.StatusCode);
    }

    [TestMethod]
    public void TestGetAllHomesWhenThereAreNoOkStatusCode()
    {
        List<Home> homes = [];
        _mockHomeService.Setup(service => service.GetHomesByFilter(It.IsAny<Func<Home, bool>>())).Returns(homes);

        HomeRequest request = new HomeRequest();
        
        ObjectResult? result = _homeController.GetHomes(request) as OkObjectResult;
        
        Assert.AreEqual(200, result!.StatusCode);
    }

    [TestMethod]
    public void TestGetAllHomesOkResponse()
    {
        List<Home> homes = new List<Home>
        {
            new Home(1,"Calle del Sol", 23, 34.0207, -118.4912),
            new Home(2,"Avenida Siempre Viva", 742, 34.0522, -118.2437)
        };
        _mockHomeService.Setup(service => service.GetHomesByFilter(It.IsAny<Func<Home, bool>>())).Returns(homes);
        HomesResponse expectedResponse = DefaultHomesResponse();
        
        HomeRequest request = new HomeRequest();
        
        ObjectResult? result = _homeController.GetHomes(request) as OkObjectResult;
        HomesResponse response = (result!.Value as HomesResponse);
        
        Assert.AreEqual(expectedResponse,response);
    }

    [TestMethod]
    public void TestGetHomeByIdOkStatusCode()
    {
        Home home = new Home(1, "Calle del Sol", 23, 34.0207, -118.4912);
        _mockSessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockHomeService.Setup(service => service.GetHomeById(1)).Returns(home);
        
        ObjectResult? result = _homeController.GetHomeById(1) 
            as OkObjectResult;
        
        Assert.AreEqual(200, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetHomeByIdOkResponse()
    {
        
        _mockHomeService.Setup(service => service.GetHomeById(1)).Returns(_defaultHome);
        HomeResponse expectedResponse = DefaultHomeResponse();

        ObjectResult? result = _homeController.GetHomeById(1) as OkObjectResult;
        HomeResponse response = (result!.Value as HomeResponse)!;
        
        Assert.AreEqual(expectedResponse, response);
    }

    [TestMethod]
    public void TestGetHomeByIdNotFoundStatusCode()
    {
        Guid token = Guid.NewGuid();
        _mockHomeService.Setup(service => service.GetHomeById(1))
            .Throws(new ElementNotFound(HomeNotFoundExceptionMessage));
        _mockSessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
     
        IActionResult result = _homeController.GetHomeById(1);

        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    private HomeResponse DefaultHomeResponse()
    {
        return new HomeResponse(_defaultHome);
    }

    private HomesResponse DefaultHomesResponse()
    {
        List<Home> homes = new List<Home>
        {
            new Home(1,"Calle del Sol", 23, 34.0207, -118.4912),
            new Home(2,"Avenida Siempre Viva", 742, 34.0522, -118.2437)
        };
        return new HomesResponse(homes);
    }
    
    private void SetupDefaultObjects()
    {
        _defaultHome = new Home(13, "Calle de la luna", 23, 34.0297, -119.4912);
    }

    private void SetupHomeController()
    {
        _mockSessionService = new Mock<ISessionService>();
        _mockHomeService = new Mock<IHomeService>();
        _homeController = new HomeController(_mockHomeService.Object);
    }
}