using Domain;
using Domain.Exceptions.GeneralExceptions;
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
    
    private const string SessionDoesNotExistExceptionMessage = "User not found";
    
    private const string Street = "Calle del Sol";
    private const int DoorNumber = 23;
    private const double Latitude = 34.0207;
    private const double Longitude = -118.4912;
    private const long homeOwnerId = 1;

    [TestInitialize]
    public void TestInitialize()
    {
        SetupHomeController();
    }

    [TestMethod]
    public void TestGetAllHomesOkStatusCode()
    {
        List<Home> homes = new List<Home>();
        Home newHome = new Home(homeOwnerId,Street, DoorNumber, Latitude, Longitude);
        homes.Add(newHome);
        _mockHomeService.Setup(service => service.GetHomesByFilter(It.IsAny<Func<Home, bool>>())).Returns(homes);
        
        HomeRequest request = new HomeRequest {};
        
        ObjectResult result = _homeController.GetHomes(request) as OkObjectResult;
        Assert.AreEqual(200, result.StatusCode);
    }
    
    [TestMethod]
    public void TestGetHomesFilterByStreet()
    {
        List<Home> homes = new List<Home>
        {
            new Home(1,"Calle del Sol", 23, 34.0207, -118.4912),
        };
        HomeRequest request = new HomeRequest
        {
            Street = "Calle del Sol",
            DoorNumber = "23"
        };
    
        _mockHomeService.Setup(service => service.GetHomesByFilter(request.ToFilter())).Returns(homes);
        
        ObjectResult result = _homeController.GetHomes(request) as OkObjectResult;
        HomesResponse response = result.Value as HomesResponse;

        Assert.AreEqual(1, response.Homes.Count);
        Assert.AreEqual("Calle del Sol", response.Homes[0].Street);
    }
    
    [TestMethod]
    public void TestGetHomesFilterByDoorNumber()
    {
        List<Home> homes = new List<Home>
        {
            new Home(1,"Calle del Sol", 23, 34.0207, -118.4912),
        };
        HomeRequest request = new HomeRequest
        {
            Street = null,
            DoorNumber = "23"
        };
    
        _mockHomeService.Setup(service => service.GetHomesByFilter(request.ToFilter())).Returns(homes);
        
        ObjectResult result = _homeController.GetHomes(request) as OkObjectResult;
        HomesResponse response = result.Value as HomesResponse;

        Assert.AreEqual(1, response.Homes.Count);
        Assert.AreEqual(23, response.Homes[0].DoorNumber);
    }
    
    [TestMethod]
    public void TestGetAllHomesOkResponse()
    {
        Home expectedHome1 = new Home(1,"Calle del Sol", 23, 34.0207, -118.4912);
        Home expectedHome2 = new Home(2,"Avenida Siempre Viva", 742, 34.0522, -118.2437);
        List<Home> homes = new List<Home>
        {
            expectedHome1,
            expectedHome2
        };
        HomeRequest request = new HomeRequest {};
        _mockHomeService.Setup(service => service.GetHomesByFilter(request.ToFilter())).Returns(homes);
        HomesResponse expectedResponse = new HomesResponse(homes);
        
        ObjectResult result = _homeController.GetHomes(request) as OkObjectResult;
        HomesResponse response = result.Value as HomesResponse;

        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestPostHomesReturnsCreatedStatusCode()
    {
        Guid token = Guid.NewGuid();
        CreateHomeRequest request = new CreateHomeRequest
        {
            Street = "Main St",
            DoorNumber = 123,
            Longitude = 50.1234,
            Latitude = -30.5678
        };

        _mockSessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(DefaultUser());
        _mockHomeService.Setup(service => service.CreateHome(It.IsAny<Home>())).Callback<Home>(home => home.Id = 1);
        ObjectResult result = _homeController.PostHomes(token, request) as CreatedResult;

        _mockHomeService.Verify(service => service.CreateHome(It.IsAny<Home>()), Times.Once);
        Assert.IsNotNull(result);
        Assert.AreEqual(201, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestPostHomesUnauthorizedStatusCode()
    {
        
        CreateHomeRequest request = new CreateHomeRequest
        {
            Street = "Main St",
            DoorNumber = 123,
            Longitude = 50.1234,
            Latitude = -30.5678
        };
        ObjectResult? result = _homeController.PostHomes(null, request) as UnauthorizedObjectResult;
        Assert.AreEqual(401,result.StatusCode);
    }

    private User DefaultUser()
    {
        return new User();
    }
    
    private void SetupHomeController()
    {
        _mockSessionService = new Mock<ISessionService>();
        _mockHomeService = new Mock<IHomeService>();
        _homeController = new HomeController(_mockHomeService.Object, _mockSessionService.Object);
    }
}