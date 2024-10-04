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

public class HomesControllerTest
{
    private HomeController _homeController;
    private Mock<IHomeService> _mockHomeService;
    private Mock<ISessionService> _mockSessionService;
    private Home _defaultHome;
    private Member _member1;
    private Member _member2; 
    
    private const string HomeNotFoundExceptionMessage = "Home not found";
    private const string ElementNotFoundMessage = "Element not found";
    
    private const string HomeOwnerName = "John";
    private const string HomeOwnerName2 = "Jane";
    private const string HomeOwnerEmail = "john@example.com";
    private const string HomeOwnerEmail2 = "jane@example.com";
    private const string Street = "Calle del Sol";
    private const string Street2 = "Avenida Siempre Viva";
    private const int DoorNumber = 23;
    private const int DoorNumber2 = 742;
    private const double Latitude = 34.0207;
    private const double Latitude2 = 34.0522;
    private const double Longitude = -118.4912;
    private const double Longitude2 = -118.2437;
    private const long HomeOwnerId = 1;
    private const long HomeOwnerId2 = 2;
    private const int OKStatusCode = 200;
    private const int CreatedStatusCode = 201;
    
    private WindowSensor _defaultWindowSensor;
    private Company _defaultCompany;
    
    private const string WindowSensorName = "My Window Sensor";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const long DeviceModel = 1345354616346;

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
            new Home()
            {
                OwnerId = HomeOwnerId,
                Street = Street,
                DoorNumber = DoorNumber,
                Latitude = Latitude,
                Longitude = Longitude
            },
            new Home()
            {
                OwnerId = HomeOwnerId2,
                Street = Street2,
                DoorNumber = DoorNumber2,
                Latitude = Latitude2,
                Longitude = Longitude2
            }
        };
        _mockHomeService
            .Setup(service => service.GetHomesByFilter(It.IsAny<Func<Home, bool>>()))
            .Returns(homes);

        HomeRequest request = new HomeRequest();

        ObjectResult? result = _homeController.GetHomes(request) as OkObjectResult;
        
        Assert.AreEqual(OKStatusCode, result!.StatusCode);
    }

    [TestMethod]
    public void TestGetAllHomesWhenThereAreNoOkStatusCode()
    {
        List<Home> homes = [];
        _mockHomeService
            .Setup(service => service.GetHomesByFilter(It.IsAny<Func<Home, bool>>()))
            .Returns(homes);

        HomeRequest request = new HomeRequest();
        
        ObjectResult? result = _homeController.GetHomes(request) as OkObjectResult;
        
        Assert.AreEqual(OKStatusCode, result!.StatusCode);
    }

    [TestMethod]
    public void TestGetAllHomesOkResponse()
    {
        List<Home> homes = new List<Home>
        {
            new Home()
            {
                OwnerId = HomeOwnerId,
                Street = Street,
                DoorNumber = DoorNumber,
                Latitude = Latitude,
                Longitude = Longitude
            },
            new Home()
            {
                OwnerId = HomeOwnerId2,
                Street = Street2,
                DoorNumber = DoorNumber2,
                Latitude = Latitude2,
                Longitude = Longitude2
            }
        };
        _mockHomeService
            .Setup(service => service.GetHomesByFilter(It.IsAny<Func<Home, bool>>()))
            .Returns(homes);
        HomesResponse expectedResponse = DefaultHomesResponse();
        
        HomeRequest request = new HomeRequest();
        
        ObjectResult? result = _homeController.GetHomes(request) as OkObjectResult;
        HomesResponse response = (result!.Value as HomesResponse);
        
        Assert.AreEqual(expectedResponse,response);
    }

    [TestMethod]
    public void TestGetHomeByIdOkStatusCode()
    {
        Home home = new Home()
        {
            OwnerId = HomeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude
        };
        _mockSessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockHomeService.Setup(service => service.GetHomeById(1)).Returns(home);
        
        ObjectResult? result = _homeController.GetHomeById(1) 
            as OkObjectResult;
        
        Assert.AreEqual(OKStatusCode, result!.StatusCode);
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
    
    [TestMethod]
    public void TestPostHomeOkStatusCode()
    {
        CreateHomeRequest request = new CreateHomeRequest()
        {
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
            MaximumMembers = 5
        };
        _mockHomeService.Setup(service => service.CreateHome(It.IsAny<Home>()));
        
        ObjectResult? result = _homeController.PostHomes(request) as CreatedAtActionResult;
        
        Assert.AreEqual(CreatedStatusCode, result!.StatusCode);
    }

    [TestMethod]
    public void TestGetMembersFromHomeOKResponse()
    {
        List<Member> members = new List<Member>
        {
            _member1, _member2
        };
    
        _mockHomeService.Setup(service => service.GetMembersFromHome(It.IsAny<long>())).Returns(members);
        MembersResponse expectedResponse = new MembersResponse(members);

        ObjectResult? result = _homeController.GetMembersFromHome(HomeOwnerId) as OkObjectResult;
    
        Assert.IsNotNull(result);
        MembersResponse actualResponse = result.Value as MembersResponse;
    
        Assert.AreEqual(expectedResponse, actualResponse);
    }
    
    [TestMethod]
    public void TestGetMembersFromHomeNotFoundStatusCode()
    {
        _mockHomeService.Setup(service => service.GetMembersFromHome(It.IsAny<long>()))
            .Throws(new ElementNotFound(ElementNotFoundMessage));
    
        IActionResult result = _homeController.GetMembersFromHome(1);
    
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }
    
    [TestMethod]
    public void TestPutDevicesInHomeOkStatusCode()
    {
        DeviceUnitRequest deviceUnitRequest = new DeviceUnitRequest()
        {
            DeviceId = _defaultWindowSensor.Id,
            IsConnected = true
        };
        
        PutHomeDevicesRequest request = new PutHomeDevicesRequest()
        {
            DeviceUnits = new List<DeviceUnitRequest> {deviceUnitRequest}
        };
        
        _mockHomeService.Setup(service => service.GetHomeById(It.IsAny<long>())).Returns(_defaultHome);
        _mockHomeService
            .Setup(service => service
                .PutDevicesInHome(It.IsAny<long>(), It.IsAny<List<DeviceUnitDTO>>()));
    
        ObjectResult? result = _homeController.PutDevicesInHome(_defaultHome.Id,request) as OkObjectResult;
    
        Assert.AreEqual(OKStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestPutDevicesInHomeOkResponse()
    {
        PutHomeDevicesRequest request = new PutHomeDevicesRequest()
        {
            DeviceUnits = new List<DeviceUnitRequest>
            {
                new DeviceUnitRequest()
                {
                    DeviceId = _defaultWindowSensor.Id,
                    IsConnected = true
                }
            }
        };
        _mockHomeService.Setup(service => service.GetHomeById(It.IsAny<long>())).Returns(_defaultHome);
        _mockHomeService
            .Setup(service => service
                .PutDevicesInHome(It.IsAny<long>(), It.IsAny<List<DeviceUnitDTO>>()));
    
        ObjectResult? result = _homeController.PutDevicesInHome(_defaultHome.Id,request) as OkObjectResult;
    
        Assert.IsNotNull(result);
    }
    
    
    [TestMethod]
    public void TestPutDevicesInHomeNotFoundWhenInputIsInvalid()
    {
        PutHomeDevicesRequest request = new PutHomeDevicesRequest() {};
        _mockHomeService.Setup(service => service.GetHomeById(It.IsAny<long>())).Returns(_defaultHome);
        _mockHomeService
            .Setup(service => service
                .PutDevicesInHome(It.IsAny<long>(), It.IsAny<List<DeviceUnitDTO>>()))
            .Throws(new ElementNotFound(ElementNotFoundMessage));
    
        IActionResult result = _homeController.PutDevicesInHome(-1,request);
    
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
            new Home()
            {
                OwnerId = HomeOwnerId,
                Street = Street,
                DoorNumber = DoorNumber,
                Latitude = Latitude,
                Longitude = Longitude
            },
            new Home()
            {
                OwnerId = HomeOwnerId2,
                Street = Street2,
                DoorNumber = DoorNumber2,
                Latitude = Latitude2,
                Longitude = Longitude2
            }
        };
        return new HomesResponse(homes);
    }
    
    private void SetupDefaultObjects()
    {
        User user1 = new User { Id = HomeOwnerId, Name = HomeOwnerName, Email = HomeOwnerEmail };
        User user2 = new User { Id = HomeOwnerId2, Name = HomeOwnerName2, Email = HomeOwnerEmail2 };

        _member1 = new Member(user1);
        _member2 = new Member(user2); 
        
        _defaultHome = new Home()
        {
            Id = 1,
            OwnerId = HomeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude
        };
        
        _defaultWindowSensor = new WindowSensor()
        {
            Id = 1,
            Name = WindowSensorName,
            PhotoURLs = new List<string> {DevicePhotoUrl},
            Model = DeviceModel,
            Company = _defaultCompany
        };
    }

    private void SetupHomeController()
    {
        _mockSessionService = new Mock<ISessionService>();
        _mockHomeService = new Mock<IHomeService>();
        _homeController = new HomeController(_mockHomeService.Object);
    }
}