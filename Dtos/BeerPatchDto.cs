namespace BeerRecipeAPI.Dtos
{
    public class BeerPatchDto
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? Style { get; set; }
        public double? Abv { get; set; }
        public double? Ibu { get; set; }
        public double? Color { get; set; }
        public string? BrewMethod { get; set; }

        public BeerPatchDto(string? name, string? url, string? style, double? abv, double? ibu, double? color, string? brewMethod)
        {
            Name = name;
            Url = url;
            Style = style;
            Abv = abv;
            Ibu = ibu;
            Color = color;
            BrewMethod = brewMethod;
        }
    }
}
