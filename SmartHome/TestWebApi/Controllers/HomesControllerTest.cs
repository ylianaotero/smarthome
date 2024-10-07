using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using Domain.DTO;
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
    private User _defaultOwner;
    private Member _member1;
    private Member _member2;
    private Home _home;
    private Home _home2;
    private List<Home> _homes;
    private List<Home> _empyHomes;
    private List<Member> _members;
    private WindowSensor _defaultWindowSensor;
    private Company _defaultCompany;
    
    private const string HomeNotFoundExceptionMessage = "Home not found";
    private const string ElementNotFoundMessage = "Element not found";
    private const string ElementAlreadyExistsMessage = "Element already exists";
    private const string CannotAddItem = "Cannot Add Item";
    
    private const string Name = "John";
    private const string Name2 = "Jane";
    private const string Email = "john@example.com";
    private const string Email2 = "jane@example.com";
    private const string Street = "Calle del Sol";
    private const string Street2 = "Avenida Siempre Viva";
    
    private const int DoorNumber = 23;
    private const int DoorNumber2 = 742;
    private const double Latitude = 34.0207;
    private const double Latitude2 = 34.0522;
    private const double Longitude = -118.4912;
    private const double Longitude2 = -118.2437;
    private const int MaxHomeMembers = 5;
    private const long HomeOwnerId = 1;
    private const long HomeOwnerId2 = 2;
    
    private const int OKStatusCode = 200;
    private const int CreatedStatusCode = 201;
    private const int NotFoundStatusCode = 404;
    private const int ConflictStatusCode = 409;
    private const int PreconditionFailedStatusCode = 412;
    
    private const bool Permission = true; 
    
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
        _mockHomeService
            .Setup(service => service.GetHomesByFilter(It.IsAny<Func<Home, bool>>()))
            .Returns(_homes);

        GetHomeRequest request = new GetHomeRequest();

        ObjectResult? result = _homeController.GetHomes(request) as OkObjectResult;
        
        Assert.AreEqual(OKStatusCode, result!.StatusCode);
    }

    [TestMethod]
    public void TestGetAllHomesWhenThereAreNoOkStatusCode()
    {
        _empyHomes = [];
        _mockHomeService
            .Setup(service => service.GetHomesByFilter(It.IsAny<Func<Home, bool>>()))
            .Returns(_homes);

        GetHomeRequest request = new GetHomeRequest();
        
        ObjectResult? result = _homeController.GetHomes(request) as OkObjectResult;
        
        Assert.AreEqual(OKStatusCode, result!.StatusCode);
    }

    [TestMethod]
    public void TestGetAllHomesOkResponse()
    {
        _mockHomeService
            .Setup(service => service.GetHomesByFilter(It.IsAny<Func<Home, bool>>()))
            .Returns(_homes);
        GetHomesResponse expectedResponse = DefaultHomesResponse();
        
        GetHomeRequest request = new GetHomeRequest();
        
        ObjectResult? result = _homeController.GetHomes(request) as OkObjectResult;
        GetHomesResponse response = (result!.Value as GetHomesResponse);
        
        Assert.AreEqual(expectedResponse,response);
    }

    [TestMethod]
    public void TestGetHomeByIdOkStatusCode()
    {
        _mockSessionService.Setup(service => service.GetUser(It.IsAny<Guid>())).Returns(new User());
        _mockHomeService.Setup(service => service.GetHomeById(1)).Returns(_home);
        
        ObjectResult? result = _homeController.GetHomeById(1) 
            as OkObjectResult;
        
        Assert.AreEqual(OKStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestGetHomeByIdOkResponse()
    {
        _mockHomeService.Setup(service => service.GetHomeById(1)).Returns(_home);
        GetHomeResponse expectedResponse = DefaultHomeResponse();

        ObjectResult? result = _homeController.GetHomeById(1) as OkObjectResult;
        GetHomeResponse response = (result!.Value as GetHomeResponse)!;
        
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
        PostHomeRequest request = new PostHomeRequest()
        {
            OwnerId = HomeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
            MaximumMembers = MaxHomeMembers
        };
        _mockHomeService.Setup(service => service.CreateHome(It.IsAny<Home>()));
        _mockHomeService.Setup(service => service.AddOwnerToHome(HomeOwnerId, It.IsAny<Home>()))
            .Returns(It.IsAny<Home>());
        
        ObjectResult? result = _homeController.PostHomes(request) as CreatedAtActionResult;
        
        Assert.AreEqual(CreatedStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestPostHomeNotFoundStatusCode()
    {
        PostHomeRequest request = new PostHomeRequest()
        {
            OwnerId = HomeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
            MaximumMembers = MaxHomeMembers
        };
        _mockHomeService.Setup(service => service.CreateHome(It.IsAny<Home>()));
        _mockHomeService.Setup(service => service.AddOwnerToHome(HomeOwnerId, It.IsAny<Home>()))
            .Throws(new ElementNotFound(ElementNotFoundMessage));
        
        NotFoundObjectResult? result = _homeController.PostHomes(request) as NotFoundObjectResult;
        
        Assert.AreEqual(404, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestPostHomePreconditionFailedStatusCode()
    {
        PostHomeRequest request = new PostHomeRequest()
        {
            OwnerId = HomeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
            MaximumMembers = MaxHomeMembers
        };
        
        _mockHomeService.Setup(service => service.AddOwnerToHome(HomeOwnerId, It.IsAny<Home>()))
            .Throws(new CannotAddItem("Member is not a home owner"));
        
        ObjectResult? result2 = (ObjectResult?)_homeController.PostHomes(request);
        
        Assert.AreEqual(412, result2!.StatusCode);
    }

    [TestMethod]
    public void TestGetMembersFromHomeOkResponse()
    {
        _mockHomeService.Setup(service => service.GetMembersFromHome(It.IsAny<long>())).Returns(_members);
        GetMembersResponse expectedResponse = new GetMembersResponse(_members);

        ObjectResult? result = _homeController.GetMembersFromHome(HomeOwnerId) as OkObjectResult;
    
        Assert.IsNotNull(result);
        GetMembersResponse actualResponse = result.Value as GetMembersResponse;
    
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
    public void TestPutMemberInHomeOkStatusCode()
    {
        PostHomeMemberRequest postHomeMemberRequest = new PostHomeMemberRequest()
        {
            UserEmail = Email,
            HasPermissionToAddADevice = Permission,
            HasPermissionToListDevices = Permission,
            ReceivesNotifications = Permission
        };

        _mockHomeService.Setup(service => service.AddMemberToHome(_home.Id , postHomeMemberRequest.ToEntity()));
    
        _homeController = new HomeController(_mockHomeService.Object); 
        
        ObjectResult? result = _homeController.AddMemberToHome(_home.Id, postHomeMemberRequest) as OkObjectResult;

        Assert.AreEqual(OKStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestPutMemberInHomeNotFoundStatusCode()
    {
        PostHomeMemberRequest postHomeMemberRequest = new PostHomeMemberRequest()
        {
            UserEmail = Email,
            HasPermissionToAddADevice = Permission,
            HasPermissionToListDevices = Permission,
            ReceivesNotifications = Permission
        };
        
        _mockHomeService
            .Setup(service => service.AddMemberToHome(_home.Id, It.IsAny<MemberDTO>()))
            .Throws(new ElementNotFound(ElementNotFoundMessage));
        
        _homeController = new HomeController(_mockHomeService.Object); 
        
        ObjectResult? result = _homeController.AddMemberToHome(_home.Id, postHomeMemberRequest) as ObjectResult;
    
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
    
        
    [TestMethod]
    public void TestTryToPutMemberThatAlreadyExistsStatusCode()
    {
        PostHomeMemberRequest postHomeMemberRequest = new PostHomeMemberRequest()
        {
            UserEmail = Email,
            HasPermissionToAddADevice = Permission,
            HasPermissionToListDevices = Permission,
            ReceivesNotifications = Permission
        };
        
        _mockHomeService
            .Setup(service => service.AddMemberToHome(_home.Id, It.IsAny<MemberDTO>()))
            .Throws(new ElementAlreadyExist(ElementAlreadyExistsMessage));
        
        _homeController = new HomeController(_mockHomeService.Object); 

        ObjectResult? result = _homeController.AddMemberToHome(_home.Id, postHomeMemberRequest) as ObjectResult;
    
        Assert.AreEqual(ConflictStatusCode, result.StatusCode);
    }
    
    public void TestTryToPutMemberInAFullHomeStatusCode()
    {
        PostHomeMemberRequest postHomeMemberRequest = new PostHomeMemberRequest()
        {
            UserEmail = Email,
            HasPermissionToAddADevice = Permission,
            HasPermissionToListDevices = Permission,
            ReceivesNotifications = Permission
        };
        
        _mockHomeService
            .Setup(service => service.AddMemberToHome(_home.Id, It.IsAny<MemberDTO>()))
            .Throws(new CannotAddItem(CannotAddItem));
        
        _homeController = new HomeController(_mockHomeService.Object); 

        ObjectResult? result = _homeController.AddMemberToHome(_home.Id, postHomeMemberRequest) as ObjectResult;
        
        Assert.AreEqual(PreconditionFailedStatusCode, result.StatusCode);
    }

    [TestMethod]
    public void TestCannotAddMemberToHomePreconditionFailed()
    {
        _mockHomeService
            .Setup(service => service.AddMemberToHome(It.IsAny<long>(), It.IsAny<MemberDTO>()))
            .Throws(new CannotAddItem(CannotAddItem));
        
        _homeController.AddMemberToHome(1, new PostHomeMemberRequest());
    }
    
    [TestMethod]
    public void TestPutDevicesInHomeOkStatusCode()
    {
        DeviceUnitRequest deviceUnitRequest = new DeviceUnitRequest()
        {
            DeviceId = _defaultWindowSensor.Id,
            IsConnected = true
        };
        
        PostHomeDevicesRequest request = new PostHomeDevicesRequest()
        {
            DeviceUnits = new List<DeviceUnitRequest> {deviceUnitRequest}
        };
        
        _mockHomeService.Setup(service => service.GetHomeById(It.IsAny<long>())).Returns(_home);
        _mockHomeService
            .Setup(service => service
                .AddDevicesToHome(It.IsAny<long>(), It.IsAny<List<DeviceUnitDTO>>()));
        
        ObjectResult? result = _homeController.AddDevicesToHome(_home.Id,request) as OkObjectResult;
    
        Assert.AreEqual(OKStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestPutDevicesInHomeOkResponse()
    {
        PostHomeDevicesRequest request = new PostHomeDevicesRequest()
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
        _mockHomeService.Setup(service => service.GetHomeById(It.IsAny<long>())).Returns(_home);
        _mockHomeService
            .Setup(service => service
                .AddDevicesToHome(It.IsAny<long>(), It.IsAny<List<DeviceUnitDTO>>()));

        ObjectResult? result = _homeController.AddDevicesToHome(_home.Id,request) as OkObjectResult;
    
        Assert.IsNotNull(result);
    }
    
    [TestMethod]
    public void TestChangePermissionsToMemberOkResponse()
    {
        PatchHomeMemberRequest request = new PatchHomeMemberRequest()
        {
            MemberEmail = Email,
            ReceivesNotifications = Permission
            
        };
        _mockHomeService
            .Setup(service => service
                .UpdateMemberNotificationPermission(It.IsAny<MemberDTO>(), It.IsAny<long>()));
    
        ObjectResult? result = _homeController.ChangeNotificationPermission(_home.Id,request) as OkObjectResult;
    
        Assert.IsNotNull(result);
    }
    
    [TestMethod]
    public void TestChangePermissionsToMemberNotFoundResponse()
    {
        PatchHomeMemberRequest request = new PatchHomeMemberRequest()
        {
            MemberEmail = Email,
            ReceivesNotifications = Permission
            
        };
        _mockHomeService
            .Setup(service => service
                .UpdateMemberNotificationPermission(It.IsAny<MemberDTO>(), It.IsAny<long>())).Throws(new ElementNotFound(ElementNotFoundMessage));
    
        ObjectResult? result = _homeController.ChangeNotificationPermission(_home.Id,request) as ObjectResult;
    
        Assert.AreEqual(NotFoundStatusCode,result.StatusCode);
    }
    
    
    [TestMethod]
    public void TestPutDevicesInHomeNotFoundWhenInputIsInvalid()
    {
        PostHomeDevicesRequest request = new PostHomeDevicesRequest() {};
        _mockHomeService.Setup(service => service.GetHomeById(It.IsAny<long>())).Returns(_home);
        _mockHomeService
            .Setup(service => service
                .AddDevicesToHome(It.IsAny<long>(), It.IsAny<List<DeviceUnitDTO>>()))
            .Throws(new ElementNotFound(ElementNotFoundMessage));
    
        IActionResult result = _homeController.AddDevicesToHome(-1,request);
    
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public void TestGetDevicesUnitOkResponse()
    {
        List<DeviceUnit> devicesUnit = new List<DeviceUnit>
        {
            new DeviceUnit()
            {
                Device = _defaultWindowSensor,
                IsConnected = true
            }
        };
        _mockHomeService.Setup(service => service.GetDevicesFromHome(It.IsAny<int>())).Returns(devicesUnit);
        GetDeviceUnitsResponse expectedResponse = new GetDeviceUnitsResponse(devicesUnit);
        
        ObjectResult? result = _homeController.GetDevicesFromHome(1) as OkObjectResult;
        GetDeviceUnitsResponse response = (result!.Value as GetDeviceUnitsResponse)!;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestGetDevicesUnitNotFoundStatusCode()
    {
        _mockHomeService.Setup(service => service.GetDevicesFromHome(It.IsAny<int>()))
            .Throws(new ElementNotFound(ElementNotFoundMessage));
        
        IActionResult result = _homeController.GetDevicesFromHome(1);
        
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public void TestUpdateDeviceStatusOkStatusCode()
    {
        PatchDeviceRequest request = new PatchDeviceRequest()
        {
            HardwareId = new Guid(),
            IsConnected = true
        };
        
        _mockHomeService.Setup(service => service.UpdateDeviceConnectionStatus(It.IsAny<long>(),It.IsAny<DeviceUnit>())); 
        _homeController = new HomeController(_mockHomeService.Object);
        
        ObjectResult? result = _homeController.UpdateDeviceConnectionStatus(_home.Id,request) as OkObjectResult;
        
        Assert.AreEqual(OKStatusCode, result.StatusCode);
        
    }
    
    [TestMethod]
    public void TestUpdateDeviceStatusNotFoundStatusCode()
    {
        PatchDeviceRequest request = new PatchDeviceRequest()
        {
            HardwareId = new Guid(),
            IsConnected = true
        };
        
        _mockHomeService.Setup(service => service.UpdateDeviceConnectionStatus(It.IsAny<long>(),It.IsAny<DeviceUnit>()))
            .Throws(new ElementNotFound(ElementNotFoundMessage));
        _homeController = new HomeController(_mockHomeService.Object);
        
        ObjectResult? result = _homeController.UpdateDeviceConnectionStatus(_home.Id,request) as ObjectResult;
        
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
        
    }
    
    private GetHomeResponse DefaultHomeResponse()
    {
        return new GetHomeResponse(_home);
    }

    private GetHomesResponse DefaultHomesResponse()
    {
        return new GetHomesResponse(_homes);
    }
    
    private void SetupDefaultObjects()
    {

        SetupHomes();
        SetupUsers();
        SetupWindowSensor();
    }
    
    private void SetupUsers()
    {
        User user1 = new User { Id = HomeOwnerId, Name = Name, Email = Email };
        User user2 = new User { Id = HomeOwnerId2, Name = Name2, Email = Email2 };
        _member1 = new Member(user1);
        _member2 = new Member(user2); 
        _members = new List<Member> {_member1, _member2};
        
        _defaultOwner = new User()
        {
            Email = Email,
            Id = HomeOwnerId,
            Roles = new List<Role>()
            {
                new HomeOwner(),
            }
        };
    }

    private void SetupHomes()
    {
        _home = new Home()
        {
            Id = 1,
            Owner = _defaultOwner,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude
        };

        _home2 = new Home()
        {
            Owner = _defaultOwner,
            Street = Street2,
            DoorNumber = DoorNumber2,
            Latitude = Latitude2,
            Longitude = Longitude2
        };
        
        _homes = new List<Home>
        {
            _home, _home2
        };
        
        _empyHomes = new List<Home>();
    }
    
    private void SetupWindowSensor()
    {
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