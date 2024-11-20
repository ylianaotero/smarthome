using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using Moq;
using WebApi.Controllers;

namespace TestWebApi.Controllers;

[TestClass]
public class CompanyOwnerTest
{
    private const string Name =  "John";
    private const string Surname = "Doe";
    private const string Email1 = "john.doe@example.com";
    private const string Password = "Securepassword1@";
    
    private const string ErrorMessageWhenInputIsInvalid = "Input not valid, try again";
    private const string ErrorMessageWhenElementAlreadyExists =  "Element already exists, try again";
    
    private const int ConflictStatusCode = 409;
    private const int BadRequestStatusCode = 400;
    
    private Mock<IUserService> _userServiceMock;
    private CompanyOwnerController _companyOwnerController;
    
    
    [TestInitialize]
    public void TestInitialize()
    {
        _userServiceMock = new Mock<IUserService>();
    }
    
    [TestMethod]
    public void CreateCompanyOwnerValidRequest()
    {
        PostCompanyOwnerRequest postCompanyOwnerRequest = new PostCompanyOwnerRequest()
        {
            Name = Name,
            Surname = Surname,
            Email = Email1, 
            Password = Password
        };
        _userServiceMock.Setup(service => service.CreateUser(It.Is<User>(u =>
            u.Name == Name &&
            u.Email == Email1 &&
            u.Password == Password &&
            u.Surname == Surname
        )));
        _companyOwnerController = new CompanyOwnerController(_userServiceMock.Object);
        PostCompanyOwnerResponse expectedResponse = new PostCompanyOwnerResponse(new User()
        {
            Name = Name,
            Surname = Surname,
            Email = Email1,
            Password = Password,
            Roles = new List<Role>(){new CompanyOwner()}
        });

        ObjectResult result = _companyOwnerController.CreateCompanyOwner(postCompanyOwnerRequest) as ObjectResult;
        PostCompanyOwnerResponse userResponse = result?.Value as PostCompanyOwnerResponse;

        _userServiceMock.VerifyAll();
        
        Assert.AreEqual(expectedResponse, userResponse);
    }
    
    [TestMethod]
    public void CreateUserServiceThrowsUserAlreadyExists()
    {
        PostCompanyOwnerRequest postCompanyOwnerRequest = new PostCompanyOwnerRequest()
        {
            Name = Name,
            Surname = Surname,
            Email = Email1, 
            Password = Password
        };
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new ElementAlreadyExist(ErrorMessageWhenElementAlreadyExists));
        _companyOwnerController = new CompanyOwnerController(_userServiceMock.Object);
        
        ObjectResult result = _companyOwnerController.CreateCompanyOwner(postCompanyOwnerRequest) as ObjectResult;
        
        _userServiceMock.VerifyAll();
        
        Assert.AreEqual(ConflictStatusCode, result.StatusCode);
    }
    
    [TestMethod]
    public void CreateCompanyOwnerInvalidRequest()
    {
        PostCompanyOwnerRequest postCompanyOwnerRequest = new PostCompanyOwnerRequest()
        {
            Name = Name,
            Surname = Surname,
            Email = Email1, 
            Password = Password
        };
        
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<User>()))
            .Throws(new InputNotValid(ErrorMessageWhenInputIsInvalid));
        _companyOwnerController = new CompanyOwnerController(_userServiceMock.Object);
        
        ObjectResult result = _companyOwnerController.CreateCompanyOwner(postCompanyOwnerRequest) as ObjectResult;
        
        _userServiceMock.VerifyAll();
        
        Assert.AreEqual(BadRequestStatusCode, result.StatusCode);
    }
}