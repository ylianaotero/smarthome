using System.ComponentModel.DataAnnotations;

namespace Domain.Concrete;

public class SmartLamp
{
    [Key]
    public long Id { get; set; }
    
    public SmartLamp()
    {
    }
}