using Blog.PanelAdmin;
using Blog.PanelAdmin.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


#region Authorize
//1-add package webassymbly.Authorize

//2-add component in main Layout

//3- add services

//سرویس اضافه شدن Authorization
builder.Services.AddAuthorizationCore();

// این اطلاعات کاربری که لاگین شده را به همه کامپوننت ها ارسال می کند
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

#endregion
await builder.Build().RunAsync();
