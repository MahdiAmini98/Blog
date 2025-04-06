using Blog.API.Model.Authentication;
using Blog.Application.Contracts.Authentication;
using Blog.Application.Interfaces.Authentication;
using Blog.Domain.Entities;
using Blog.Infrastructure.Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuthController(
        ITokenService tokenService,
        JwtService jwtService,
        UserManager<User> _userManager,
        SignInManager<User> _signInManager
        ) : ControllerBase
    {

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid username or password." });

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid username or password." });


            var claims = new List<Claim>
             {
               new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
             };

            // افزودن نقش‌های کاربر به Claims
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


            var token = jwtService.GenerateToken(new GenerateTokenRequest(user.Id, claims));

            return Ok(token);
        }
    }
}
