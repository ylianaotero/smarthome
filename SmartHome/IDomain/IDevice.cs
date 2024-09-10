namespace IDomain;

public interface IDevice
{
    public string Name { get; set; }
    public bool IsConnected { get; set; }
}