using Domain;
using WebApi.In;

namespace TestWebApi.In;

[TestClass]
public class CreateUserTest
{
    private readonly string nameSample = "nameSample";
    private readonly string surnameSample = "surnameSample";
    private readonly string emailSample = "email@sample.com";
    private readonly string passwordSample = "passwordSample1@";
    private CreateUserRequest request;
    
    [TestInitialize]
    public void Init()
    {
        request = new CreateUserRequest()
        {
            Name = nameSample,
            Surname = surnameSample,
            Email = emailSample,
            Password = passwordSample,
        };
    }
    
    [TestMethod]
    public void AssignsProperties()
    {
        Assert.AreEqual(request.Name, nameSample);
        Assert.AreEqual(request.Surname, surnameSample);
        Assert.AreEqual(request.Email, emailSample);
        Assert.AreEqual(request.Password, passwordSample);
    }
    
    [TestMethod]
    public void UserRequestToUserEntity()
    {
        User entity = request.ToEntity();

        Assert.AreEqual(entity.Name, nameSample);
        Assert.AreEqual(entity.Surname, surnameSample);
        Assert.AreEqual(entity.Email, emailSample);
        Assert.AreEqual(entity.Password, passwordSample);
    }
}