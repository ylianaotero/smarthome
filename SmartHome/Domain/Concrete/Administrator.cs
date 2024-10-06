using Domain.Abstract;

namespace Domain.Concrete;

public class Administrator : Role
{
    public override string Kind { get; set; }
    
    public Administrator()
    {
        Kind = GetType().Name;
    }
}