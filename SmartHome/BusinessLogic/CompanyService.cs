using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class CompanyService(IRepository<Company> companyRepository) : ICompanyService
{
    public List<Company> GetAllCompanies()
    {
        return companyRepository.GetAll();
    }

    public List<Company> GetCompaniesByFilter(Func<Company, bool> filter)
    {
        return companyRepository.GetByFilter(filter);
    }
}