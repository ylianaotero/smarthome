using CustomExceptions;
using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class CompanyOwnerTest
{
    Company _company;
    Company _company2;
    CompanyOwner _companyOwner;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _company = new Company();
        _company2 = new Company();
        _companyOwner = new CompanyOwner();
    }
    
    [TestMethod]
    public void TestAddCompanyToCompanyOwner()
    {
        _companyOwner.Company = _company; 
        Assert.AreEqual(_company,_companyOwner.Company);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void TestCannotAddCompanyToCompanyOwner()
    {
        _companyOwner.Company = _company; 
        _companyOwner.Company = _company2;
    }
    
    [TestMethod]
    public void TestAddIncompleteCompany()
    {
        Assert.IsFalse(_companyOwner.HasACompleteCompany);
    }
    
    [TestMethod]
    public void TestValidateExistingCompany()
    {
        _companyOwner.Company = _company; 
        Assert.IsTrue(_companyOwner.HasACompleteCompany);
    }
    
    [TestMethod]
    public void TestValidateNotExistentCompanyException()
    {
        Assert.IsFalse(_companyOwner.HasACompleteCompany);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ElementAlreadyExist))]
    public void TestValidateExistingCompanyException()
    {
        _companyOwner.Company = _company; 
        _companyOwner.Company = new Company();
    }
    
}