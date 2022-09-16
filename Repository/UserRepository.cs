using BeerRecipeAPI.Interfaces;
using BeerRecipeAPI.Models;

namespace BeerRecipeAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users;
        private readonly DataSerializer<User> _dataSerializer;

        public UserRepository()
        {
            _dataSerializer = new DataSerializer<User>();
            _users = _dataSerializer.GetList("users");
        }

        public Task<IQueryable<User>> Get(int page, int maxResults)
        {
            return Task.Run(() =>
            {
                var users = _users.AsQueryable().Skip((page - 1) * maxResults).Take(maxResults);
                return users.Any() ? users : new List<User>().AsQueryable();
            });
        }

        public Task<User?> Get(string username, string password)
        {
            return Task.Run(() =>
            {
                return _users.FirstOrDefault(u => u.Username.Equals(username) && u.Password.Equals(password));
            });
        }
    }
}
