using CustomExceptions;
using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class CompanyOwnerTest
{
    [TestMethod]
    public void TestAddCompanyToCompanyOwner()
    {
        Company company = new Company();
        CompanyOwner companyOwner = new CompanyOwner();
        companyOwner.Company = company; 
        Assert.AreEqual(company,companyOwner.Company);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void TestCannotAddCompanyToCompanyOwner()
    {
        Company company = new Company();
        CompanyOwner companyOwner = new CompanyOwner();
        companyOwner.Company = company; 
        Company company2 = new Company();
        companyOwner.Company = company2;
    }
    
    [TestMethod]
    public void TestAddIncompleteCompany()
    {
        CompanyOwner companyOwner = new CompanyOwner();
        Assert.IsFalse(companyOwner.HasACompleteCompany);
    }
    
    [TestMethod]
    public void TestValidateExistingCompany()
    {
        Company company = new Company();
        CompanyOwner companyOwner = new CompanyOwner();
        companyOwner.Company = company; 
        Assert.IsTrue(companyOwner.HasACompleteCompany);
    }
    
    [TestMethod]
    public void TestValidateNotExistentCompanyException()
    {
        CompanyOwner companyOwner = new CompanyOwner();
        Assert.IsFalse(companyOwner.HasACompleteCompany);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void TestValidateExistingCompanyException()
    {
        Company company = new Company();
        CompanyOwner companyOwner = new CompanyOwner();
        companyOwner.Company = company; 
        companyOwner.Company = new Company();
    }
    
}