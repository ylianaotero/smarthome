using Domain;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DomainTest;

[TestClass]
public class CompanyTests
{
    private Company company;
    
    private const string CompanyName = "SecurityCameras & Co.";
    private const string CompanyRUT = "123456789";
    private const string CompanyLogoURL = "https://example.com/logo.jpg";
    
    [TestInitialize]
    public void TestInitialize()
    {
        company = new Company();
    }
    
    [TestMethod]
    public void TestAddNameToCompany()
    {
        company.Name = CompanyName;

        Assert.AreEqual(CompanyName, company.Name);
    }
    
    [TestMethod]
    public void TestAddRUTToCompany()
    {
        company.RUT = CompanyRUT;

        Assert.AreEqual(CompanyRUT, company.RUT);
    }
    
    [TestMethod]
    public void TestAddLogoURLToCompany()
    {
        company.LogoURL = CompanyLogoURL;

        Assert.AreEqual(CompanyLogoURL, company.LogoURL);
    }

    [TestMethod]
    public void TestDifferentiationOfCompaniesViaId()
    {
        Company company1 = new Company()
        {
            Id = 1,
            Name = CompanyName,
            RUT = CompanyRUT,
            LogoURL = CompanyLogoURL
        };
        
        Company company2 = new Company()
        {
            Id = 2,
            Name = CompanyName,
            RUT = CompanyRUT,
            LogoURL = CompanyLogoURL
        };
        
        Assert.AreNotEqual(company1, company2);
    }
}