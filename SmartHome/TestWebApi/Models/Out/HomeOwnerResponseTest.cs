using Domain;
using WebApi.Models.Out;

namespace TestWebApi.Models.Out;

[TestClass]
public class HomeOwnerResponseTest
{
    private const string NameSample = "nameSample";
    private const string SurnameSample = "surnameSample";
    private const string EmailSample = "email@sample.com";
    private const string PasswordSample = "passwordSample1@";
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    
    [TestMethod]
    public void AssignValues()
    {
        User user = new User()
        {
            Name = NameSample,
            Surname = SurnameSample,
            Email = EmailSample,
            Password = PasswordSample,
            Photo = ProfilePictureUrl
        };

        HomeOwnerResponse response = new HomeOwnerResponse(user);

        Assert.IsTrue(
            user.Name == response.Name &&
            user.Surname == response.Surname &&
            response.Roles.Count() == 1 &&
            user.Email == response.Email &&
            user.Photo == response.Photo
        );

    }
}