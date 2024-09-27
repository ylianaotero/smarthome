using Domain;

namespace WebApi.Models.In;

public class DeviceRequest
{
    public string? Name { get; set; }
    public long? Model { get; set; }
    public string? Company { get; set; }
    public string? Kind { get; set; }
    
    

    public Device ToEntity()
    {
        return new Device()
        {
            Name = this.Name,
            Model = this.Model,
            Company = this.Company,
            Kind = this.Kind
        };
    }
    
    public Func<Device, bool> ToFilter() 
    {
        return device => (string.IsNullOrEmpty(this.Name) || device.Name == this.Name) &&
                           (this.Model == 0 || device.Model == this.Model) &&
                           (string.IsNullOrEmpty(this.Company) || 
                            (device.Company != null && device.Company.Name == this.Company)) &&
                           (string.IsNullOrEmpty(this.Kind) || device.Kind == this.Kind);
    }
}