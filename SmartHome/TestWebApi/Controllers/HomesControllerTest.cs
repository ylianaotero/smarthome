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
        
        ObjectResult result = _homeController.GetHomes(null, null,null,null) as OkObjectResult;
        Assert.AreEqual(200, result.StatusCode);
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

        ObjectResult result = _homeController.GetHomes(null, null, null, null) as OkObjectResult;
        HomesResponse response = result.Value as HomesResponse;

        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestGetMembersByHomeIdOkStatusCode()
    {
        int homeId = 1;
        List<Member> members = new List<Member>
        {
            new Member { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
            new Member { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" }
        };
    
        _mockHomeService.Setup(service => service.GetMembersByHomeId(homeId)).Returns(members);

        ObjectResult result = _homeController.GetMembersByHomeId(homeId) as OkObjectResult;

        Assert.AreEqual(200, result.StatusCode);
    }
    
    /*
    [TestMethod]
    public void TestGetMembersByHomeIdOkResponse()
    {
        int homeId = 1;
        DateTime createdAtJohn = DateTime.Now.AddDays(-10);
        DateTime createdAtJane = DateTime.Now.AddDays(-20);

        List<Member> members = new List<Member>
        {
            new Member
            {
                Name = "John",
                Surname = "Doe",
                Roles = new List<Role> { new RoleTest(1) } // Usamos TestRole en lugar de Moq
            },
            new Member
            {
                Name = "Jane",
                Surname = "Doe",
                Roles = new List<Role> { new RoleTest(2) } // Usamos TestRole en lugar de Moq
            }
        };

        members[0].GetType().GetProperty("CreatedAt").SetValue(members[0], createdAtJohn);
        members[1].GetType().GetProperty("CreatedAt").SetValue(members[1], createdAtJane);

        _mockHomeService.Setup(service => service.GetMembersByHomeId(homeId)).Returns(members);

        var result = _homeController.GetMembersByHomeId(homeId) as OkObjectResult;
        var response = result.Value as dynamic;

        Assert.AreEqual("John Doe", response.Members[0].FullName);
        Assert.AreEqual("Jane Doe", response.Members[1].FullName);

        Assert.AreEqual(createdAtJohn, response.Members[0].CreatedAt);
        Assert.AreEqual(createdAtJane, response.Members[1].CreatedAt);

        Assert.AreEqual(1, response.Members[0].Roles.Count);
        Assert.AreEqual(1, response.Members[1].Roles.Count);
    }*/

    private void SetupHomeController()
    {
        _mockHomeService = new Mock<IHomeService>();
        _homeController = new HomeController(_mockHomeService.Object);
    }
}