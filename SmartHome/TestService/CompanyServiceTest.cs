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
    
    private const string CompanyName = "IoT Devices & Co.";
    private const string RUT = "123456789";
    private const string LogoUrl = "https://example.com/logo.jpg";

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
        _mockCompanyRepository.Setup(x => x.GetAll(It.IsAny<PageData>())).Returns(companies);
        
        List<Company> retrievedCompanies =  _companyService.GetAllCompanies(PageData.Default);
        
        Assert.AreEqual(companies, retrievedCompanies);
    }
    
    [TestMethod]
    public void TestGetCompaniesWithFilter()
    {
        List<Company> companies = new List<Company>();
        companies.Add(_company);
        Func<Company, bool> filter = c => c.Name == CompanyName;
        
        _mockCompanyRepository
            .Setup(x => x.GetByFilter(filter, It.IsAny<PageData>()))
            .Returns(companies);
        
        List<Company> retrievedCompanies =  _companyService.GetCompaniesByFilter(filter, PageData.Default);
        
        Assert.AreEqual(companies, retrievedCompanies);
    }

    [TestMethod]
    public void TestCreateCompany()
    {
        _company = new Company()
        {
            Name = CompanyName,
            RUT = RUT,
            LogoURL = LogoUrl

        };
        _mockCompanyRepository.Setup(x => x.Add(_company));
        _companyService.CreateCompany(_company);
        _mockCompanyRepository.Verify(x => x.Add(_company), Times.Once);
    }
}