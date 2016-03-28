using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace UncannySoft.MvcAuthExample.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index() => View();

        [Route("Error")]
        public IActionResult Error() => View();

        [Authorize(Policy = "AdministratorOnly")]
        public IActionResult Admin() => View();
    }
}
