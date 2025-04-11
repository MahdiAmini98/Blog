using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Blog.UI.CustomClaimsFactory
{
    //کار این کلاس اینکه
    // در زمان احراز هویت کاربر، اطلاعات اضافی را به ClaimsIdentity اضافه کند.
    //یعنی میتونیم با استفاده از  این کلاس 
    //claims 
    //های دلخواه خود را اضافه کنیم و در کامپوننت های مختلف از آن استفاده کنیم
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
    {
        public CustomClaimsPrincipalFactory(
            UserManager<User> userManager,
            IOptions<IdentityOptions> options
            ) : base(userManager, options)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name ?? "نامشخص"));
            identity.AddClaim(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? "ندارد"));

            return identity;
        }
    }
}
