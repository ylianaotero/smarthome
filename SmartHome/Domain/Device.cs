using System.ComponentModel.DataAnnotations;

namespace Domain;

public abstract class Device
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public long Model { get; set; }
    public string Description { get; set; }
    public List<string> PhotoURLs { get; set; }
    public Company? Company { get; set; }
    public abstract string Kind { get; set; }
}