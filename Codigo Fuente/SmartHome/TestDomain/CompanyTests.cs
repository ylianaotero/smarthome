using Domain.Concrete;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestDomain;

[TestClass]
public class CompanyTests
{
    private User _owner;
    private Company _company;
    private Company _company1;
    private Company _company2;
    
    private const string CompanyName = "SecurityCameras & Co.";
    private const string CompanyRut = "123456789";
    private const string CompanyLogoUrl = "https://example.com/logo.jpg";
    
    [TestInitialize]
    public void TestInitialize()
    {
        _owner = new User();
        _company = new Company();
        
        _company1 = new Company()
        {
            Id = 1,
            Name = CompanyName,
            RUT = CompanyRut,
            LogoURL = CompanyLogoUrl
        };
        
        _company2 = new Company()
        {
            Id = 2,
            Name = CompanyName,
            RUT = CompanyRut,
            LogoURL = CompanyLogoUrl
        };
    }
    
    [TestMethod]
    public void TestAddNameToCompany()
    {
        _company.Name = CompanyName;

        Assert.AreEqual(CompanyName, _company.Name);
    }
    
    [TestMethod]
    public void TestAddRutToCompany()
    {
        _company.RUT = CompanyRut;

        Assert.AreEqual(CompanyRut, _company.RUT);
    }
    
    [TestMethod]
    public void TestAddLogoUrlToCompany()
    {
        _company.LogoURL = CompanyLogoUrl;

        Assert.AreEqual(CompanyLogoUrl, _company.LogoURL);
    }

    [TestMethod]
    public void TestDifferentiationOfCompaniesViaId()
    {
        Assert.AreNotEqual(_company1, _company2);
    }

    [TestMethod]
    public void TestCompaniesAreEqual()
    {
        _company1.Id = _company2.Id;
        Assert.AreEqual(_company1, _company2);
    }
    
    [TestMethod]
    public void TestAddOwnerToCompany()
    {
        _company.Owner = _owner;
        _owner.AddRole(new CompanyOwner());
        
        Assert.AreEqual(_owner, _company.Owner);
    }
    
    [TestMethod]
    public void TestAddValidationMethodToCompany()
    {
        _company.ValidationMethod = "ValidatorNumber";
        
        Assert.AreEqual("ValidatorNumber", _company.ValidationMethod);
    }
}