
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomExceptions;
using Domain.Abstract;

namespace Domain.Concrete;

public class DeviceUnit
{
    [Key] 
    public Guid HardwareId { get; set; }
    public string Name { get; set; }
    public Device Device { get; set; }
    public bool IsConnected { get; set; }
    
    public string? Status
    {
        get => _status;
        set
        {
            Device.ValidateStatus(value);
            _status = value;
        }
    }
        
    [ForeignKey("RoomId")]
    public Room? Room
    {
        get => _room;
        set
        {
            _room = value;
        }
    }
    
    private string? _status { get; set; }
    private Room? _room { get; set; }
    
    public DeviceUnit()
    {
        IsConnected = false;
    }

    public void ExecuteAction(string relatedFunctionality)
    {
        Status = "Closed";
    }
    
    public override bool Equals(object? obj)
    {
        return obj is DeviceUnit unit &&
                HardwareId == unit.HardwareId;
    }
}