using Microsoft.AspNet.Mvc;

namespace UncannySoft.MvcAuthExample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        [Route("Error")]
        public IActionResult Error() => View();
    }
}
