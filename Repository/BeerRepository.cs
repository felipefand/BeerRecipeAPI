using BeerRecipeAPI.Dtos;
using BeerRecipeAPI.Interfaces;
using BeerRecipeAPI.Models;

namespace BeerRecipeAPI.Repository
{
    public class BeerRepository : IBeerRepository
    {
        private List<Beer> _beers;
        private readonly DataSerializer<Beer> _dataSerializer;
        private readonly string DBNAME = "beer.json";

        public BeerRepository()
        {
            _dataSerializer = new DataSerializer<Beer>();
            _beers = _dataSerializer.GetList("beer");
        }

        public Task<IQueryable<Beer>> Get(int page, int maxResults)
        {
            return Task.Run(() =>
            {
                var beers = _beers.AsQueryable().Skip((page -1 ) * maxResults).Take(maxResults);
                return beers.Any() ? beers : new List<Beer>().AsQueryable();
            });
        }

        public Task<IQueryable<Beer>> GetAny(BeerQueryDto beerQueryDto)
        {
            return Task.Run(() =>
            {
                var beers = _beers.AsQueryable().Where(x =>
                    beerQueryDto.Name == null ? false : x.Name.Equals(beerQueryDto.Name)
                    || beerQueryDto.Style == null ? false : x.Style.Equals(beerQueryDto.Style)
                    || beerQueryDto.Abv == null ? false : x.Abv.Equals(beerQueryDto.Abv)
                    || beerQueryDto.Ibu == null ? false : x.Ibu.Equals(beerQueryDto.Ibu)
                    || beerQueryDto.Color == null ? false : x.Color.Equals(beerQueryDto.Color)
                    || beerQueryDto.BrewMethod == null ? false : x.BrewMethod.Equals(beerQueryDto.BrewMethod)
                    ).Skip((beerQueryDto.Page - 1) * beerQueryDto.MaxResults).Take(beerQueryDto.MaxResults);

                return beers.Any() ? beers : new List<Beer>().AsQueryable();
            });
        }

        public Task<Beer?> GetById(int id)
        {
            return Task.Run(() =>
            {
                return _beers.FirstOrDefault(x => x.Id == id);
            });
        }

        public Task<Beer> Insert(BeerDto beerDto)
        {
            return Task.Run(() =>
            {
                var beer = CreateBeer(beerDto);
                _beers.Add(beer);
                _dataSerializer.SaveList(DBNAME, _beers);
                return beer;
            });
        }

        public Task<Beer?> UpdatePut(int id, BeerDto beerDto)
        {
            return Task.Run(async () =>
            {
                var beer = await GetById(id);
                if (beer == null)
                    return beer;

                UpdateRepositoryBeerPut(beerDto, beer);
                //_dataSerializer.SaveList(DBNAME, _beers);
                return beer;
            });
        }

        public Task<Beer?> UpdatePatch(int id, BeerPatchDto beerPatchDto)
        {
            return Task.Run(async () =>
            {
                var beer = await GetById(id);
                if (beer == null)
                    return beer;

                UpdateRepositoryBeerPatch(beerPatchDto, beer);
                //_dataSerializer.SaveList(DBNAME, _beers);
                return beer;
            });
        }

        public Task<int> Delete(Beer beer)
        {
            return Task.Run(() =>
            {
                _beers.Remove(beer);
                _dataSerializer.SaveList(DBNAME, _beers);
                return beer.Id;
            });
        }


        private void UpdateRepositoryBeerPut(BeerDto beerDto, Beer beer)
        {
            beer.Name = beerDto.Name;
            beer.Url = beerDto.Url;
            beer.Style = beerDto.Style;
            beer.Abv = beerDto.Abv;
            beer.Ibu = beerDto.Ibu;
            beer.Color = beerDto.Color;
            beer.BrewMethod = beerDto.BrewMethod;

        }

        private void UpdateRepositoryBeerPatch(BeerPatchDto beerPatchDto, Beer beer)
        {
            if (beerPatchDto.Name != null)
                beer.Name = beerPatchDto.Name;

            if (beerPatchDto.Url != null)
                beer.Url = beerPatchDto.Url;

            if (beerPatchDto.Style != null)
                beer.Style = beerPatchDto.Style;

            if (beerPatchDto.Abv != null)
                beer.Abv = (double)beerPatchDto.Abv;

            if (beerPatchDto.Ibu != null)
                beer.Ibu = (double)beerPatchDto.Ibu;

            if (beerPatchDto.Color != null)
                beer.Color = (double)beerPatchDto.Color;

            if (beerPatchDto.BrewMethod != null)
                beer.BrewMethod = beerPatchDto.BrewMethod;
        }

        private int GetBeerId()
        {
            var orderedList = _beers.OrderBy(x => x.Id).ToList();
            for (int i = 1; i < orderedList.Count; i++)
            {
                if (!i.Equals(orderedList[i - 1].Id))
                    return i;
            }

            return orderedList.Count + 1;
        }

        private Beer CreateBeer(BeerDto beerDto)
        {
            var id = GetBeerId();
            var beer = new Beer(id, beerDto.Name, beerDto.Url, beerDto.Style, beerDto.Abv, beerDto.Ibu, beerDto.Color, beerDto.BrewMethod);
            return beer;
        }
    }
}
