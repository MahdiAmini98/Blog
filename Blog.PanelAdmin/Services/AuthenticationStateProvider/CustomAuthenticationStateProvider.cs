using Blog.PanelAdmin.Services.TokenService;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blog.PanelAdmin.Services.AuthenticationStateProvider
{
    public class CustomAuthenticationStateProvider : Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider
    {
        private readonly ITokenService tokenService;

        public CustomAuthenticationStateProvider(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await tokenService.GetAuthTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                //کاربر لاگین نیست
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claimsPrincipal = CreateClaimsPrincipalFromJwt(token);
            return new AuthenticationState(claimsPrincipal);

        }

        private ClaimsPrincipal CreateClaimsPrincipalFromJwt(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(jwt))
            {
                return new ClaimsPrincipal(new ClaimsIdentity());
            }

            var jwtToken = tokenHandler.ReadJwtToken(jwt);

            var claims = jwtToken.Claims;

            var identity = new ClaimsIdentity(claims, "jwt");

            return new ClaimsPrincipal(identity);
        }

        // وقتی تغییراتی مثل لاگین یا لاگ اوت انجام می شود ما باید آن تغییرلات به گوش کامپوننت ها و برنامه خودمون برسونیم
        //که کامپوننت ها بتوانند تغییر وضعیت کاربر در 
        //athorize 
        //در نظر بگیرند
        public void UpdateAuthenticationState()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }


    }
}