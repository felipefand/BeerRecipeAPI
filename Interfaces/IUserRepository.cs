using BeerRecipeAPI.Models;

namespace BeerRecipeAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<IQueryable<User>> Get(int page, int maxResults);
        Task<User?> Get(string username, string password);
    }
}
