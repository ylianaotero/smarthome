using Domain;


namespace Model.In;


public class UpdateDeviceConnectionStatusRequest
{
    public Guid DeviceUnitId { get; set; }
    public bool Status { get; set; }
  
 
    public DeviceUnit ToEntity()
    {
        return new DeviceUnit()
        {
            HardwareId = DeviceUnitId,
            IsConnected = Status
        };
    }
}