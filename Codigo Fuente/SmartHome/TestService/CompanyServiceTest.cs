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
    private List<Company> _companies;
    private Company _company;
    private Device _device;

    private const string CompanyName = "IoT Devices & Co.";
    private const string RUT = "123456789";
    private const string LogoUrl = "https://example.com/logo.jpg";
    private const long CompanyId = 1;

    private const string DeviceName = "Device 1";

    [TestInitialize]
    public void TestInitialize()
    {
        SetupDefaultObjects();
        CreateMockCompanyRepository();
    }

    [TestMethod]
    public void TestGetCompaniesWithFilter()
    {
        _companies.Add(_company);
        Func<Company, bool> filter = c => c.Name == CompanyName;

        _mockCompanyRepository
            .Setup(x => x.GetByFilter(filter, It.IsAny<PageData>()))
            .Returns(_companies);

        List<Company> retrievedCompanies = _companyService.GetCompaniesByFilter(filter, PageData.Default);

        Assert.AreEqual(_companies, retrievedCompanies);
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
        _mockCompanyRepository.Setup(x => x.GetById(CompanyId)).Returns(_company);

        _companyService = new CompanyService(_mockCompanyRepository.Object);
        _companyService.AddCompanyToDevice(CompanyId, _device);

        _mockCompanyRepository.Verify(x => x.Update(_company), Times.Once);
        Assert.AreEqual(_device.Company, _company);
    }

    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestAddNonExistentCompanyToDevice()
    {
        _mockCompanyRepository.Setup(x => x.GetById(CompanyId)).Returns((Company?)null);
        _companyService.AddCompanyToDevice(CompanyId, _device);
    }

    [TestMethod]
    public void TestGetCompaniesOfOwners()
    {
        _companies.Add(_company);

        _mockCompanyRepository.Setup(x => x.GetByFilter(It.IsAny<Func<Company, bool>>(), null)).Returns(_companies);

        _companyService = new CompanyService(_mockCompanyRepository.Object);

        List<Company> list = _companyService.GetCompaniesOfOwners(1);

        _mockCompanyRepository.VerifyAll();

        Assert.AreEqual(_companies, list);
    }

    [TestMethod]
    public void TestGetCompaniesOfOwners_returnNull()
    {
        _mockCompanyRepository.Setup(x => x.GetByFilter(It.IsAny<Func<Company, bool>>(), null)).Returns(new List<Company>());

        _companyService = new CompanyService(_mockCompanyRepository.Object);

        List<Company> list = _companyService.GetCompaniesOfOwners(1);

        _mockCompanyRepository.VerifyAll();

        Assert.AreEqual(0, list.Count);
    }
    
    [TestMethod]
    public void TestUpdateValidationMethod()
    {
        const string validationMethod = "SHA-256";
        _mockCompanyRepository.Setup(x => x.GetById(CompanyId)).Returns(_company);

        _companyService.UpdateValidationMethod(CompanyId, validationMethod);

        _mockCompanyRepository.Verify(x => x.Update(_company), Times.Once);
        Assert.AreEqual(validationMethod, _company.ValidationMethod);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementNotFound))]
    public void TestUpdateValidationMethodThrowsException()
    {
        const string validationMethod = "SHA-256";
        _mockCompanyRepository.Setup(x => x.GetById(CompanyId)).Returns((Company?)null);

        _companyService.UpdateValidationMethod(CompanyId, validationMethod);
    }

    private void SetupDefaultObjects()
    {
        _companies = new List<Company>();
        _device = new SecurityCamera()
        {
            Name = DeviceName,
        };

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
