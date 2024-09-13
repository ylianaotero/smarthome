using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestDomain;

[TestClass]
public class CompanyTests
{
    [TestMethod]
    public void TestAddNameToCompany()
    {
        Company company = new Company();
        company.Name = "SecurityCameras & Co.";

        Assert.AreEqual("SecurityCameras & Co.", company.Name);
    }
    
    [TestMethod]
    public void TestAddRUTToCompany()
    {
        Company company = new Company();
        company.RUT = "123456789";

        Assert.AreEqual("123456789", company.RUT);
    }
    
    [TestMethod]
    public void TestAddLogoURLToCompany()
    {
        Company company = new Company();
        company.LogoURL = "https://example.com/logo.jpg";

        Assert.AreEqual("https://example.com/logo.jpg", company.LogoURL);
    }

    [TestMethod]
    public void TestDiferentiateCompaniesViaId()
    {
        Company company1 = new Company()
        {
            Id = 1,
            Name = "Company 1",
            RUT = "123456789",
            LogoURL = "https://example.com/logo1.jpg"
        };
        
        Company company2 = new Company()
        {
            Id = 2,
            Name = "Company 1",
            RUT = "123456789",
            LogoURL = "https://example.com/logo1.jpg"
        };
        
        Assert.AreNotEqual(company1, company2);
        
    }
}