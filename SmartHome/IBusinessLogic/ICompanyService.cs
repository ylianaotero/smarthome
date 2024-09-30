using Domain;

namespace IBusinessLogic;

public interface ICompanyService
{
    public List<Company> GetAllCompanies();
    public List<Company> GetCompaniesByFilter(Func<Company, bool> filter);
    public void CreateCompany(Company company);
}