using Domain.Concrete;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestDomain;

[TestClass]
public class CompanyTests
{
    private Company _company;
    
    private const string CompanyName = "SecurityCameras & Co.";
    private const string CompanyRut = "123456789";
    private const string CompanyLogoUrl = "https://example.com/logo.jpg";
    
    [TestInitialize]
    public void TestInitialize()
    {
        _company = new Company();
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
        Company company1 = new Company()
        {
            Id = 1,
            Name = CompanyName,
            RUT = CompanyRut,
            LogoURL = CompanyLogoUrl
        };
        
        Company company2 = new Company()
        {
            Id = 2,
            Name = CompanyName,
            RUT = CompanyRut,
            LogoURL = CompanyLogoUrl
        };
        
        Assert.AreNotEqual(company1, company2);
    }

    [TestMethod]
    public void TestCompaniesAreEqual()
    {
        Company company1 = new Company()
        {
            Id = 1,
            Name = CompanyName,
            RUT = CompanyRut,
            LogoURL = CompanyLogoUrl
        };
        
        Company company2 = new Company()
        {
            Id = 1,
            Name = CompanyName,
            RUT = CompanyRut,
            LogoURL = CompanyLogoUrl
        };
        
        Assert.AreEqual(company1, company2);
    }
    
    [TestMethod]
    public void TestAddOwnerToCompany()
    {
        User owner = new User();
        _company.Owner = owner;
        owner.AddRole(new CompanyOwner());
        
        Assert.AreEqual(owner, _company.Owner);
    }
}