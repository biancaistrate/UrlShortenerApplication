using Application.Repository;
using AutoMapper;
using Domain.Master;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class HomeController : Controller
    {
        private IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public HomeController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;

        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody]LoginInfo login, CancellationToken cancellationToken)
        {
            UserIdentity? user;
            user = await _userRepository.GetByEmailAndSecret(login.Email, login.PasswordHash, cancellationToken);
            if (user == null)
            {
                return new NotFoundObjectResult(login.Email);
            }

            await HttpContext.SignInAsync("default",
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[] {
                            new Claim("usr", user.Email)
                        }, "default")
                )
                );
            return Ok();
        }

        [HttpPut("/register")]
        public async Task<IActionResult> Register([FromBody] UserDTO user, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetByEmail(user.Email, cancellationToken) != null)
            {
                return new ConflictObjectResult("An user with the same email address is already registered");
            }
            await _userRepository.Create(_mapper.Map<UserIdentity>(user), cancellationToken);
            return Ok();
        }
    }
}
