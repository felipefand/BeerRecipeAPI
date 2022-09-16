namespace BeerRecipeAPI.Dtos
{
    public class BeerQueryDto
    {
        public string? Name { get; set; }
        public string? Style { get; set; }
        public double? Abv { get; set; }
        public double? Ibu { get; set; }
        public double? Color { get; set; }
        public string? BrewMethod { get; set; }
        public int Page { get; set; } = 1;
        public int MaxResults { get; set; } = 10;

        public BeerQueryDto(string? name, string? style, double? abv, double? ibu, double? color, string? brewMethod, int page, int maxResults)
        {
            Name = name;
            Style = style;
            Abv = abv;
            Ibu = ibu;
            Color = color;
            BrewMethod = brewMethod;
            Page = page;
            MaxResults = maxResults;
        }
    }
}
