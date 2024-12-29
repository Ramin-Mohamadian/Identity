using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }




        public IActionResult Registe()
        {
            return View();
        }



    }
}
