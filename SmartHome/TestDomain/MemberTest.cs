using Domain;

namespace DomainTest;

[TestClass]
public class MemberTest
{
    [TestMethod]
    public void CreateNewHome()
    {
        Member member = new Member();

        member.Permission = true; 
        
        Assert.IsTrue(member.Permission);
    }
    
}