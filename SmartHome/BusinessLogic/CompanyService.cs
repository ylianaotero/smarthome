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

    private void CreateCompany(Company company)
    {
        companyRepository.Add(company);
    }

    public void AddOwnerToCompany(User owner, Company company)
    {
        company.Owner = owner; 
        CreateCompany(company); 
    }
}