using System.ComponentModel.DataAnnotations;
using CustomExceptions;
using Domain.Concrete;

namespace Domain.Abstract;

public abstract class Device
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public List<string> PhotoURLs { get; set; }
    public Company? Company { get; set; }
    public abstract string Kind { get; set; }

    public void ValidateStatus(string status)
    {
        if (Kind == "SmartLamp" && (status != "On" && status != "Off"))
        {
            throw new InputNotValid("SmartLamp status can only be on or off.");
        }
    }
}