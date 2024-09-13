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
}