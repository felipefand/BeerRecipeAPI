using BeerRecipeAPI.Models;
using System.Text;
using System.Text.Json;

namespace BeerRecipeAPI.Utils
{
    public class UserGenerator
    {
        private readonly string[] _firstName = {"Fulano", "Felipe", "Nicoly", "Ciclano", "Arthur", "Gustavo", "Milena", "Rafael", "André"};
        private readonly string[] _lastName = { "da Silva", "Pereira", "Andrade", "Assis", "Machado", "Motta", "Fulaninho" };
        private readonly string[] _roles = { "!SysAdmin", "Manager", "Employee", "User" };
        private readonly int QUANTITY = 10;
        private readonly int SECRETUSERINDEX = 6;

        public void Generate()
        {
            var userList = new List<User>();
            var random = new Random();

            for (int i = 0; i < QUANTITY; i++)
            {
                if (i.Equals(SECRETUSERINDEX))
                {
                    userList.Add(GenerateSecretUser());
                    continue;
                }

                var randomFirstName = _firstName[random.Next(0, _firstName.Length)];
                var randomLastName = _lastName[random.Next(0, _lastName.Length)];
                var randomRole = _roles[random.Next(0, _roles.Length)];
                var user = new User(i + 1, $"{randomFirstName} {randomLastName}", randomFirstName, $"{randomLastName}{i}", randomRole);

                userList.Add(user);
            }

            var json = JsonSerializer.Serialize(userList);
            File.WriteAllText("users.json", json, Encoding.UTF8);
        }

        public User GenerateSecretUser()
        {
            return new User(SECRETUSERINDEX + 1, "Bruno Bragança", "Brun0", "Br@gança1", "SysAdmin");
        }
    }
}
