using Domain;
using WebApi.Out;

namespace TestWebApi.Out;

[TestClass]
public class HomeOwnerResponseTest
{
    private readonly string nameSample = "nameSample";
    private readonly string surnameSample = "surnameSample";
    private readonly string emailSample = "email@sample.com";
    private readonly string passwordSample = "passwordSample1@";
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    
    [TestMethod]
    public void AssignValues()
    {
        User user = new User()
        {
            Name = nameSample,
            Surname = surnameSample,
            Email = emailSample,
            Password = passwordSample,
            Photo = ProfilePictureUrl
        };

        HomeOwnerResponse response = new HomeOwnerResponse(user);

        Assert.AreEqual(user.Name, response.Name);
        Assert.AreEqual(user.Surname, response.Surname);
        Assert.AreEqual(1, response.Roles.Count());
        Assert.AreEqual(user.Email, response.Email);
        Assert.AreEqual(user.Photo, response.Photo);
    }
}