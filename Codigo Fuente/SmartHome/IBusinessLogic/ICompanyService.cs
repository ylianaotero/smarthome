using Domain.Abstract;
using Domain.Concrete;
using IDataAccess;

namespace IBusinessLogic;

public interface ICompanyService
{
    public void CreateCompany(Company company);
    public List<Company> GetCompaniesByFilter(Func<Company, bool> filter, PageData pageData);
    Device AddCompanyToDevice(long companyId, Device device);

    List<Company> GetCompaniesOwners(long userId); 
}