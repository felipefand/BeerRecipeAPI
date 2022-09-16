using BeerRecipeAPI.Dtos;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using System.Text.Json;

namespace BeerRecipeAPI.Filters
{
    public class CustomPutFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            using (StreamReader reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                var json = reader.ReadToEndAsync().Result;
                Console.WriteLine(json);
            }
        }
    }
}
