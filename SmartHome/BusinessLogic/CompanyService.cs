using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class CompanyService(IRepository<Company> companyRepository) : ICompanyService
{
    
}