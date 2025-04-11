using Blog.PanelAdmin.Services.Authentication;
using Blog.PanelAdmin.Services.TokenService;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

namespace Blog.PanelAdmin.Handlers
{
    // با استفاده از این کلاس میتوانیم 
    //توکن jwt را به 
    //صورت اتوماتیک روی request ها
    //در header http request 
    //jwt token را قرار می دهد
    //اینجوری به api
    //که ریکویست می زنیم در هدرش jwt دارد و اهراز هویت بین دو پروژه برقرار می شود
    // refresh token نیز تولید می کند
    public class JwtAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly ITokenService tokenService;
        private readonly IAuthService authService;
        private readonly NavigationManager navigationManager;
        public JwtAuthorizationMessageHandler(ITokenService tokenService,
            IAuthService authService,
            NavigationManager navigationManager)
        {
            this.tokenService = tokenService;
            this.authService = authService;
            this.navigationManager = navigationManager;

        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await tokenService.GetAuthTokenAsync();

                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await base.SendAsync(request, cancellationToken);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    //send RefreshToken
                    var refreshToken = await tokenService.GetRefreshTokenAsync();
                    if (!string.IsNullOrEmpty(refreshToken))
                    {
                        var newToken = await authService.RefreshTokenAsync(refreshToken);
                        if (!string.IsNullOrEmpty(newToken))
                        {
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
                            return await base.SendAsync(request, cancellationToken);
                        }
                    }

                    navigationManager.NavigateTo("login", true);
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in JwtAuthorizationMessageHandler: {ex.Message}");
                throw;

            }

        }
    }
}
