using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.In;
using WebApi.Models.Out;


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
    
    private CompaniesResponse DefaultCompaniesResponse()
    {
        var companies = new List<Company>
        {
            new Company { Id = 1, Name = "Company 1" },
            new Company { Id = 2, Name = "Company 2" }
        };
        return new CompaniesResponse(companies);
    }
    
    [TestMethod]
    public void TestPostCompany()
    {
        CompanyRequest request = new CompanyRequest
        {
            Name = "Company 1",
            RUT = "123456789",
            LogoURL = "https://www.google.com"
        };
        Company company = new Company
        {
            Name = request.Name,
            RUT = request.RUT,
            LogoURL = request.LogoURL
        };
        _mockICompanyService.Setup(service => service.CreateCompany(company)).Returns(company);
        
        ObjectResult? result = _companyController.PostCompany(request) as ObjectResult;
        
        Assert.AreEqual(201, result?.StatusCode);
    }
}