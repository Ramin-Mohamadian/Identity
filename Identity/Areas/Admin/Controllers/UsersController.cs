using Identity.Areas.Admin.Data.Dtos;
using Identity.Data.Dto;
using Identity.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }



        public IActionResult Index()
        {
            var users = _userManager.Users.Select(p => new UserListDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                UserName=p.UserName,
                EmailConfirmed=p.EmailConfirmed,
                PhoneNumber=p.PhoneNumber,
                AccessFailedCount = p.AccessFailedCount
            }).ToList();


            return View(users);
        }


        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }



        [HttpPost]
        public IActionResult CreateUser(RegisterDto register)
        {
            if (ModelState.IsValid == false)
                return View(register);


            User newUser = new User()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                UserName = register.Email,
            };

            var result = _userManager.CreateAsync(newUser, register.Password).Result;

            if (result.Succeeded)
            {
                return Redirect("/Admin/Users/Index");
            }

            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }

            TempData["message"] = message;

            return View(register);          
        }





    }
}
