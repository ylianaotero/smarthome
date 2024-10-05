using CustomExceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class CompanyService(IRepository<Company> companyRepository) : ICompanyService
{
    public List<Company> GetAllCompanies(PageData pageData)
    {
        return companyRepository.GetAll(pageData);
    }

    public List<Company> GetCompaniesByFilter(Func<Company, bool> filter, PageData pageData)
    {
        return companyRepository.GetByFilter(filter, pageData);
    }

    public void CreateCompany(Company company)
    {
        companyRepository.Add(company);
    }

    public Device AddCompanyToDevice(long companyId, Device device)
    {
        Company company = companyRepository.GetById(companyId);
        
        if (company == null)
        {
            throw new ElementNotFound("Company not found");
        }
        
        device.Company = company;
        companyRepository.Update(company);
        return device;
    }
}