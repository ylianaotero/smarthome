using Domain;
using IDataAccess;

namespace IBusinessLogic;

public interface ICompanyService
{
    public List<Company> GetAllCompanies(PageData pageData);
    public List<Company> GetCompaniesByFilter(Func<Company, bool> filter, PageData pageData);
    public void CreateCompany(Company company);
}