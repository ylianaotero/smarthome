using Domain;

namespace WebApi.Models.In;

public class HomeDevicesRequest
{ 
    public List<DeviceRequest> Devices { get; set; }
    
    /*
    public List<Device> ToEntities()
    {
        return Devices.Select(device => new Device()
        {
            Name = device.Name,
            Model = device.Model,
            Company = new Company() { Name = device.Company },
            Kind = device.Kind
        }).ToList();
    }*/
}