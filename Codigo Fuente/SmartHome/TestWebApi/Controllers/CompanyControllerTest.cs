using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
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
    private Mock<IUserService> _mockIUserService;
    private CompanyController _companyController;
    private CompanyOwner _companyOwner;
    private Company _defaultCompany1;
    private Company _defaultCompany2;
    private User _defaultUser;
    private List<Company> _companies;
    
    private string Name = "Company 1";
    private string Name2 = "Company 2";
    private long Id = 1;
    private long Id2 = 2;
    private string RUT = "123456789";
    private int OkStatusCode = 200;
    private int CreatedStatusCode = 201;
    private int NotFoundStatusCode = 404;
    private string LogoURL = "https://www.logo.com";
    //private bool ValidateNumberTrue = true;
    private string ValidationMethodNumber = "ValidatorNumber";
    
    private const string UserName =  "John";
    private const string UserSurname = "Doe";
    private const string Email1 = "john.doe@example.com";
    private const string UserDoesNotExistExceptionMessage = "Member not found";

    [TestInitialize]
    public void Initialize()
    {
        SetupCompanyController();
        SetupDefaultObjects();
    }

    [TestMethod]
    public void TestGetCompaniesByFilterOkStatusCode()
    {
        _mockICompanyService
            .Setup(service => service
                .GetCompaniesByFilter(It.IsAny<Func<Company, bool>>(), It.IsAny<PageData>()))
            .Returns(_companies);

        GetCompaniesRequest request = new GetCompaniesRequest()
        {
            Name = Name,
            Owner = _defaultUser.Name + " " + _defaultUser.Surname,
            OwnerEmail = _defaultUser.Email
        };
        ObjectResult? result = _companyController.GetCompanies(request, DefaultPageDataRequest()) as ObjectResult;
        
        Assert.AreEqual(OkStatusCode, result?.StatusCode);
    }
    
    [TestMethod]
    public void TestGetCompaniesByFilterOkResponse()
    {
        _mockICompanyService
            .Setup(service => service
                .GetCompaniesByFilter(It.IsAny<Func<Company, bool>>(), It.IsAny<PageData>()))
            .Returns(_companies);

        GetCompaniesRequest request = new GetCompaniesRequest()
        {
            Name = Name,
            Owner = _defaultUser.Name + " " + _defaultUser.Surname
        };
        
        GetCompaniesResponse expectedResponse = DefaultCompaniesResponse();
        
        ObjectResult? result = _companyController.GetCompanies(request, DefaultPageDataRequest()) as ObjectResult;
        GetCompaniesResponse response = result?.Value as GetCompaniesResponse;
        
        Assert.AreEqual(expectedResponse, response);
    }
    
    [TestMethod]
    public void TestGetAllCompaniesOkStatusCode()
    {
        _mockICompanyService
            .Setup(service => service
                .GetCompaniesByFilter(It.IsAny<Func<Company, bool>>(), It.IsAny<PageData>()))
            .Returns(_companies);
        
        GetCompaniesRequest request = new GetCompaniesRequest();
        ObjectResult? result = _companyController.GetCompanies(request, DefaultPageDataRequest()) as ObjectResult;
        
        Assert.AreEqual(OkStatusCode, result?.StatusCode);
    }
    
    [TestMethod]
    public void TestPostCompanyOkStatusCode()
    {
        PostCompanyRequest request = new PostCompanyRequest()
        {
            Name = Name,
            RUT = RUT,
            LogoURL = LogoURL,
            OwnerId = _defaultUser.Id,
            ValidationMethod = ValidationMethodNumber
        };

        _mockIUserService
            .Setup(service => service.AddOwnerToCompany(_defaultUser.Id,request.ToEntity()))
            .Returns(_defaultCompany1); 
        _mockICompanyService.Setup(service => service.CreateCompany(It.IsAny<Company>()));
        
        ObjectResult? result = _companyController.PostCompany(request) as ObjectResult;
        
        Assert.AreEqual(CreatedStatusCode, result?.StatusCode);
    }
    
    [TestMethod]
    public void TestPostCompanyNotFoundStatusCode()
    {
        PostCompanyRequest request = new PostCompanyRequest()
        {
            Name = Name,
            RUT = RUT,
            LogoURL = LogoURL,
            OwnerId = _defaultUser.Id,
            ValidationMethod = ValidationMethodNumber
        };

        _mockIUserService.
            Setup(service => service.AddOwnerToCompany(_defaultUser.Id, request.ToEntity()))
            .Throws(new ElementNotFound(UserDoesNotExistExceptionMessage)); 
        _mockICompanyService.Setup(service => service.CreateCompany(It.IsAny<Company>()));
        
        ObjectResult? result = _companyController.PostCompany(request) as ObjectResult;
        
        Assert.AreEqual(NotFoundStatusCode, result?.StatusCode);
    }

    [TestMethod]
    public void TestUpdateCompanyValidationMethod()
    {
        PatchCompanyRequest request = new PatchCompanyRequest()
        {
            ValidationMethod = ValidationMethodNumber
        };
        
        _mockICompanyService.Setup(service => service.UpdateValidationMethod(Id, ValidationMethodNumber));
        
        ObjectResult? result = _companyController.UpdateValidationMethod(Id, request) as ObjectResult;
        
        Assert.AreEqual(OkStatusCode, result?.StatusCode);
    }
    
    [TestMethod]
    public void TestUpdateCompanyValidationMethodNotFound()
    {
        PatchCompanyRequest request = new PatchCompanyRequest()
        {
            ValidationMethod = ValidationMethodNumber
        };
        
        _mockICompanyService.Setup(service => service.UpdateValidationMethod(Id, ValidationMethodNumber))
            .Throws(new ElementNotFound("Company not found"));
        
        ObjectResult? result = _companyController.UpdateValidationMethod(Id, request) as ObjectResult;
        
        Assert.AreEqual(NotFoundStatusCode, result?.StatusCode);
    }
    
    private void SetupCompanyController()
    {
        _mockIUserService = new Mock<IUserService>(MockBehavior.Strict);
        _mockICompanyService = new Mock<ICompanyService>(MockBehavior.Strict);
        _companyController = new CompanyController(_mockICompanyService.Object, _mockIUserService.Object);
    }

    private void SetupDefaultObjects()
    {
        SetupDefaultUser();
        SetupDefaultCompanies();
    }

    private void SetupDefaultUser()
    {
        _companyOwner = new CompanyOwner(); 
        _defaultUser = new User()
        {
            Id = 1,
            Email = Email1,
            Name = UserName,
            Surname = UserSurname,
            Roles = new List<Role>() { _companyOwner }
        };
    }

    private void SetupDefaultCompanies()
    {
        _defaultCompany1 = new Company()
        {
            Id = Id,
            Name = Name,
            RUT = RUT,
            LogoURL = LogoURL,
            Owner = _defaultUser,
            ValidationMethod = ValidationMethodNumber
        };
        
        _defaultCompany2 = new Company()
        {
            Id = Id2,
            Name = Name2,
            RUT = RUT,
            LogoURL = LogoURL,
            Owner = _defaultUser,
            ValidationMethod = ValidationMethodNumber
        };
        
        _companies = new List<Company>
        {
            _defaultCompany1,
            _defaultCompany2
        };
    }
    
    private PageDataRequest DefaultPageDataRequest()
    {
        PageDataRequest request = new PageDataRequest();
        
        request.Page = 1;
        request.PageSize = 10;
        
        return request;
    }
    
    private GetCompaniesResponse DefaultCompaniesResponse()
    {
        List<Company> companies = new List<Company>
        {
            _defaultCompany1,
            _defaultCompany2
        };
        
        return new GetCompaniesResponse(companies, 2);
    }
}