using BusinessLogic.IServices;
using Domain;
using IDataAccess;

namespace BusinessLogic.Services;

public class HomeService (IRepository<Home> homeRepository) : IHomeService
{
    public void CreateHome(Home home)
    {
        homeRepository.Add(home);
    }
}