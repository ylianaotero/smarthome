using Domain;
using Domain.Concrete;

namespace TestDomain;

[TestClass]
public class AdministratorTest
{
    private const long Id = 1345354616346;
    
    [TestMethod]
    public void TestAddAdministrator()
    {
        Administrator admin = new Administrator();
        admin.Id = Id; 
        Assert.AreEqual(Id,admin.Id);
    }
}