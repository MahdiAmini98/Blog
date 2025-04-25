using Blog.PanelAdmin.Handlers;
using Blog.PanelAdmin.Services.Authentication;
using Blog.PanelAdmin.Services.Categories;
using Blog.PanelAdmin.Services.LocalStorage;
using Blog.PanelAdmin.Services.Medias;
using Blog.PanelAdmin.Services.Tag;
using Blog.PanelAdmin.Services.TokenService;

namespace Blog.PanelAdmin.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
      

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILocalStorageService, LocalStorageService>();


            #region Jwt Services
            //add handler service
            services.AddScoped<JwtAuthorizationMessageHandler>();

            //add http service
            services.AddHttpClient("ApiWithAuth", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7230/");

            }).AddHttpMessageHandler<JwtAuthorizationMessageHandler>();
            #endregion


            services.AddHttpClient<IAuthService, AuthService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7230/");
            });


            services.AddScoped<ICategoryService, CategoryService>(sp =>
            {
                var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiWithAuth");
                return new CategoryService(httpClient);
            });


            services.AddScoped<ITagService, TagService>(sp =>
            {
                var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiWithAuth");
                return new TagService(httpClient);
            });



            services.AddScoped<IMediaService, MediaService>(sp =>
            {
                var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiWithAuth");
                return new MediaService(httpClient);
            });
            return services;
        }

    }
}
