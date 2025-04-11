using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
//AddAuthenticationStateDeserialization
//دی سریالایز کردن اطلاعات کاربر که در فایل program.cs 
//پروژه سمت سرور ارسال میشه
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

await builder.Build().RunAsync();
