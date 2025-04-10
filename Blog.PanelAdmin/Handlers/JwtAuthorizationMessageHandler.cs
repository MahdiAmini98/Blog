using Blog.PanelAdmin.Services.TokenService;
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
    public class JwtAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly ITokenService tokenService;

        public JwtAuthorizationMessageHandler(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await tokenService.GetAuthTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
