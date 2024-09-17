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
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    private CreateHomeOwnerRequest request;
    
    [TestInitialize]
    public void Init()
    {
        request = new CreateHomeOwnerRequest()
        {
            Name = nameSample,
            Surname = surnameSample,
            Email = emailSample,
            Password = passwordSample,
            Photo = ProfilePictureUrl
        };
    }
    
    [TestMethod]
    public void AssignsProperties()
    {
        Assert.AreEqual(request.Name, nameSample);
        Assert.AreEqual(request.Surname, surnameSample);
        Assert.AreEqual(request.Email, emailSample);
        Assert.AreEqual(request.Password, passwordSample);
        Assert.AreEqual(request.Photo, ProfilePictureUrl );
    }
    
    [TestMethod]
    public void UserRequestToUserEntity()
    {
        User entity = request.ToEntity();

        Assert.AreEqual(entity.Name, nameSample);
        Assert.AreEqual(entity.Surname, surnameSample);
        Assert.AreEqual(entity.Email, emailSample);
        Assert.AreEqual(entity.Password, passwordSample);
        Assert.AreEqual(entity.Photo, ProfilePictureUrl );
    }
}