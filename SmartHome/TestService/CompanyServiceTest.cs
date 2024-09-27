using BusinessLogic;
using Domain;
using IBusinessLogic;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class CompanyServiceTest
{
    private Mock<IRepository<Company>> _mockCompanyRepository;
    private CompanyService _companyService;
    private Company _company;

    [TestInitialize]
    public void TestInitialize()
    {
        CreateMockCompanyRepository();
    }

    private void CreateMockCompanyRepository()
    {
        _mockCompanyRepository = new Mock<IRepository<Company>>();
        _companyService = new CompanyService(_mockCompanyRepository.Object);
    }
    
    [TestMethod]
    public void TestGetAllCompanies()
    {
        List<Company> companies = new List<Company>();
        companies.Add(_company);
        _mockCompanyRepository.Setup(x => x.GetAll()).Returns(companies);
        
        List<Company> retrievedCompanies =  _companyService.GetAllCompanies();
        
        Assert.AreEqual(companies, retrievedCompanies);
    }
}