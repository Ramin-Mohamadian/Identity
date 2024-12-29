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
            if (ModelState.IsValid==false)
            return View(register);


            User newUser = new User()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                UserName=register.Email,
            };

            var result=_userManager.CreateAsync(newUser,register.Password).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }

            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }

            TempData["message"]=message;

            return View();
        }


    }
}
