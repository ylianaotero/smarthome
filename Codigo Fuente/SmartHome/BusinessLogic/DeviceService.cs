using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class DeviceService(IRepository<Device> deviceRepository) : IDeviceService
{
    private const string ModelValidatorError = "Model is not valid";
    private const string DeviceNotFoundMessage = "Device not found";
    
    private IModelValidator _modelValidator;
    private Modelo _modelo;
    
    public void CreateDevice(Device device)
    {
        Company deviceCompany = device.Company;
        //bool validateNumber = deviceCompany.ValidateNumber;
        //Prueba
        bool validateNumber = false;
        Modelo deviceModel = new Modelo();
        deviceModel.set_Value(device.Model);
        
        if (ModelIsValid(deviceModel, validateNumber))
        {
            deviceRepository.Add(device);
        }
        else
        {
            throw new InputNotValid(ModelValidatorError);
        }
    }

    private bool ModelIsValid(Modelo deviceModel, bool validateNumber)
    {
        bool IsValid = false;
        if (_modelValidator == null)
        {
            string modelValidatorName = validateNumber ? "ValidatorNumber" : "ValidatorLetter";
            _modelValidator = new ModelValidatorLogic().GetAllValidators().Find(v => v.GetType().Name == modelValidatorName);
            IsValid = _modelValidator.EsValido(deviceModel);
        }

        return IsValid;
    }

    public Device GetDeviceById(long id)
    {
        Device device = deviceRepository.GetById(id);
        
        if (device == null)
        {
            throw new ElementNotFound(DeviceNotFoundMessage);
        }
        
        return device;
    }
    
    public List<Device> GetDevicesByFilter(Func<Device, bool> filter, PageData pageData)
    {
        return deviceRepository.GetByFilter(filter, pageData);
    }
    
    public List<string> GetDeviceTypes()
    {
        List<string> deviceTypes = typeof(Device).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Device)) && !t.IsAbstract)
            .Select(t => t.Name)
            .ToList();

        return deviceTypes;
    }
}