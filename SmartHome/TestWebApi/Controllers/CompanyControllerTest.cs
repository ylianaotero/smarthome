using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using Moq;
using WebApi.Controllers;


namespace TestWebApi.Controllers;


[TestClass]
public class CompanyControllerTest
{
    private Mock<ICompanyService> _mockICompanyService;
    private CompanyController _companyController;


    [TestInitialize]
    public void Initialize()
    {
        SetupCompanyController();
    }

    private void SetupCompanyController()
    {
        _mockICompanyService = new Mock<ICompanyService>();
        _companyController = new CompanyController(_mockICompanyService.Object);
    }

    [TestMethod]
    public void TestGetCompanies()
    {
        var companies = new List<Company>
        {
            new Company { Id = 1, Name = "Company 1" },
            new Company { Id = 2, Name = "Company 2" }
        };
        _mockICompanyService.Setup(service => service.GetCompaniesByFilter(It.IsAny<Func<Company, bool>>())).Returns(companies);
        
        CompanyRequest request = new CompanyRequest();
        ObjectResult? result = _companyController.GetCompanies(request) as ObjectResult;
        
        Assert.AreEqual(200, result?.StatusCode);
    }
    
    [TestMethod]
    public void TestGetCompaniesWithEmptyRequest()
    {
        List<Company> companies = [];
        _mockICompanyService.Setup(service => service.GetCompaniesByFilter(It.IsAny<Func<Company, bool>>())).Returns(companies);
        CompaniesResponse expectedResponse = DefaultCompaniesResponse();
        
        CompanyRequest request = new CompanyRequest();
        ObjectResult? result = _companyController.GetCompanies(request) as ObjectResult;
        
        Assert.AreEqual(200, result?.StatusCode);
    }

    [TestMethod]
    public void TestGetAllCopmaniesOkResponse()
    {
        var companies = new List<Company>
        {
            new Company { Id = 1, Name = "Company 1" },
            new Company { Id = 2, Name = "Company 2" }
        };
        _mockICompanyService.Setup(service => service.GetCompaniesByFilter(It.IsAny<Func<Company, bool>>())).Returns(companies);
        
        CompanyRequest request = new CompanyRequest();
        ObjectResult? result = _companyController.GetCompanies(request) as ObjectResult;
        
        Assert.AreEqual(200, result?.StatusCode);
    }
    
    [TestMethod]
    public void TestPostCompanyOkStatusCode()
    {
        CompanyRequest request = new CompanyRequest()
        {
            Name = "Company 1",
            RUT = "123456789",
            LogoURL = "https://www.logo.com"
        };
        
        _mockICompanyService.Setup(service => service.CreateCompany(It.IsAny<Company>()));
        
        ObjectResult? result = _companyController.PostCompany(request) as ObjectResult;
        
        Assert.AreEqual(201, result?.StatusCode);
    }
    
    private CompaniesResponse DefaultCompaniesResponse()
    {
        var companies = new List<Company>
        {
            new Company { Id = 1, Name = "Company 1" },
            new Company { Id = 2, Name = "Company 2" }
        };
        return new CompaniesResponse(companies);
    }
}