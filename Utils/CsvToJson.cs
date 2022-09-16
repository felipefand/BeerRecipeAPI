using BeerRecipeAPI.Models;
using System.Text;
using System.Text.Json;

namespace BeerRecipeAPI
{
    public class CsvToJson
    {
        public void Convert()
        {
            var beerList = new List<Beer>();

            Console.Clear();
            var lines = File.ReadAllLines("data.csv", Encoding.GetEncoding("iso-8859-1"));
            for (int i = 1; i < 1001; i++)
            {
                var separatedColumns = lines[i].Split(',');
                var id = int.Parse(separatedColumns[0]);
                var name = separatedColumns[1];
                var url = $"https://www.brewersfriend.com{separatedColumns[2]}";
                var style = separatedColumns[3];
                var abv = double.Parse(separatedColumns[8].Replace(".", ","));
                var ibu = double.Parse(separatedColumns[9].Replace(".", ","));
                var color = double.Parse(separatedColumns[10].Replace(".", ","));
                var brewMethod = separatedColumns[17];

                var beer = new Beer(id, name, url, style, abv, ibu, color, brewMethod);
                beerList.Add(beer);
            }

            var json = JsonSerializer.Serialize(beerList);
            File.WriteAllText("beer.json", json, Encoding.GetEncoding("iso-8859-1"));
        }
    }
}
