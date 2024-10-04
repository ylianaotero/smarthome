using BusinessLogic;
using CustomExceptions;
using Domain;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class HomeServiceTest
{
    private Mock<IRepository<Home>> _mockHomeRepository;
    private Home _home;
    
    private Home _defaultHome;
    private const string Street = "Calle del Sol";
    private const int DoorNumber = 23;
    private const double Latitude = 34.0207;
    private const double Longitude = -118.4912;
    private const long homeOwnerId = 000;
    private const string NewEmail = "juan.perez@example.com";
    private const string NewEmail2 = "juan.lopez@example.com";
    private const string NameSecurityCamera = "Camara de Seguridad";
    private const string NameWindowSensor = "Sensor de Ventana";
    private const string DescriptionWindowSensor = "Sensor para ventanas";
    private const string DescriptionSecurityCamera = "Cámara para exteriores";
    private const int ModelWindowSensor = 456;
    private const int ModelSecurityCamera = 123;
    private const int Id1 = 1;
    private const int Id2 = 999;
    private const int Id3 = 2;
    private const bool IsConectedTrue = true;
    private const bool IsConnectedFalse = false;

    private User _user1;
    private User _user2;

    private Member _member2;
    private Member _member1; 
    
    [TestInitialize]
    public void TestInitialize()
    {
        CreateMockHomeRepository();
        SetupDefaultObjects();
    }

    private void SetupDefaultObjects()
    {
        _user1 = new User() {Email = NewEmail};
        _user2 = new User() {Email = NewEmail2};
        _member1 = new Member(_user1);
        _member2 = new Member(_user2); 
        
        _defaultHome = new Home()
        {
            OwnerId = homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude
        };
    }

    private void CreateMockHomeRepository()
    {
        _mockHomeRepository = new Mock<IRepository<Home>>();
    }
    
    [TestMethod]
    public void TestGetAllHomes()
    {
        List<Home> homes = new List<Home>();
        Home newHome = new Home()
        {
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude
        };
        homes.Add(newHome);
        _mockHomeRepository.Setup(m => m.GetAll(It.IsAny<PageData>())).Returns(homes);
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        List<Home> retrievedHomes = homeService.GetAllHomes();
        Assert.AreEqual(homes, retrievedHomes); 
    }
    
    [TestMethod]
    public void TestCreateHome()
    {
        _mockHomeRepository.Setup(m => m.Add(_defaultHome));
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        homeService.CreateHome(_defaultHome);
        _mockHomeRepository.Verify(m => m.Add(_defaultHome), Times.Once);
    }
    
    [TestMethod]
    public void TestGetMembersByHomeId()
    { 
        List<Member> members = new List<Member>
        {
            _member1, _member2
        };
        _home = new Home()
        {
            OwnerId = homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
        };
        foreach (Member member in members)
        {
            _home.AddMember(member);
        }
        _mockHomeRepository.Setup(m => m.GetById(1)).Returns(_home);
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        List<Member> retrievedMembers = homeService.GetMembersFromHome(1);
        CollectionAssert.AreEqual(members, retrievedMembers); 
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotGetMemberByIdBecauseHomeDoesNotExist()
    {
        int searchedHomeId = Id2;
    
        _mockHomeRepository.Setup(m => m.GetById(searchedHomeId)).Returns((Home)null);

        HomeService homeService = new HomeService(_mockHomeRepository.Object);

        homeService.GetMembersFromHome(searchedHomeId);
    }

    
    [TestMethod]
    public void TestGetDevicesFromHome()
    {
        List<Device> devices = new List<Device>
        {
            new SecurityCamera
            {
                Id = Id1, 
                Name = NameSecurityCamera, 
                Model = ModelSecurityCamera, 
                Description = DescriptionSecurityCamera, 
                IsConnected = IsConectedTrue
            },
            
            new WindowSensor 
            { 
                Id = Id3, 
                Name = NameWindowSensor, 
                Model = ModelWindowSensor, 
                Description = DescriptionWindowSensor, 
                IsConnected = IsConnectedFalse 
            }
        };
    
        _home = new Home()
        {
            OwnerId = homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
        };
        
        foreach (Device device in devices)
        {
            _home.AddDevice(device);
        }
    
        _mockHomeRepository.Setup(m => m.GetById(1)).Returns(_home);
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        List<Device> retrievedDevices = homeService.GetDevicesFromHome(1);
        CollectionAssert.AreEqual(devices, retrievedDevices);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotGetDeviceFromHomeBecauseItIsNotFound()
    {
        int searchedId = Id2;
        _mockHomeRepository.Setup(m => m.GetById(searchedId)).Returns((Home)null);
    
        HomeService homeService = new HomeService(_mockHomeRepository.Object);

        homeService.GetDevicesFromHome(searchedId);
    }
    
    [TestMethod]
    public void TestAddMemberToHome()
    {
        _home = new Home()
        {
            OwnerId = homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
        };
        _mockHomeRepository.Setup(m => m.GetById(_home.Id)).Returns(_home);

        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        
        homeService.AddMemberToHome(_home.Id, _member1);
        
        _mockHomeRepository.Verify(m => m.Update(_home), Times.Once);
        
        Assert.IsTrue(_home.Members.Contains(_member1));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void TestCannotAddMemberToHomeBecauseItIsAlreadyExist()
    {
        _home = new Home()
        {
            OwnerId = homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
        };
        _home.AddMember(_member1);
    
        _mockHomeRepository.Setup(m => m.GetById(_home.Id)).Returns(_home);

        HomeService homeService = new HomeService(_mockHomeRepository.Object);

        homeService.AddMemberToHome(_home.Id, _member1);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestCannotAddMemberToHomeBecauseItIsNotFound()
    {
        int nonExistentHomeId = Id2;
    
        _mockHomeRepository.Setup(m => m.GetById(nonExistentHomeId)).Returns((Home)null);

        HomeService homeService = new HomeService(_mockHomeRepository.Object);

        homeService.AddMemberToHome(nonExistentHomeId, _member1);
    }

    [TestMethod]
    public void TestGetHomesByFilter()
    {
        Func<Home, bool> filter = home => home.Street == Street;
        List<Home> homes = new List<Home>
        {
            new Home()
            {
                OwnerId = homeOwnerId,
                Street = Street,
                DoorNumber = DoorNumber,
                Latitude = Latitude,
                Longitude = Longitude,
            }
        };
        _mockHomeRepository
            .Setup(m => m.GetByFilter(filter, It.IsAny<PageData>()))
            .Returns(homes);
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        
        List<Home> retrievedHomes = homeService.GetHomesByFilter(filter);
        
        CollectionAssert.AreEqual(homes, retrievedHomes);
    }
    
    [TestMethod]
    public void TestGetHomeById()
    {
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns(_defaultHome);
        
        Home retrivedHome = homeService.GetHomeById(1);
        Assert.AreEqual(_defaultHome, retrivedHome);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestGetHomeByIdThrowsElementNotFound()
    {
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns((Home?)null);
        
        homeService.GetHomeById(1);
    }

    [TestMethod]
    public void TestPutDevicesInHome()
    {
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        Home home = new Home()
        {
            OwnerId = homeOwnerId,
            Street = Street,
            DoorNumber = DoorNumber,
            Latitude = Latitude,
            Longitude = Longitude,
        };
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns(home);
        
        List<Device> homeDevices = new List<Device>
        {
            new WindowSensor
            {
                Id = Id1, 
                Name = NameWindowSensor, 
                Model = ModelWindowSensor, 
                Description = DescriptionWindowSensor, 
                IsConnected = false ,
            },
            new SecurityCamera
            {
                Id = Id1, 
                Name = NameSecurityCamera, 
                Model = ModelSecurityCamera, 
                Description = DescriptionSecurityCamera, 
                IsConnected = true
            }
        };
        
        homeService.PutDevicesInHome(1, homeDevices);
        Assert.AreEqual(home.Devices,homeDevices);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestPutDevicesInHomeThrowsElementNotFound()
    {
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        _mockHomeRepository.Setup(x=>x.GetById(1)).Returns((Home?)null);
        
        List<Device> homeDevices = new List<Device>
        {
            new WindowSensor
            {
                Id = Id1, 
                Name = NameWindowSensor, 
                Model = ModelWindowSensor, 
                Description = DescriptionWindowSensor, 
                IsConnected = false ,
            },
            new SecurityCamera
            {
                Id = Id1, 
                Name = NameSecurityCamera, 
                Model = ModelSecurityCamera, 
                Description = DescriptionSecurityCamera, 
                IsConnected = true
            }
        };
        
        homeService.PutDevicesInHome(1, homeDevices);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotAddItem))]
    public void TestTryToCreateExistingHome()
    {
        _mockHomeRepository.Setup(m => m.GetById(_defaultHome.Id)).Returns(_defaultHome);
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        homeService.CreateHome(_defaultHome);
        _mockHomeRepository.Verify(m => m.GetById(_defaultHome.Id), Times.Once);
    }
    
}