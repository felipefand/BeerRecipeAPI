using BeerRecipeAPI.Models;
using System.Text.Json;

namespace BeerRecipeAPI.Repository
{
    public class DataSerializer <T> where T : class
    {
        public List<T> GetList(string fileName)
        {
            using var reader = new StreamReader($"{fileName}.json");
            var json = reader.ReadToEnd();
            var list = JsonSerializer.Deserialize<List<T>>(json);

            return list;
        }

        public void SaveList(string fileName, List<T> list)
        {
            var json = JsonSerializer.Serialize(list);
            File.WriteAllText(fileName, json);
        }
    }
}
