using BeerRecipeAPI.Dtos;
using BeerRecipeAPI.Filters;
using BeerRecipeAPI.Interfaces;
using BeerRecipeAPI.Models;
using BeerRecipeAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BeerRecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "SysAdmin")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class BeerController : Controller, IBeerController
    {
        private readonly IBeerRepository _repository;
        public BeerController (IBeerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Beer>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int page, int maxResults)
        {
            var beers = await _repository.Get(page, maxResults);
            return Ok(beers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Beer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var beer = await _repository.GetById(id);

            if (beer == null) return NoContent();
            return Ok(beer);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Beer), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] BeerDto beerDto)
        {
            var beer = await _repository.Insert(beerDto);
            return Created(string.Empty, beer);
        }

        [HttpPost]
        [Route("query")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<Beer>), StatusCodes.Status200OK)]

        public async Task<IActionResult> PostQuery([FromBody] BeerQueryDto beerQueryDto)
        {
            var beers = await _repository.GetAny(beerQueryDto);
            return Ok(beers);
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Beer), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Beer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [CustomPutFilter]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] BeerDto beerDto)
        {
            var beer = await _repository.GetById(id);

            if (beer == null)
            {
                var insertedBeer = await _repository.Insert(beerDto);
                return Created(string.Empty, insertedBeer);
            }

            var updatedBeer = await _repository.UpdatePut(id, beerDto);
            return Ok(updatedBeer);
        }

        [HttpPatch("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Beer), StatusCodes.Status200OK)]
        public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] BeerPatchDto beerPatchDto)
        {
            var beer = await _repository.GetById(id);

            if (beer == null) return NoContent();

            var updatedBeer = await _repository.UpdatePatch(id, beerPatchDto);
            return Ok(updatedBeer);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var beer = await _repository.GetById(id);

            if (beer == null) return NoContent();

            var deletedBeerId = await _repository.Delete(beer);
            return Ok(deletedBeerId);
        }
    }
}
