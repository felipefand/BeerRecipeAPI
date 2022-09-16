using BeerRecipeAPI.Auth;
using BeerRecipeAPI.Interfaces;
using BeerRecipeAPI.Models;
using BeerRecipeAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeerRecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller, ILoginController
    {
        private readonly IUserRepository _repository;
        private readonly GenerateToken _tokenGenerator;
        private readonly AuthenticationInfo _firstAuthenticationInfo;

        public LoginController (IUserRepository userRepository, GenerateToken tokenGenerator, AuthenticationInfo firstAuthenticationInfo)
        {
            _repository = userRepository;
            _tokenGenerator = tokenGenerator;
            _firstAuthenticationInfo = firstAuthenticationInfo;
        }

        [HttpGet]
        [Route("UserBase")]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[Authorize(Roles = "User,SysAdmin")]
        public async Task<IActionResult> Get(int page, int maxResults)
        {
            var user = await _repository.Get(page, maxResults);
            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(AuthenticationInfo authenticationInfo)
        {

            if (authenticationInfo.Username.Equals(_firstAuthenticationInfo.Username) && authenticationInfo.Password.Equals(_firstAuthenticationInfo.Password))
                return Ok(_tokenGenerator.GenerateJwt());

            var user = await _repository.Get(authenticationInfo.Username, authenticationInfo.Password);

            if (user == null)
                return Unauthorized("Usuário ou senha inválidos");

            user.Password = "";
            var token = _tokenGenerator.GenerateJwt(user);
            return Ok(new { user = user, token = token });

        }
    }
}
