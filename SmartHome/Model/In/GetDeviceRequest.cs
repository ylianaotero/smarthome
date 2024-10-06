using Domain.Abstract;

namespace Model.In;

public class GetDeviceRequest
{
    public string? Name { get; set; }
    public long? Model { get; set; }
    public string? Company { get; set; }
    public string? Kind { get; set; }
    
    public Func<Device, bool> ToFilter() 
    {
        return device => (string.IsNullOrEmpty(this.Name) || device.Name == this.Name) &&
                           (this.Model == null || device.Model == this.Model) &&
                           (string.IsNullOrEmpty(this.Company) || 
                            (device.Company != null && device.Company.Name == this.Company)) &&
                           (string.IsNullOrEmpty(this.Kind) || device.Kind == this.Kind);
    }
}