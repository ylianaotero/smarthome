
using System.ComponentModel.DataAnnotations;

namespace Domain;

public class DeviceUnit
{
    [Key] 
    public long Id { get; set; }
    public long HardwareId { get; set; }
    public Device Device { get; set; }
    public bool Connected { get; set; }
    
    public DeviceUnit()
    {
        Connected = false;
    }
}