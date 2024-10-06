using System.ComponentModel.DataAnnotations;

namespace Domain.Concrete;

public class Session
{
    [Key]
    public Guid Id { get; set; } 
    public virtual User User { get; set; }
}