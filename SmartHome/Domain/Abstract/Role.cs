using System.ComponentModel.DataAnnotations;

namespace Domain.Abstract;

public abstract class Role
{
    [Key]
    public long Id { get; set; }
    public abstract string Kind { get; set; }
}