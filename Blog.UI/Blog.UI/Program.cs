using Blog.Application.Interfaces;
using Blog.Application.Interfaces.BlogUIService;
using Blog.Application.Services;
using Blog.Application.Services.BlogUIService;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.Persistence.Contexts;
using Blog.Persistence.Repositories;
using Blog.Persistence.Transactions;
using Blog.UI.Client.Pages;
using Blog.UI.Components;
using Blog.UI.CustomClaimsFactory;
using Blog.UI.Services.AuthenticationStateProvider;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//  .AddAuthenticationStateSerialization()
//داده های 
//Authentication را در سمت
//سرور سریالایز می کند و برای 
//client همین پروژه می فرستد
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors = true;
    })
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();


//اطلاعات کاربر لاگین شده را به صورت پارامتر به همه کامپوننت ها ارسال می کنه
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


#region Identity

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";

    options.ExpireTimeSpan = TimeSpan.FromDays(5);
    options.SlidingExpiration = true;

});

#endregion

#region Services

builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomClaimsPrincipalFactory>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateRevalidator>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBlogUIPostService, BlogUIPostService>();



#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Blog.UI.Client._Imports).Assembly);

//این متد مربوط به کلاس
//IdentityEndpointsExtensions می باشد که توضیحاتش در بالای این کد نوشته شده است
app.MapIdentityEndpoints();

app.Run();
