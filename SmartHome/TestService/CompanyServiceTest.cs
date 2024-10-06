using BusinessLogic;
using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IDataAccess;
using Moq;

namespace TestService;

[TestClass]
public class CompanyServiceTest
{
    private Mock<IRepository<Company>> _mockCompanyRepository;
    private CompanyService _companyService;
    private Company _company;
    private User _user; 
    private const string NewEmail = "juan.perez@example.com";
    
    private const string CompanyName = "IoT Devices & Co.";
    private const string RUT = "123456789";
    private const string LogoUrl = "https://example.com/logo.jpg";
    private const long CompanyId = 1;

    [TestInitialize]
    public void TestInitialize()
    {
        SetupDefaultObjects();
        CreateMockCompanyRepository();
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
        _mockCompanyRepository.Setup(x => x.Add(_company));
        _companyService.CreateCompany(_company);
        _mockCompanyRepository.Verify(x => x.Add(_company), Times.Once);
    }
    
    [TestMethod]
    public void TestAddCompanyToDevice()
    {
        Device device = new SecurityCamera()
        {
            Name = "Device 1",
        };
        
        _mockCompanyRepository.Setup(x => x.GetById(CompanyId)).Returns(_company);

        _companyService = new CompanyService(_mockCompanyRepository.Object); 
        _companyService.AddCompanyToDevice(CompanyId, device);
        
        _mockCompanyRepository.Verify(x => x.Update(_company), Times.Once);
        Assert.AreEqual(device.Company, _company);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestAddNonExistentCompanyToDevice()
    {
        Device device = new SecurityCamera()
        {
            Name = "Device 1",
        };
        
        _mockCompanyRepository.Setup(x => x.GetById(CompanyId)).Returns((Company?)null);
        _companyService.AddCompanyToDevice(CompanyId, device);
    }
    
    private void SetupDefaultObjects()
    {
        _user = new User() { Email = NewEmail }; 
        _company = new Company()
        {
            Name = CompanyName,
            RUT = RUT,
            LogoURL = LogoUrl,
            Id = CompanyId
        };
    }
    
    private void CreateMockCompanyRepository()
    {
        _mockCompanyRepository = new Mock<IRepository<Company>>();
        _companyService = new CompanyService(_mockCompanyRepository.Object);
    }
}