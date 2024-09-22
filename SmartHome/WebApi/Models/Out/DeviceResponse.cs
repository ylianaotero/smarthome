namespace WebApi.Out;

public class DeviceResponse
{
    public string Name { get; set;  }
    public long Model { get; set;  }
    public string PhotoUrl { get; set;  }
    public string CompanyName { get; set;  }

    public override bool Equals(object? obj)
    {
        return obj is DeviceResponse response &&
               Name == response.Name &&
               Model == response.Model &&
               PhotoUrl == response.PhotoUrl &&
               CompanyName == response.CompanyName;
    }
}