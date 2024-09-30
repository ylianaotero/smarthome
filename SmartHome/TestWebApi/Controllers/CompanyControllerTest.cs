using Domain;
using IBusinessLogic;
using Moq;
using WebApi.Controllers;


namespace TestWebApi.Controllers;


[TestClass]
public class CompanyControllerTest
{
    private Mock<ICompanyService> _companyServiceMock;
    private CompanyController _companyController;


    [TestInitialize]
    public void Initialize()
    {
        SetupCompanyController();
    }


    private void SetupCompanyController()
    {
        _companyServiceMock = new Mock<ICompanyService>();
        _companyController = new CompanyController(_companyServiceMock.Object);
    }


    [TestMethod]
    public void GetAllCompanies_ReturnsAllCompanies()
    {
        var companies = new List<Company>
        {
            new Company { Id = 1, Name = "Company 1" },
            new Company { Id = 2, Name = "Company 2" }
        };
        _companyServiceMock.Setup(x => x.GetAllCompanies()).Returns(companies);


        var result = _companyController.GetAllCompanies();
      
        Assert.AreEqual(companies, result);
    }
}