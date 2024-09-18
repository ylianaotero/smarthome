using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Session
{
    [Key]
    public Guid Id { get; set; } 
    public virtual User User { get; set; }
}