using Identity.Data.Dto;
using Identity.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }




        public IActionResult Registe()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Registe(RegisterDto register)
        {



            return View();
        }


    }
}
