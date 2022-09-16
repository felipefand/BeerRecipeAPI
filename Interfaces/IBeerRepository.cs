using BeerRecipeAPI.Dtos;
using BeerRecipeAPI.Models;

namespace BeerRecipeAPI.Interfaces
{
    public interface IBeerRepository
    {
        Task<IQueryable<Beer>> Get(int page, int maxResults);
        Task<Beer> GetById(int id);
        Task<IQueryable<Beer>> GetAny(BeerQueryDto beerQueryDto);
        Task<Beer> Insert (BeerDto beer);
        Task<Beer> UpdatePut (int id, BeerDto beerDto);
        Task<Beer> UpdatePatch (int id, BeerPatchDto beerPatchDto);
        Task<int> Delete(Beer beer);
    }
}
