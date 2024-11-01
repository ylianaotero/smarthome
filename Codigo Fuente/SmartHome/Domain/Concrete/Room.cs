using System.ComponentModel.DataAnnotations;

namespace Domain.Concrete;

public class Room
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
}