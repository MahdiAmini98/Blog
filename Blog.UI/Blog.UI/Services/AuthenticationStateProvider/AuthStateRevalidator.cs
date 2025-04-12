using Blog.Domain.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Blog.UI.Services.AuthenticationStateProvider
{
    //.149
    //فکر کن ما صفحات interactive server داریم
    //ارتباط این صفحات با signalR است
    //و ما می خواهیم وقتی کاربر لاگین کرد یا از لاگین خارج شد یعنی اطلاعات کاربر وقتی با لاگین یا لاگ اوت تغییر می کند
    //باید در کانکشن signalR هم این تغییرات را اعمال کنیم
    // که در این صورت این تغییرات که در کانکشن signalR اعمال می شود و سپس در
    //بک اند اطلاعات جدبد لاگین و لاگ اوت کاربر را می گیرد در اختبار کامپوننت ها قرار دهد
    // حالا اگر کارهایی که در بالا توضیح دادم انجام نشه این اتفاق میوفته که کاربر در یک صفحه ای هست و سپس روی خروج می زند
    //و لاگ اوت می کند.
    //در این حالت کامپوننت و یا صفحه مورد نظر نمی فهمه این کاربر لاگ اوت شده و هنوز داره اطلاعات کاربر نمایش میده
    // تا وقتی که کاربر صفحه را رفرش نکد یا به صفحه دیگه ای نرود که ارتباط سیگنال آر دوباره برقرار شود همین اتفاق می افته
    // حالا ما برای حل این مشکل باید به صورت دوره ای مثلا هر چند دقیقه یک بار چک کنیم که کاربر لاگین هست یا نه و اگر
    //لاک اوت بود کامپوننت ها و صفحات از لاگ اوت بودن کاربر آگاه شوند
    // منظور از کامپوننت ها همه کامپوننت هایی که داخل کامپوننت AuthorizedView قرار دارند
    // و در واقع در این کامپوننت ها از 
    //در وب اسمبلی برای این کار کلاس CustomAuthenticationStateProvider نوشتیم
    //ValidateAuthenticationStateAsync  اگر خروجی این متد == false بود 
    //دوباره  کامپوننت
    // <AuthorizeView> که در 
    //Layout.razor   است رندر می شود
    // و در واقع این کامپوننت دوباره رندر می شود و در نتیجه کامپوننت های فرزند هم دوباره رندر می شوند
    // TimeSpan.FromMinutes(10) => هر 10 دقیقه یک بار  چک کمی کند

    public class AuthStateRevalidator(ILoggerFactory loggerFactory,
          IServiceScopeFactory scopeFactory,
          IOptions<IdentityOptions> options
         )
         : RevalidatingServerAuthenticationStateProvider(loggerFactory)
    {
        protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(10);

        protected async override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState,
            CancellationToken cancellationToken)
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            ClaimsPrincipal principal = authenticationState.User;

            var user = await userManager.GetUserAsync(principal);

            if (user is null)
            {
                return false;
            }
            else
            {
                var principalStamp =
                    principal.FindFirstValue(options.Value.ClaimsIdentity.SecurityStampClaimType);

                var userStamp = await userManager.GetSecurityStampAsync(user);
                return principalStamp == userStamp;
            }
        }
    }
}
