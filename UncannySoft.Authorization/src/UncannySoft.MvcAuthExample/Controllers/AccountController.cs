using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;
using Microsoft.AspNet.Http.Authentication;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace UncannySoft.MvcAuthExample.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index() => View();
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUri = null) => await Unauthorized(returnUri);

        [AllowAnonymous]
        public async Task<IActionResult> Unauthorized(string returnUri = null)
        {
            const string Issuer = "https://contoso.com";
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, "barry", ClaimValueTypes.String, Issuer),
                new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String, Issuer),
                new Claim("EmployeeId", "123", ClaimValueTypes.String, Issuer),
                new Claim(ClaimTypes.DateOfBirth, "1970-06-08", ClaimValueTypes.Date),
                new Claim("BadgeNumber", "123456", ClaimValueTypes.String, Issuer),
                //new Claim("TemporaryBadgeExpiry", DateTime.Now.AddDays(-1).ToString(), ClaimValueTypes.String, Issuer),
            };

            var userIdentity = new ClaimsIdentity("SuperSecureLogin");
            userIdentity.AddClaims(claims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.Authentication.SignInAsync("Cookie", userPrincipal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                    IsPersistent = false,
                    AllowRefresh = false
                });

            return RedirectToLocal(returnUri);
        }

        public IActionResult Forbidden()
        {
            return View();
        }

        public async Task<IActionResult> Logout(string returnUri)
        {
            await HttpContext.Authentication.SignOutAsync("Cookie");
            return RedirectToLocal(returnUri);
        }

        private IActionResult RedirectToLocal(string returnUri)
        {
            if (Url.IsLocalUrl(returnUri))
            {
                return Redirect(returnUri);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
