﻿using Blog.PanelAdmin;
using Blog.PanelAdmin.Services.Authentication;
using Blog.PanelAdmin.Services.AuthenticationStateProvider;
using Blog.PanelAdmin.Services.LocalStorage;
using Blog.PanelAdmin.Services.TokenService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blog.PanelAdmin.Handlers;
using MudBlazor.Services;
using Blog.PanelAdmin.Services.Categories;
using Blog.PanelAdmin.Extensions;
using MudBlazor;

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


//اضافه کردن سرویس ها
builder.Services.AddApplicationServices();

//اضافه کردن سرویس های Mud Blazor
builder.Services.AddMudServices(config =>
{
    //تنظیمات پیش فرض برای snack bar 
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});


await builder.Build().RunAsync();
