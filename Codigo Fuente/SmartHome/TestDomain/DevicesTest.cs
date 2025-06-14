using CustomExceptions;
using Domain.Concrete;
using Domain.Enum;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestDomain;

[TestClass]
public class DevicesTest
{
    private SecurityCamera _securityCamera;
    private SecurityCamera _securityCamera1;
    private SecurityCamera _securityCamera2;
    private WindowSensor _windowSensor;
    private WindowSensor _windowSensor1;
    private WindowSensor _windowSensor2;
    private Company _company;
    private List<SecurityCameraFunctionality> _cameraFunctionalities;
    private List<WindowSensorFunctionality> _windowSensorfunctionalities;
    private List<MotionSensorFunctionality> _motionSensorFunctionalities;
    private List<SmartLampFunctionality> _smartLampFunctionalities;
    
    private const string CameraName = "My Security Camera";
    private const string WindowSensorName = "My Window Sensor";
    private const string MotionSensorName = "My Motion Sensor";
    private const string SmartLampName = "My Smart Lamp";
    private const string DeviceDescription = "This is a device";
    private const string DevicePhotoUrl = "https://example.com/photo.jpg";
    private const string DeviceModel = "1345354616346";
    
    private const string WindowSensorType = "WindowSensor";
    private const string SecurityCameraType = "SecurityCamera";
    private const string MotionSensorType = "MotionSensor";
    private const string SmartLampType = "SmartLamp";
    
    [TestInitialize]
    public void TestInitialize()
    {
        _company = new Company();
        _windowSensorfunctionalities = new List<WindowSensorFunctionality> {WindowSensorFunctionality.OpenClosed};
        _cameraFunctionalities = new List<SecurityCameraFunctionality> {SecurityCameraFunctionality.MotionDetection};
        _motionSensorFunctionalities = new List<MotionSensorFunctionality> {MotionSensorFunctionality.MotionDetection};
        _smartLampFunctionalities = new List<SmartLampFunctionality> {SmartLampFunctionality.OnOff};

        InitializeDevices();
    }

    private void InitializeDevices()
    {
        _securityCamera = new SecurityCamera();
        _windowSensor = new WindowSensor();
        _windowSensor1 = new WindowSensor()
        {
            Id = 1,
            Name = WindowSensorName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = _windowSensorfunctionalities,
            Company = _company
        };
        _windowSensor2 = new WindowSensor()
        {
            Id = 2,
            Name = WindowSensorName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = _windowSensorfunctionalities,
            Company = _company
        };
        
        _securityCamera1 = new SecurityCamera()
        {
            Id = 1,
            Name = CameraName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = _cameraFunctionalities,
            Company = _company
        };
        _securityCamera2 = new SecurityCamera()
        {
            Id = 2,
            Name = CameraName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = _cameraFunctionalities,
            Company = _company
        };
    }
    
    [TestMethod]
    public void TestAddNameToSecurityCamera()
    {
        _securityCamera.Name = CameraName;
        
        Assert.AreEqual(CameraName, _securityCamera.Name);
    }
    
    [TestMethod]
    public void TestAddModelToSecurityCamera()
    {
        _securityCamera.Model = DeviceModel;
        
        Assert.AreEqual(DeviceModel, _securityCamera.Model);
    }
    
    [TestMethod]
    public void TestAddPhotosToSecurityCamera()
    {
        List<string> photos = new List<string> { DevicePhotoUrl, DevicePhotoUrl };
        
        _securityCamera.PhotoURLs = photos;
        
        Assert.AreEqual(photos, _securityCamera.PhotoURLs);
    }

    [TestMethod]
    public void TestAddCompanyDataToSecurityCamera()
    {
        Company company = new Company(){};
        
        _securityCamera.Company = company;
        
        Assert.AreEqual(company, _securityCamera.Company);
    }
    
    [TestMethod]
    public void TestAddDescriptionToSecurityCamera()
    {
        _securityCamera.Description = DeviceDescription;
        
        Assert.AreEqual(DeviceDescription, _securityCamera.Description);
    }
    
    [TestMethod]
    public void TestAddIndoorLocationTypeToSecurityCamera()
    {
        LocationType locationType = LocationType.Indoor;
        
        _securityCamera.LocationType = locationType;
        
        Assert.AreEqual(locationType, _securityCamera.LocationType);
    }

    [TestMethod]
    public void TestAddOutdoorLocationTypeToSecurityCamera()
    {
        LocationType locationType = LocationType.Outdoor;
        
        _securityCamera.LocationType = locationType;
        
        Assert.AreEqual(locationType, _securityCamera.LocationType);
    }
    
    [TestMethod]
    public void TestAddMovementDetectionFunctionalityToSecurityCamera()
    {
        List<SecurityCameraFunctionality>? functionalities = new List<SecurityCameraFunctionality> 
            {SecurityCameraFunctionality.MotionDetection};
        
        _securityCamera.Functionalities = functionalities;
        
        Assert.AreEqual(functionalities, _securityCamera.Functionalities);
    }
    
    [TestMethod]
    public void TestAddHumanDetectionFunctionalityToSecurityCamera()
    {
        List<SecurityCameraFunctionality>? functionalities = new List<SecurityCameraFunctionality> 
            {SecurityCameraFunctionality.HumanDetection};
        
        _securityCamera.Functionalities = functionalities;
        
        Assert.AreEqual(functionalities, _securityCamera.Functionalities);
    }
    
    [TestMethod]
    public void TestAddOpenClosedFunctionalityToWindowSensor()
    {
        List<WindowSensorFunctionality>? functionalities = new List<WindowSensorFunctionality> 
            {WindowSensorFunctionality.OpenClosed};
        
        _windowSensor.Functionalities = functionalities;
        
        Assert.AreEqual(_windowSensor.Functionalities, functionalities);
    }
    
    [TestMethod]
    public void TestAddDeviceCharacteristicsToWindowSensor()
    {
        _windowSensor = new WindowSensor() 
        {
            Name = WindowSensorName,
        };
        
        Assert.IsNotNull(_windowSensor);
    }
    
    [TestMethod]
    public void TestWindowSensorTypeIsCorrect()
    {
        _windowSensor = new WindowSensor(){};
        
        Assert.AreEqual(WindowSensorType, _windowSensor.Kind);
    }
    
    [TestMethod]
    public void TestSecurityCameraTypeIsCorrect()
    {
        _securityCamera = new SecurityCamera(){};
        
        Assert.AreEqual(SecurityCameraType, _securityCamera.Kind);
    }

    [TestMethod]
    public void TestDifferentiationOfWindowSensorsViaId()
    {
        Assert.IsFalse(_windowSensor1.Equals(_windowSensor2));
    }
    
    [TestMethod]
    public void TestDifferentiationOfSecurityCamerasViaId()
    {
        Assert.IsFalse(_securityCamera1.Equals(_securityCamera2));
    }

    [TestMethod]
    public void TestCreateMotionSensor()
    {
        MotionSensor motionSensor = new MotionSensor() {Id = 1};
        
        Assert.AreEqual(1, motionSensor.Id);
    }
    
    [TestMethod]
    public void TestAddMotionDetectionFunctionalityToMotionSensor()
    {
        MotionSensor motionSensor = new MotionSensor();
        motionSensor.Functionalities = _motionSensorFunctionalities;
        
        Assert.AreEqual(_motionSensorFunctionalities, motionSensor.Functionalities);
    }
    
    [TestMethod]
    public void TestAddKindToMotionSensor()
    {
        MotionSensor motionSensor = new MotionSensor() {Kind = MotionSensorType};
        
        Assert.AreEqual(MotionSensorType, motionSensor.Kind);
    }
    
    [TestMethod]
    public void TestMotionSensorEquals()
    {
        MotionSensor motionSensor1 = new MotionSensor()
        {
            Id = 1,
            Name = MotionSensorName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = _motionSensorFunctionalities,
            Company = _company
        };
        
        MotionSensor motionSensor2 = new MotionSensor()
        {
            Id = 1,
            Name = MotionSensorName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = _motionSensorFunctionalities,
            Company = _company
        };
        
        Assert.IsTrue(motionSensor1.Equals(motionSensor2));
    }
    
    [TestMethod]
    public void TestCreateSmartLamp()
    {
        SmartLamp smartLamp = new SmartLamp() {Id = 1};
        
        Assert.AreEqual(1, smartLamp.Id);
    }
    
    [TestMethod]
    public void TestAddOnOffFunctionalityToSmartLamp()
    {
        SmartLamp smartLamp = new SmartLamp();
        smartLamp.Functionalities = _smartLampFunctionalities;
        
        Assert.AreEqual(_smartLampFunctionalities, smartLamp.Functionalities);
    }
    
    [TestMethod]
    public void TestAddKindToSmartLamp()
    {
        SmartLamp smartLamp = new SmartLamp() {Kind = SmartLampType};
        
        Assert.AreEqual(SmartLampType, smartLamp.Kind);
    }
    
    [TestMethod]
    public void TestSmartLampWithoutFunctionalitiesDefaultStatusIsEmpty()
    {
        SmartLamp smartLamp = new SmartLamp();
        
        string defaultStatus = "";
        
        Assert.AreEqual(defaultStatus, smartLamp.DefaultStatus());
    }
    
    [TestMethod]
    public void TestWindowSensorWithoutFunctionalitiesDefaultStatusIsEmpty()
    {
        WindowSensor windowSensor = new WindowSensor();
        
        string defaultStatus = "";
        
        Assert.AreEqual(defaultStatus, windowSensor.DefaultStatus());
    }
    
    [TestMethod]
    public void TestSmartLampWithFunctionalitiesDefaultStatusIsOff()
    {
        SmartLamp smartLamp = new SmartLamp();
        smartLamp.Functionalities = _smartLampFunctionalities;
        
        string defaultStatus = "Off";
        
        Assert.AreEqual(defaultStatus, smartLamp.DefaultStatus());
    }
    
    [TestMethod]
    public void TestWindowSensorWithFunctionalitiesDefaultStatusIsClosed()
    {
        WindowSensor windowSensor = new WindowSensor();
        windowSensor.Functionalities = _windowSensorfunctionalities;
        
        string defaultStatus = "Closed";
        
        Assert.AreEqual(defaultStatus, windowSensor.DefaultStatus());
    }
    
    [TestMethod]
    public void TestSecurityCameraDefaultStatusIsEmpty()
    {
        SecurityCamera securityCamera = new SecurityCamera();
        
        string defaultStatus = "";
        
        Assert.AreEqual(defaultStatus, securityCamera.DefaultStatus());
    }
    
    [TestMethod]
    public void TestMotionSensorWithoutFunctionalitiesDefaultStatusIsEmpty()
    {
        MotionSensor motionSensor = new MotionSensor();
        
        string defaultStatus = "";
        
        Assert.AreEqual(defaultStatus, motionSensor.DefaultStatus());
    }

    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestSmartLampValidateStatus()
    {
        SmartLamp smartLamp = new SmartLamp();
        
        smartLamp.ValidateStatus("Invalid");
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestWindowSensorValidateStatus()
    {
        WindowSensor windowSensor = new WindowSensor();
        
        windowSensor.ValidateStatus("Invalid");
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestSecurityCameraValidateStatus()
    {
        SecurityCamera securityCamera = new SecurityCamera();
        
        securityCamera.ValidateStatus("Invalid");
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestMotionSensorValidateStatus()
    {
        MotionSensor motionSensor = new MotionSensor();
        
        motionSensor.ValidateStatus("Invalid");
    }
    
    [TestMethod]
    public void TestSmartLampRunFunctionality()
    {
        SmartLamp smartLamp = new SmartLamp();
        smartLamp.Functionalities = _smartLampFunctionalities;
        
        string status = "On";
        
        Assert.AreEqual("Off", smartLamp.RunFunctionality("OnOff", status));
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestSmartLampRunFunctionalityThrowsException()
    {
        SmartLamp smartLamp = new SmartLamp();
        smartLamp.Functionalities = _smartLampFunctionalities;
        
        smartLamp.RunFunctionality("Invalid", "On");
    }
    
    [TestMethod]
    public void TestWindowSensorRunFunctionality()
    {
        WindowSensor windowSensor = new WindowSensor();
        windowSensor.Functionalities = _windowSensorfunctionalities;
        
        string status = "Open";
        
        Assert.AreEqual("Closed", windowSensor.RunFunctionality("OpenClosed", status));
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestWindowSensorRunFunctionalityThrowsException()
    {
        WindowSensor windowSensor = new WindowSensor();
        windowSensor.Functionalities = _windowSensorfunctionalities;
        
        windowSensor.RunFunctionality("Invalid", "Open");
    }
    
    [TestMethod]
    public void TestSecurityCameraRunFunctionality()
    {
        SecurityCamera securityCamera = new SecurityCamera();
        securityCamera.Functionalities = _cameraFunctionalities;
        
        string status = "";
        
        Assert.AreEqual(status, securityCamera.RunFunctionality("MotionDetection", status));
    }
    
    [TestMethod]
    [ExpectedException(typeof(InputNotValid))]
    public void TestSecurityCameraRunFunctionalityThrowsException()
    {
        SecurityCamera securityCamera = new SecurityCamera();
        securityCamera.Functionalities = _cameraFunctionalities;
        
        securityCamera.RunFunctionality("Invalid", "On");
    }
    
    [TestMethod]
    public void TestMotionSensorRunFunctionality()
    {
        MotionSensor motionSensor = new MotionSensor();
        motionSensor.Functionalities = _motionSensorFunctionalities;
        
        string status = "";
        
        Assert.AreEqual(status, motionSensor.RunFunctionality("MotionDetection", status));
    }
    
    [TestMethod]
    public void TestSmartLampEquals()
    {
        SmartLamp smartLamp1 = new SmartLamp()
        {
            Id = 1,
            Name = SmartLampName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = _smartLampFunctionalities,
            Company = _company
        };
        
        SmartLamp smartLamp2 = new SmartLamp()
        {
            Id = 1,
            Name = SmartLampName,
            Model = DeviceModel,
            Description = DeviceDescription,
            PhotoURLs = new List<string> { DevicePhotoUrl },
            Functionalities = _smartLampFunctionalities,
            Company = _company
        };
        
        Assert.IsTrue(smartLamp1.Equals(smartLamp2));
    }
}