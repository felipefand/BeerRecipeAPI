using BeerRecipeAPI.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BeerRecipeAPI.Interfaces
{
    public interface ILoginController
    {
        Task<IActionResult> Login(AuthenticationInfo authenticationInfo);
        Task<IActionResult> Get(int page, int maxResults);
    }
}
