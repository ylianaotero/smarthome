using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class DeviceService : IDeviceService
{
    private const string ModelValidatorError = "Model is not valid";
    private const string DeviceNotFoundMessage = "Device not found";
    private const string ValidationMethodNotFoundMessage = "Validation method not found";

    private const string ValidatorNumber = "ValidatorNumber";
    private const string ValidatorLetter = "ValidatorLetter";
    
    private readonly IRepository<Device> _deviceRepository;
    
    public DeviceService(IRepository<Device> deviceRepository)
    {
        this._deviceRepository = deviceRepository;
    }
    
    
    public void CreateDevice(Device device)
    {
        Company deviceCompany = device.Company;
        
        string? validationMethod = deviceCompany.ValidationMethod;

        if (validationMethod != null)
        {
            validationMethod = validationMethod.ToUpper();
        }
        
        Modelo deviceModel = new Modelo();
        deviceModel.set_Value(device.Model);
        
        if (ModelIsValid(deviceModel, validationMethod))
        {
            _deviceRepository.Add(device);
        }
        else
        {
            throw new InputNotValid(ModelValidatorError);
        }
    }

    public Device GetDeviceById(long id)
    {
        Device device = _deviceRepository.GetById(id);
        
        if (device == null)
        {
            throw new ElementNotFound(DeviceNotFoundMessage);
        }
        
        return device;
    }
    
    public List<Device> GetDevicesByFilter(Func<Device, bool> filter, PageData pageData)
    {
        return _deviceRepository.GetByFilter(filter, pageData);
    }
    
    public List<string> GetDeviceTypes()
    {
        List<string> deviceTypes = typeof(Device).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Device)) && !t.IsAbstract)
            .Select(t => t.Name)
            .ToList();

        return deviceTypes;
    }
    
    private bool ModelIsValid(Modelo deviceModel, string? validationMethod)
    {
        IModelValidator _modelValidator;
        bool isValid;
        
        if (validationMethod == "")
        {
            isValid = true;
        }
        else
        {
            if(validationMethod == "VALIDATORNUMBER")
            {
                _modelValidator = new ModelValidatorLogic()
                    .GetAllValidators().Find(v => v.GetType().Name == ValidatorNumber);
            }
            else if(validationMethod == "VALIDATORLETTER")
            {
                _modelValidator = new ModelValidatorLogic()
                    .GetAllValidators().Find(v => v.GetType().Name == ValidatorLetter);
            }
            else
            {
                throw new ElementNotFound(ValidationMethodNotFoundMessage);
            }
            isValid = _modelValidator.EsValido(deviceModel);
        }

        return isValid;
    }
}
