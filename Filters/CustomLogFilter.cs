using BeerRecipeAPI.Interfaces;
using BeerRecipeAPI.Logs;
using BeerRecipeAPI.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace BeerRecipeAPI.Filters
{
    public class CustomLogFilter : Attribute, IResultFilter, IActionFilter
    {
        private readonly IBeerRepository _repository;
        private readonly List<int> _successStatusCodes;
        private Beer _beforeBeer;

        public CustomLogFilter (IBeerRepository beerRepository)
        {
            _repository = beerRepository;
            _successStatusCodes = new List<int>() { StatusCodes.Status200OK, StatusCodes.Status201Created };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor.RouteValues["controller"].Equals("beer", StringComparison.InvariantCultureIgnoreCase))
            {
                var id = 0;
                if (context.ActionArguments.ContainsKey("id") && int.TryParse(context.ActionArguments["id"].ToString(), out id))
                {
                    if (context.HttpContext.Request.Method.Equals("put", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("patch", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var beer = _repository.GetById(id).Result;

                        if (beer == null) return;

                        _beforeBeer = beer.Clone();
                    }
                }
            }

            //var id = 0;
            //if (!int.TryParse(context.ActionArguments["id"].ToString(), out id))
            //    return;

            //var beer = _repository.GetById(id).Result;

            //_beforeBeer = beer.Clone();
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.HttpContext.Request.Path.Value.StartsWith("/api/beer", StringComparison.InvariantCultureIgnoreCase))
            {
                if (_successStatusCodes.Contains(context.HttpContext.Response.StatusCode))
                {
                    if (context.HttpContext.Request.Method.Equals("put", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("patch", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var id = int.Parse(context.HttpContext.Request.Path.ToString().Split("/").Last());
                        var beer = _repository.GetById(id).Result;

                        if (beer == null) return;

                        CustomLogger.SaveLog(beer.Id, "Beer", beer.Name, context.HttpContext.Request.Method, _beforeBeer, beer);
                    }
                    else if (context.HttpContext.Request.Method.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
                    {
                        CustomLogger.SaveLog(_beforeBeer.Id, "Beer", _beforeBeer.Name, context.HttpContext.Request.Method, _beforeBeer);
                        _beforeBeer = null;
                    }
                }
            }

            //if (!_successStatusCodes.Contains(context.HttpContext.Response.StatusCode))
            //    return;

            //var beer = _repository.GetById(_beforeBeer.Id).Result;

            //CustomLogger.SaveLog(beer.Id, "Beer", beer.Name, context.HttpContext.Request.Method, _beforeBeer, beer);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // NOT USED
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            // NOT USED
        }
    }
}
