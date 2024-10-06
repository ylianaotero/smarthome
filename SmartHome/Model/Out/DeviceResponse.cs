using Domain.Abstract;

namespace Model.Out;

public class DeviceResponse
{
    public string Name { get; set;  }
    public long Model { get; set;  }
    public string? PhotoUrl { get; set;  }
    public string? CompanyName { get; set;  }
    
    public DeviceResponse (Device device)
    {
        Name = device.Name;
        Model = device.Model;
        PhotoUrl = device.PhotoURLs.FirstOrDefault() ?? string.Empty;
        CompanyName = device.Company?.Name ?? string.Empty;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is DeviceResponse response &&
               Name == response.Name &&
               Model == response.Model &&
               PhotoUrl == response.PhotoUrl &&
               CompanyName == response.CompanyName;
    }
}