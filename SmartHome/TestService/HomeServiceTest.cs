using BusinessLogic.Services;
using Domain;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class HomeServiceTest
{
    private Mock<IRepository<Home>> _mockHomeRepository;
    private Home _home;
    
    private const string Street = "Calle del Sol";
    private const int DoorNumber = 23;
    private const double Latitude = 34.0207;
    private const double Longitude = -118.4912;
    
    [TestInitialize]
    public void TestInitialize()
    {
        CreateMockHomeRepository();
    }

    private void CreateMockHomeRepository()
    {
        _mockHomeRepository = new Mock<IRepository<Home>>();
    }
    
    [TestMethod]
    public void TestGetAllHomes()
    {
        List<Home> homes = new List<Home>();
        Home newHome = new Home(Street, DoorNumber, Latitude, Longitude);
        homes.Add(newHome);
        _mockHomeRepository.Setup(m => m.GetAll()).Returns(homes);
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        List<Home> retrievedHomes = homeService.GetAllHomes();
        Assert.AreEqual(homes, retrievedHomes); 
    }
    
    [TestMethod]
    public void TestCreateHome()
    {
        Home newHome = new Home(Street, DoorNumber, Latitude, Longitude);
        _mockHomeRepository.Setup(m => m.Add(newHome));
        HomeService homeService = new HomeService(_mockHomeRepository.Object);
        homeService.CreateHome(newHome);
        _mockHomeRepository.Verify(m => m.Add(newHome), Times.Once);
    }
}