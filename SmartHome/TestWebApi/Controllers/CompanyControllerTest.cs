/*using Domain;
using IBusinessLogic;
using IDataAccess;
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
    
    private string Name = "Company 1";
    private string Name2 = "Company 2";
    private long Id = 1;
    private long Id2 = 2;
    private string RUT = "123456789";
    private int OkStatusCode = 200;
    private int CreatedStatusCode = 201;
    private string LogoURL = "https://www.logo.com";


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
            new Company { Id = Id, Name = Name },
            new Company { Id = Id2, Name = Name2}
        };
        _mockICompanyService
            .Setup(service => service
                .GetCompaniesByFilter(It.IsAny<Func<Company, bool>>(), It.IsAny<PageData>()))
            .Returns(companies);
        
        CompanyRequest request = new CompanyRequest();
      //  ObjectResult? result = _companyController.GetCompanies(request, DefaultPageDataRequest()) as ObjectResult;
        
     //   Assert.AreEqual(OkStatusCode, result?.StatusCode);
    }
    
    [TestMethod]
    public void TestGetCompaniesWithEmptyRequest()
    {
        List<Company> companies = [];
        _mockICompanyService
            .Setup(service => service
                .GetCompaniesByFilter(It.IsAny<Func<Company, bool>>(), It.IsAny<PageData>()))
            .Returns(companies);
        CompaniesResponse expectedResponse = DefaultCompaniesResponse();
        
        CompanyRequest request = new CompanyRequest();
      //  ObjectResult? result = _companyController.GetCompanies(request, DefaultPageDataRequest()) as ObjectResult;
        
     //   Assert.AreEqual(OkStatusCode, result?.StatusCode);
    }

    [TestMethod]
    public void TestGetAllCopmaniesOkResponse()
    {
        var companies = new List<Company>
        {
            new Company { Id = Id, Name = Name },
            new Company { Id = Id2, Name = Name2 }
        };
        _mockICompanyService
            .Setup(service => service
                .GetCompaniesByFilter(It.IsAny<Func<Company, bool>>(), It.IsAny<PageData>()))
            .Returns(companies);
        
        CompanyRequest request = new CompanyRequest();
     //   ObjectResult? result = _companyController.GetCompanies(request, DefaultPageDataRequest()) as ObjectResult;
        
     //   Assert.AreEqual(OkStatusCode, result?.StatusCode);
    }
    
    [TestMethod]
    public void TestPostCompanyOkStatusCode()
    {
        CompanyRequest request = new CompanyRequest()
        {
            Name = Name,
            RUT = RUT,
            LogoURL = LogoURL
        };
        
        _mockICompanyService.Setup(service => service.CreateCompany(It.IsAny<Company>()));
        
        ObjectResult? result = _companyController.PostCompany(request) as ObjectResult;
        
        Assert.AreEqual(CreatedStatusCode, result?.StatusCode);
    }
    
    private CompaniesResponse DefaultCompaniesResponse()
    {
        var companies = new List<Company>
        {
            new Company { Id = Id, Name = Name },
            new Company { Id = Id2, Name = Name2 }
        };
        return new CompaniesResponse(companies);
    }
    
    private PageDataRequest DefaultPageDataRequest()
    {
        PageDataRequest request = new PageDataRequest();
        
        request.Page = 1;
        request.PageSize = 10;
        
        return request;
    }
}*/