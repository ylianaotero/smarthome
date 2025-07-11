using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class CompanyService(IRepository<Company> companyRepository) : ICompanyService
{
    private const string CompanyNotFound = "Company not found";
    
    public void CreateCompany(Company company)
    {
        companyRepository.Add(company);
    }

    public List<Company> GetCompaniesByFilter(Func<Company, bool> filter, PageData pageData)
    {
        return companyRepository.GetByFilter(filter, pageData);
    }

    public Device AddCompanyToDevice(long companyId, Device device)
    {
        Company company = companyRepository.GetById(companyId);
        
        if (company == null)
        {
            throw new ElementNotFound(CompanyNotFound);
        }
        
        device.Company = company;
        companyRepository.Update(company);
        
        return device;
    }
    
    public List<Company> GetCompaniesOfOwners(long userId)
    {
        return companyRepository.GetByFilter(s => s.Owner.Id == userId, null);
    }
    
    public void UpdateValidationMethod(long companyId, string validationMethod)
    {
        Company company = companyRepository.GetById(companyId);
        
        if (company == null)
        {
            throw new ElementNotFound(CompanyNotFound);
        }
        
        company.ValidationMethod = validationMethod;
        companyRepository.Update(company);
    }
}