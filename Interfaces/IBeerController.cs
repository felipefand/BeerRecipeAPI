using BeerRecipeAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BeerRecipeAPI.Interfaces
{
    public interface IBeerController
    {
        Task<IActionResult> Get(int page, int maxResults);
        Task<IActionResult> GetById(int id);
        Task<IActionResult> Post(BeerDto beerDto);
        Task<IActionResult> PostQuery([FromBody] BeerQueryDto beerQueryDto);
        Task<IActionResult> Put(int id, BeerDto beerDto);
        Task<IActionResult> Patch (int id, BeerPatchDto beerPatchDto);
        Task<IActionResult> Delete(int id);
    }
}
