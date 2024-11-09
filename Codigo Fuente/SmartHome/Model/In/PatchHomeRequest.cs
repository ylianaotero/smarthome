using Domain.Concrete;

namespace Model.In;

public class PatchHomeRequest
{
    public long HomeId { get; set; }
    public string Alias { get; set; }
    
    public Home ToEntity()
    {
        Home home = new Home()
        {
            Id = this.HomeId,
            Alias = this.Alias
        };

        return home;
    }
}