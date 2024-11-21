using Domain.Abstract;
using Domain.Concrete;
using Domain.Enum;

namespace Model.In;

public class Photo
{
    public string Path { get; set; }
    public bool EsPrincipal { get; set; }
}

public class DeviceImportModel
{
    public string Nombre { get; set; }
    public string Modelo { get; set; }
    public string Tipo { get; set; }
    public List<Photo> Fotos { get; set; }
    public bool PersonDetection { get; set; }
    public bool MovementDetection { get; set; }

    private const string CameraType = "camera";
    private const string SensorMovementType = "sensor-movement";
    private const string SmartLampType = "smart-lamp";
    
    private const string CameraTypeValue = "SecurityCamera";
    private const string SensorMovementTypeValue = "MotionSensor";
    private const string SmartLampTypeValue = "SmartLamp";

    private const string WindowSensor = "WindowSensor";

    public Device ToEntity(Company company)
    {
        switch (Tipo.ToLower())
        {
            case CameraType:
                return new SecurityCamera
                {
                    Name = Nombre,
                    Model = Modelo,
                    PhotoURLs = Fotos.Select(f => f.Path).ToList(),
                    Kind = CameraTypeValue,
                    Functionalities = GetCameraFunctionalities(),
                    Description = Nombre,
                    Company = company
                };

            case SensorMovementType:
                return new MotionSensor
                {
                    Name = Nombre,
                    Model = Modelo,
                    PhotoURLs = Fotos.Select(f => f.Path).ToList(),
                    Kind = SensorMovementTypeValue,
                    Description = Nombre,
                    Company = company
                };

            case SmartLampType:
                return new SmartLamp
                {
                    Name = Nombre,
                    Model = Modelo,
                    PhotoURLs = Fotos.Select(f => f.Path).ToList(),
                    Kind = SmartLampTypeValue,
                    Description = Nombre,
                    Company = company
                };

            default:
                return new WindowSensor
                {
                    Name = Nombre,
                    Model = Modelo,
                    PhotoURLs = Fotos.Select(f => f.Path).ToList(),
                    Kind = WindowSensor,
                    Description = Nombre,
                    Company = company
                };
        }
    }
    
    private List<SecurityCameraFunctionality> GetCameraFunctionalities()
    {
        List<SecurityCameraFunctionality> functionalities = new List<SecurityCameraFunctionality>();

        if (MovementDetection)
        {
            functionalities.Add(SecurityCameraFunctionality.MotionDetection);
        }

        if (PersonDetection)
        {
            functionalities.Add(SecurityCameraFunctionality.HumanDetection);
        }

        return functionalities;
    }
}
