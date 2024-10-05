using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/v1/testdata")]
[ApiController]
[AllowAnonymous]
public class TestData : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly ICompanyService _companyService;
    private readonly IUserService _userService;
    private readonly IHomeService _homeService;
    private readonly INotificationService _notificationService;
    
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new {message = "Hello World!"});
    }

    [HttpPost]
    public IActionResult Post()
    {
        Role administrator = new Administrator() {};
        Role companyOwner1 = new CompanyOwner() {};
        Role companyOwner2 = new CompanyOwner() {};
        Role homeOwner = new HomeOwner() {};
        
        User user1 = new User
        {
            Name = "Yliana",
            Surname = "Otero",
            Email = "ylianao@gmail.com",
            Password = "Password1%",
            Photo = "https://www.photo.com",
            Roles = new List<Role> {administrator}
        };
        
        User user2 = new User
        {
            Name = "Belen",
            Surname = "Rywaczuc",
            Email = "belenr@gmail.com",
            Photo = "https://www.photo.com",
            Password = "Password1%",
            Roles = new List<Role> {administrator, companyOwner1}
        };
        
        User user3 = new User
        {
            Name = "Angelina",
            Surname = "Maverino",
            Email = "angelinam@gmail.com",
            Photo = "https://www.photo.com",
            Password = "Password1%",
            Roles = new List<Role> {homeOwner, companyOwner2}
        };
        
        User user4 = new User
        {
            Name = "Bruno",
            Surname = "Apa",
            Email = "brunoa@gmail.com",
            Password = "Password1%",
            Photo = "https://www.photo.com",
            Roles = new List<Role> {homeOwner}
        };
        
        _userService.CreateUser(user1);
        _userService.CreateUser(user2);
        _userService.CreateUser(user3);
        _userService.CreateUser(user4);
        /*
        Company company1 = new Company
        {
            Name = "Company1",
            RUT = "123456789",
            LogoURL = "https://www.logo.com"
        };
        
        _companyService.CreateCompany(company1);
        
        _userService.AddOwnerToCompany(user2.Id, company1);
        
        Company company2 = new Company
        {
            Name = "Company2",
            RUT = "987654321",
            LogoURL = "https://www.logo.com"
        };
        
        _companyService.CreateCompany(company2);
        
        _userService.AddOwnerToCompany(user3.Id, company2);
        
        Home home1 = new Home
        {
            Street = "Calle Falsa",
            DoorNumber = 123,
            Latitude = 123.123,
            Longitude = 123.123,
            MaximumMembers = 5
        };
        
        _homeService.CreateHome(home1);
        _homeService.AddOwnerToHome(user4.Id, home1);
        
        
        MemberDTO member1 = new MemberDTO
        {
            UserEmail = user1.Email,
            HasPermissionToAddADevice = true,
            HasPermissionToListDevices = true,
            ReceivesNotifications = true
        };
        
        _homeService.AddMemberToHome(home1.Id, member1);
        
        Device securitycamera1 = new SecurityCamera()
        {
            Name = "Security Camera 1",
            Description = "Security Camera 1",
            Model = 1,
            PhotoURLs = new List<string> {"https://www.photo.com"},
            Company = company1,
            LocationType = LocationType.Indoor,
            Functionalities = new List<SecurityCameraFunctionality> {SecurityCameraFunctionality.MotionDetection}
        };
        
        _deviceService.CreateDevice(securitycamera1);
        
        Device securitycamera2 = new SecurityCamera()
        {
            Name = "Security Camera",
            Description = "Security Camera",
            Model = 2,
            PhotoURLs = new List<string> {"https://www.photo.com"},
            Company = company2,
            LocationType = LocationType.Outdoor,
            Functionalities = new List<SecurityCameraFunctionality> {SecurityCameraFunctionality.HumanDetection}
        };
        
        _deviceService.CreateDevice(securitycamera2);
        
        Device windowSensor = new WindowSensor()
        {
            Name = "Window Sensor",
            Description = "Window Sensor",
            Model = 3,
            PhotoURLs = new List<string> {"https://www.photo.com"},
            Company = company1,
            Functionalities = new List<WindowSensorFunctionality> {WindowSensorFunctionality.OpenClosed}
            
        };
        
        _deviceService.CreateDevice(windowSensor);
        
        DeviceUnitDTO deviceUnitDTO1 = new DeviceUnitDTO
        {
            DeviceId = securitycamera1.Id,
            IsConnected = true,
        };
        
        DeviceUnitDTO deviceUnitDTO2 = new DeviceUnitDTO
        {
            DeviceId = securitycamera2.Id,
            IsConnected = true,
        };
        
        DeviceUnitDTO deviceUnitDTO3 = new DeviceUnitDTO
        {
            DeviceId = windowSensor.Id,
            IsConnected = false,
        };
        
        _homeService.PutDevicesInHome(home1.Id, new List<DeviceUnitDTO> {deviceUnitDTO1, deviceUnitDTO2, deviceUnitDTO3});
        */
            
        return Ok("Data created successfully");
    }

}