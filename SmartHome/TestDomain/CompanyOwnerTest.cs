using CustomExceptions;
using Domain;

namespace TestDomain;

[TestClass]
public class CompanyOwnerTest
{
    [TestMethod]
    public void TestAddCompanyToCompanyOwner()
    {
        Company company = new Company();
        CompanyOwner companyOwner = new CompanyOwner(company);
        Assert.AreEqual(company,companyOwner.Company);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void TestCannotAddCompanyToCompanyOwner()
    {
        Company company = new Company();
        CompanyOwner companyOwner = new CompanyOwner(company);
        Company company2 = new Company();
        companyOwner.Company = company2;
    }
    
    [TestMethod]
    public void TestAddIncompleteCompany()
    {
        CompanyOwner companyOwner = new CompanyOwner();
        Assert.IsFalse(companyOwner.HasACompleteCompany);
    }
}