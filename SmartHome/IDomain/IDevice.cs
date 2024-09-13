namespace IDomain;

public interface IDevice
{
    public string Name { get; set; }
    public long Model { get; set; }
    public string Description { get; set; }
    public List<string> PhotoURLs { get; set; }
    public ICompany Company { get; set; }
    public bool IsConnected { get; set; }
}