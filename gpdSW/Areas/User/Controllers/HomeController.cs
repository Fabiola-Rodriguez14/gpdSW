using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gpdSW.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    [Route("/User/[controller]/[action]/{id?}")]
    public class HomeController : Controller
    {
        [Route("/User")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

