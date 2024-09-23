using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.Out;

namespace TestWebApi.Controllers;

[TestClass]

public class HomesControllerTest
{
    private HomeController _homeController;
    private Mock<IHomeService> _mockHomeService;
    
    private const string Street = "Calle del Sol";
    private const int DoorNumber = 23;
    private const double Latitude = 34.0207;
    private const double Longitude = -118.4912;

    [TestInitialize]
    public void TestInitialize()
    {
        SetupHomeController();
    }

    [TestMethod]
    public void TestGetAllHomesOkStatusCode()
    {
        List<Home> homes = new List<Home>();
        Home newHome = new Home(Street, DoorNumber, Latitude, Longitude);
        homes.Add(newHome);
        _mockHomeService.Setup(service => service.GetAllHomes()).Returns(homes);
        
        ObjectResult result = _homeController.GetHomes(null, null) as OkObjectResult;
        Assert.AreEqual(200, result.StatusCode);
    }
    
    [TestMethod]
    public void TestGetHomesFilterByStreet()
    {
        List<Home> homes = new List<Home>
        {
            new Home("Calle del Sol", 23, 34.0207, -118.4912),
            new Home("Avenida Siempre Viva", 742, 34.0522, -118.2437)
        };
    
        _mockHomeService.Setup(service => service.GetAllHomes()).Returns(homes);
        
        ObjectResult result = _homeController.GetHomes("Calle del Sol", null) as OkObjectResult;
        HomesResponse response = result.Value as HomesResponse;

        Assert.AreEqual(1, response.Homes.Count);
        Assert.AreEqual("Calle del Sol", response.Homes[0].Street);
    }
    
    [TestMethod]
    public void TestGetHomesFilterByDoorNumber()
    {
        List<Home> homes = new List<Home>
        {
            new Home("Calle del Sol", 23, 34.0207, -118.4912),
            new Home("Avenida Siempre Viva", 742, 34.0522, -118.2437)
        };
    
        _mockHomeService.Setup(service => service.GetAllHomes()).Returns(homes);

        ObjectResult result = _homeController.GetHomes(null, "23") as OkObjectResult;
        HomesResponse response = result.Value as HomesResponse;

        Assert.AreEqual(1, response.Homes.Count);
        Assert.AreEqual(23, response.Homes[0].DoorNumber);
    }
    
    [TestMethod]
    public void TestGetAllHomesOkResponse()
    {
        List<Home> homes = new List<Home>
        {
            new Home("Calle del Sol", 23, 34.0207, -118.4912),
            new Home("Avenida Siempre Viva", 742, 34.0522, -118.2437)
        };

        _mockHomeService.Setup(service => service.GetAllHomes()).Returns(homes);

        List<HomeResponse> expectedHomeResponses = new List<HomeResponse>
        {
            new HomeResponse
            {
                Street = "Calle del Sol",
                DoorNumber = 23,
                Latitude = 34.0207,
                Longitude = -118.4912
            },
            new HomeResponse
            {
                Street = "Avenida Siempre Viva",
                DoorNumber = 742,
                Latitude = 34.0522,
                Longitude = -118.2437
            }
        };

        HomesResponse expectedResponse = new HomesResponse()
        {
            Homes = expectedHomeResponses
        };

        ObjectResult result = _homeController.GetHomes(null, null) as OkObjectResult;
        HomesResponse response = result.Value as HomesResponse;

        Assert.AreEqual(expectedResponse, response);
    }

    private void SetupHomeController()
    {
        _mockHomeService = new Mock<IHomeService>();
        _homeController = new HomeController(_mockHomeService.Object);
    }
}