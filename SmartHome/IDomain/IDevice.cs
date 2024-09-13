using System.ComponentModel.DataAnnotations;

namespace IDomain;

public interface IDevice
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public long Model { get; set; }
    public string Description { get; set; }
    public List<string> PhotoURLs { get; set; }
    public ICompany Company { get; set; }
    public bool IsConnected { get; set; }
}