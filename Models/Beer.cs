namespace BeerRecipeAPI.Models
{
    public class Beer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Style { get; set; }
        public double Abv { get; set; }
        public double Ibu { get; set; }
        public double Color { get; set; }
        public string BrewMethod { get; set; }

        public Beer(int id, string name, string url, string style, double abv, double ibu, double color, string brewMethod)
        {
            Id = id;
            Name = name;
            Url = url;
            Style = style;
            Abv = abv;
            Ibu = ibu;
            Color = color;
            BrewMethod = brewMethod;
        }

        public Beer Clone()
        {
            return (Beer)this.MemberwiseClone();
        }
    }
}
