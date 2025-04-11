using Blog.PanelAdmin.Models.Authentication;
using Blog.PanelAdmin.Services.AuthenticationStateProvider;
using Blog.PanelAdmin.Services.TokenService;
using System.Net.Http.Json;

namespace Blog.PanelAdmin.Services.Authentication
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginRequestDto loginRequest);
        Task<string?> RefreshTokenAsync(string refreshToken);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly ITokenService tokenService;
        private readonly Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider authenticationStateProvider;

        public AuthService(HttpClient httpClient, ITokenService tokenService
            , Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider authenticationStateProvider)
        {
            this.httpClient = httpClient;
            this.tokenService = tokenService;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> LoginAsync(LoginRequestDto loginRequest)
        {
            var response = await httpClient.PostAsJsonAsync("/api/Auth/login", loginRequest);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                if (result != null)
                {

                    //ذخیره اطلاعات
                    await tokenService.SetTokenAsync(result.Token, result.RefreshToken);

                    //اطلاع رسانی تغییر وضعیت کاربر

                    ((CustomAuthenticationStateProvider)authenticationStateProvider).MarkUserAsAuthenticated();
                    return true;
                }
            }

            return false;
        }

        public async Task<string?> RefreshTokenAsync(string refreshToken)
        {
            var response = await httpClient.PostAsJsonAsync("/api/Auth/refresh", new { RefreshToken = refreshToken });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RefreshTokenResponseDto>();
                if (result != null)
                {
                    await tokenService.SetTokenAsync(result.Token, result.RefreshToken);

                    ((CustomAuthenticationStateProvider)authenticationStateProvider).MarkUserAsAuthenticated();


                    return result.Token;
                }
            }
            return null;
        }
    }
}
