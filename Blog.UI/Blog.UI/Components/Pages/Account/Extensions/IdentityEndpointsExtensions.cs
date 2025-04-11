using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Routing
{
    //در بلیزور سرور زمانی که از identity استفاده می کنیم باید از متد 
    //signInManager.SignOutAsync استفاده کنیم
    //خب این متد باید header را بنویسد و 
    //اطلاعات مربوط به اهراز هویت را بنویسد
    //زمانی که از بلیزور اسرور استفاده می کنیم با استفاده از کانکشن سیگنال آر ارتباط بین سرور و کلاینت برقرار می شود ولی این ارتباط دسترسی و اجازه تغییر هدر ها را ندارد
    // intrtsctive باشد به همین دلیل وقتی یک کامپوننت که 
    //حالا چه intrtsctive-server یا interactive-webassymbley 
    //فرقی ندارد کدام باشد در هر صورت از سرویس ها ومتد های 
    //singInManager 
    // که مربوط به identity است 
    //نمیتوانیم استفاده کنیم و خطا دریافت می کنیم
    // دو راه حل دارد؟ 1- یا کامپوننت ما استاتیک رندر باشد و هیچ تمایلی نداشته باشد
    //2-یک endpoint برای این کار بسازیم
    // و زمانی که کاربر میخواهد از سیستم خارج شود و لاگ اوت شود 
    //یک ریکویست به این endpoint بفرستیم    
    // و در این endpoint از متد signInManager.SignOutAsync استفاده کنیم
    // حالا دقیقا اینجا در این کد این کار انجام شده است
    //فقط namespace این کلاس را
    //Microsoft.AspNetCore.Routing می گذاریم 
    //که راحت تر در program.cs استفاده کنیم
    public static class IdentityEndpointsExtensions
    {
        public static IEndpointConventionBuilder
            MapIdentityEndpoints(this IEndpointRouteBuilder endpoint)
        {
            ArgumentNullException.ThrowIfNull(endpoint);

            var accountGroup = endpoint.MapGroup("/Account");


            accountGroup.MapPost("/Logout", async (
              [FromServices] SignInManager<User> signInManager,
              [FromForm] string returnUrl = "/") =>
            {
                await signInManager.SignOutAsync();
                return TypedResults.LocalRedirect(returnUrl);
            });




            return accountGroup;

        }
    }
}
