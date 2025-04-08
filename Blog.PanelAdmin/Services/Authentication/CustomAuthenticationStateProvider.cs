using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Blog.PanelAdmin.Services.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"مهدی امینی "),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim(ClaimTypes.Email,"Admin@admin.com"),
            };

            var identity = new ClaimsIdentity(claims, "testAuthentication");
            var user = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(user));

        }
    }
}
