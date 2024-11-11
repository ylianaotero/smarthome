namespace IBusinessLogic;

public interface IActionService
{
    string PostAction(long homeId, Guid hardwareId, string functionality);
}