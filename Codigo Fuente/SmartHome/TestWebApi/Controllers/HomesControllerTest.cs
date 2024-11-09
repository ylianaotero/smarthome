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
    private Mock<IDeviceUnitService> _mockDeviceUnitService;
    private Mock<IMemberService> _mockMemberService;
    private User _defaultOwner;
    private Member _member1;
    private Member _member2;
    private Home _home;
    private Home _home2;
    private List<Home> _homes;
    private List<Member> _members;
    private WindowSensor _defaultWindowSensor;
    private Company _defaultCompany;
    private Room _defaultRoom;
    
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
    private const string Alias = "Mi Casa";
    private const string CustomName = "Living Camera";
    private const string RoomName = "Living Room";
    private const int DoorNumber = 2;
    private const string? DoorNumberString = "742";
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
    private const string DeviceModel = "1345354616346";

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

        GetHomeRequest request = new GetHomeRequest()
        {
            DoorNumber = DoorNumberString,
            Street = Street
        };

        ObjectResult? result = _homeController.GetHomes(request) as OkObjectResult;
        
        Assert.AreEqual(OKStatusCode, result!.StatusCode);
    }

    [TestMethod]
    public void TestGetAllHomesWhenThereAreNoOkStatusCode()
    {
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
    [ExpectedException(typeof(InputNotValid))]
    public void TestPostHomeWithoutAliasOkStatusCode()
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
    public void TestPostHomeWithAliasOkStatusCode()
    {
        PostHomeRequest request = new PostHomeRequest()
        {
            OwnerId = HomeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
            MaximumMembers = MaxHomeMembers,
            Alias = Alias
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
            Alias = Alias,
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
            Alias = Alias,
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
    public void TestPatchHomeAliasOkStatusCode()
    {
        PatchHomeRequest request = new PatchHomeRequest()
        {
            Alias = Alias
        };
        
        _mockHomeService.Setup(service => service.UpdateHomeAlias(It.IsAny<long>(), It.IsAny<string>()));
        
        ObjectResult? result = _homeController.UpdateHomeAlias(1,request) as OkObjectResult;
        
        Assert.AreEqual(OKStatusCode, result!.StatusCode);
    }
    
    [TestMethod]
    public void TestPatchHomeAliasNotFoundStatusCode()
    {
        PatchHomeRequest request = new PatchHomeRequest()
        {
            Alias = Alias
        };

        _mockHomeService.Setup(service => service.UpdateHomeAlias(It.IsAny<long>(), It.IsAny<string>()))
            .Throws(new ElementNotFound(ElementNotFoundMessage));

        IActionResult result = _homeController.UpdateHomeAlias(1, request);

        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));

        ObjectResult? objectResult = result as ObjectResult;
        Assert.AreEqual(NotFoundStatusCode, objectResult!.StatusCode);
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

        _mockMemberService.Setup(service => service.AddMemberToHome(_home.Id , postHomeMemberRequest.ToEntity()));
    
        _homeController = new HomeController(_mockHomeService.Object, _mockDeviceUnitService.Object, _mockMemberService.Object); 
        
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
        
        _mockMemberService
            .Setup(service => service.AddMemberToHome(_home.Id, It.IsAny<MemberDTO>()))
            .Throws(new ElementNotFound(ElementNotFoundMessage));
        
        _homeController = new HomeController(_mockHomeService.Object, _mockDeviceUnitService.Object, _mockMemberService.Object); 
        
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
        
        _mockMemberService
            .Setup(service => service.AddMemberToHome(_home.Id, It.IsAny<MemberDTO>()))
            .Throws(new ElementAlreadyExist(ElementAlreadyExistsMessage));
        
        _homeController = new HomeController(_mockHomeService.Object, _mockDeviceUnitService.Object, _mockMemberService.Object); 

        ObjectResult? result = _homeController.AddMemberToHome(_home.Id, postHomeMemberRequest) as ObjectResult;
    
        Assert.AreEqual(ConflictStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestTryToPutMemberInAFullHomeStatusCode()
    {
        PostHomeMemberRequest postHomeMemberRequest = new PostHomeMemberRequest()
        {
            UserEmail = Email,
            HasPermissionToAddADevice = Permission,
            HasPermissionToListDevices = Permission,
            ReceivesNotifications = Permission
        };
        
        _mockMemberService
            .Setup(service => service.AddMemberToHome(_home.Id, It.IsAny<MemberDTO>()))
            .Throws(new CannotAddItem(CannotAddItem));
        
        _homeController = new HomeController(_mockHomeService.Object, _mockDeviceUnitService.Object, _mockMemberService.Object); 

        ObjectResult? result = _homeController.AddMemberToHome(_home.Id, postHomeMemberRequest) as ObjectResult;
        
        Assert.AreEqual(PreconditionFailedStatusCode, result.StatusCode);
    }

    [TestMethod]
    public void TestCannotAddMemberToHomePreconditionFailed()
    {
        _mockMemberService
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
        _mockDeviceUnitService
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
        _mockDeviceUnitService
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
        _mockMemberService
            .Setup(service => service
                .UpdateMemberNotificationPermission( It.IsAny<long>(), It.IsAny<MemberDTO>()));
    
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
        _mockMemberService
            .Setup(service => service
                .UpdateMemberNotificationPermission(It.IsAny<long>(), It.IsAny<MemberDTO>())).Throws(new ElementNotFound(ElementNotFoundMessage));
    
        ObjectResult? result = _homeController.ChangeNotificationPermission(_home.Id,request) as ObjectResult;
    
        Assert.AreEqual(NotFoundStatusCode,result.StatusCode);
    }
    
    
    [TestMethod]
    public void TestPutDevicesInHomeNotFoundWhenInputIsInvalid()
    {
        PostHomeDevicesRequest request = new PostHomeDevicesRequest() {};
        _mockHomeService.Setup(service => service.GetHomeById(It.IsAny<long>())).Returns(_home);
        _mockDeviceUnitService
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
                IsConnected = true,
                Name = "Window Sensor",
                HardwareId = Guid.NewGuid()
            }
        };
        _mockHomeService.Setup(service => service.GetHomeById(It.IsAny<int>())).Returns(_home); 
        _mockHomeService.Setup(service => service.GetDevicesFromHome(_home.Id)).Returns(devicesUnit);
        GetDeviceUnitsResponse expectedResponse = new GetDeviceUnitsResponse(devicesUnit);
        
        ObjectResult? result = _homeController.GetDevicesFromHome(1) as OkObjectResult;
        GetDeviceUnitsResponse response = (result!.Value as GetDeviceUnitsResponse)!;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestGetDevicesUnitNotFoundStatusCode()
    {
        _mockHomeService.Setup(service => service.GetDevicesFromHome(_home.Id))
            .Throws(new ElementNotFound(ElementNotFoundMessage));
        
        IActionResult result = _homeController.GetDevicesFromHome(1);
        
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public void TestUpdateDeviceStatusOkStatusCode()
    {
        PatchDeviceUnitRequest unitRequest = new PatchDeviceUnitRequest()
        {
            HardwareId = new Guid(),
            IsConnected = true
        };
        
        _mockDeviceUnitService
            .Setup(service => service
                .UpdateDeviceUnit(It.IsAny<long>(), unitRequest.ToEntity())); 
        _homeController = new HomeController(_mockHomeService.Object, _mockDeviceUnitService.Object, _mockMemberService.Object);
        
        ObjectResult? result = _homeController.UpdateDevice(_home.Id, unitRequest) as OkObjectResult;
        
        Assert.AreEqual(OKStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestUpdateDeviceStatusNotFoundStatusCode()
    {
        PatchDeviceUnitRequest unitRequest = new PatchDeviceUnitRequest()
        {
            HardwareId = Guid.NewGuid(),
            IsConnected = true
        };
        
        _mockDeviceUnitService
            .Setup(service => service
                .UpdateDeviceUnit(It.IsAny<long>(),It.IsAny<DeviceUnitDTO>()))
            .Throws(new ElementNotFound(ElementNotFoundMessage));
        _homeController = new HomeController(_mockHomeService.Object, _mockDeviceUnitService.Object, _mockMemberService.Object);
        
        ObjectResult? result = _homeController.UpdateDevice(_home.Id, unitRequest) as ObjectResult;
        
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }

    [TestMethod]
    public void TestUpdateDeviceUnitNameOkStatusCode()
    {
        PatchDeviceUnitRequest request = new PatchDeviceUnitRequest()
        {
            Name = CustomName
        };
        
        _mockDeviceUnitService
            .Setup(service => service
                .UpdateDeviceUnit(It.IsAny<long>(), request.ToEntity()));
        _homeController = new HomeController
            (_mockHomeService.Object, _mockDeviceUnitService.Object, _mockMemberService.Object);
        
        ObjectResult? result = _homeController.UpdateDevice(_home.Id, request) as OkObjectResult;
        
        Assert.AreEqual(OKStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestUpdateDeviceUnitNameNotFoundStatusCode()
    {
        PatchDeviceUnitRequest request = new PatchDeviceUnitRequest()
        {
            HardwareId = Guid.NewGuid(),
            Name = CustomName
        };
        
        _mockDeviceUnitService
            .Setup(service => service
                .UpdateDeviceUnit(It.IsAny<long>(), It.IsAny<DeviceUnitDTO>()))
            .Throws(new ElementNotFound(ElementNotFoundMessage));
        _homeController = new HomeController
            (_mockHomeService.Object, _mockDeviceUnitService.Object, _mockMemberService.Object);
        
        ObjectResult? result = _homeController.UpdateDevice(_home.Id, request) as ObjectResult;
        
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestAddRoomToHomeOkStatusCode()
    {
        PostHomeRoomRequest request = new PostHomeRoomRequest()
        {
            Name = _defaultRoom.Name
        };
        
        _mockHomeService.Setup(service => service.AddRoomToHome(It.IsAny<long>(), It.IsAny<Room>()));
        _homeController = new HomeController(_mockHomeService.Object, _mockDeviceUnitService.Object, _mockMemberService.Object);
        
        ObjectResult? result = _homeController.AddRoomToHome(_home.Id,request) as OkObjectResult;
        
        Assert.AreEqual(OKStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestAddRoomToHomeNotFoundStatusCode()
    {
        PostHomeRoomRequest request = new PostHomeRoomRequest()
        {
            Name = _defaultRoom.Name
        };
        
        _mockHomeService
            .Setup(service => service.AddRoomToHome(It.IsAny<long>(), It.IsAny<Room>()))
            .Throws(new ElementNotFound(ElementNotFoundMessage));
        
        _homeController = new HomeController(_mockHomeService.Object, _mockDeviceUnitService.Object, _mockMemberService.Object);

        ObjectResult? result = _homeController.AddRoomToHome(_home.Id, request) as NotFoundObjectResult;
        
        Assert.AreEqual(NotFoundStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestAddRoomToHomeConflictStatusCode()
    {
        PostHomeRoomRequest request = new PostHomeRoomRequest()
        {
            Name = _defaultRoom.Name
        };
        
        _mockHomeService
            .Setup(service => service.AddRoomToHome(It.IsAny<long>(), It.IsAny<Room>()))
            .Throws(new ElementAlreadyExist(ElementAlreadyExistsMessage));
        
        _homeController = new HomeController
            (_mockHomeService.Object, _mockDeviceUnitService.Object, _mockMemberService.Object);

        ObjectResult? result = _homeController.AddRoomToHome(_home.Id, request) as ConflictObjectResult;
        
        Assert.AreEqual(ConflictStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestUpdateDeviceRoomOkStatusCode()
    {
        PatchDeviceUnitRequest unitRequest = new PatchDeviceUnitRequest()
        {
            HardwareId = Guid.NewGuid(),
            RoomId = _defaultRoom.Id,
        };

        _mockDeviceUnitService
            .Setup(service => service.UpdateDeviceUnit(_home.Id, unitRequest.ToEntity()));


        ObjectResult? result = _homeController.UpdateDevice(_home.Id, unitRequest) as OkObjectResult;
        
        Assert.AreEqual(OKStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestGetHomeRoomsOkStatusCode()
    {
        _mockHomeService.Setup(service => service.GetHomeById(It.IsAny<long>())).Returns(_home);
        _mockHomeService.Setup(service => service.GetRoomsFromHome(It.IsAny<long>())).Returns(_home.Rooms);

        ObjectResult? result = _homeController.GetRooms(_home.Id) as OkObjectResult;
        
        Assert.AreEqual(OKStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void TestGetHomeRoomsOkResponse()
    {
        GetRoomsResponse expectedResponse = new GetRoomsResponse(_home.Rooms);
        
        _mockHomeService.Setup(service => service.GetHomeById(It.IsAny<long>())).Returns(_home);
        _mockHomeService.Setup(service => service.GetRoomsFromHome(It.IsAny<long>())).Returns(_home.Rooms);

        ObjectResult? result = _homeController.GetRooms(_home.Id) as OkObjectResult;
        GetRoomsResponse response = (result!.Value as GetRoomsResponse)!;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestGetHomeRoomsNotFoundStatusCode()
    {
        _mockHomeService
            .Setup(service => service.GetHomeById(It.IsAny<long>()))
            .Throws(new ElementNotFound(ElementNotFoundMessage));

        ObjectResult? result = _homeController.GetRooms(_home.Id) as OkObjectResult;
        
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
        SetupRoom();
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
            DoorNumber = DoorNumber,
            Latitude = Latitude2,
            Longitude = Longitude2
        };
        
        _homes = new List<Home>
        {
            _home, _home2
        };
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

    private void SetupRoom()
    {
        _defaultRoom = new Room()
        {
            Name = RoomName
        };
    }

    private void SetupHomeController()
    {
        _mockSessionService = new Mock<ISessionService>();
        _mockHomeService = new Mock<IHomeService>();
        _mockDeviceUnitService = new Mock<IDeviceUnitService>();
        _mockMemberService = new Mock<IMemberService>();
        _homeController = new HomeController(_mockHomeService.Object, _mockDeviceUnitService.Object, _mockMemberService.Object);
    }
}