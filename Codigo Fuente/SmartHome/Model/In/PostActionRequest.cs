namespace Model.In;

public class PostActionRequest
{
    public long HomeId { get; set; }
    public Guid HardwareId { get; set; }
    public string Functionality { get; set; }
}