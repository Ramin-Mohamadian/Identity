using Identity.Data.Dto;
using Identity.Data.Entity;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly EmailService _emailService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager
            , RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = new EmailService();
        }


        public IActionResult Index()
        {
            return View();
        }




        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(RegisterDto register)
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
                var token = _userManager.GenerateEmailConfirmationTokenAsync(newUser).Result;
                string callbackUrl = Url.Action("ConfirmEmail", "Account", new { UserId = newUser.Id, Token = token },
                    protocol: Request.Scheme);

                string Body = $"کاربر گرامی برای فعال سازی ایمیل خود روی لینک زیر کلیک کنید! <br/> <a href={callbackUrl}> Link </a>";

                _emailService.Excute(newUser.Email, Body, "تایید ایمیل حساب کاربر");
                return RedirectToAction("DisplayEmail");
            }

            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }

            TempData["message"] = message;

            return View(register);
        }




        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Login(LoginDto login)
        {

            if (ModelState.IsValid == false)
                return View(login);

            var user = _userManager.FindByNameAsync(login.UserName).Result;

            var result = _signInManager.PasswordSignInAsync(user, login.Password, login.IsPersistent, true).Result;


            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }



            return View();
        }



        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult DisplayEmail()
        {
            return View();
        }


        public IActionResult ConfirmEmail(string UserId, string Token)
        {
            if (UserId == null || Token == null)
            {
                return BadRequest();
            }

            var user = _userManager.FindByIdAsync(UserId).Result;
            if (user == null)
            {
                return NotFound();
            }

            var result = _userManager.ConfirmEmailAsync(user, Token).Result;
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordDto forgot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = _userManager.FindByEmailAsync(forgot.Email).Result;
            if (user == null)
            {
                return NotFound();
            }

            string token = _userManager.GeneratePasswordResetTokenAsync(user).Result;

            string callbackUrl = Url.Action("RessetPassword", "Account", new
            {
                UserId = user.Id,
                Token = token
            }, protocol: Request.Scheme);

            string Body = $"برای بازیابی کلمه عبور بر روی لینک زیر کلیک کنید! <br/> <a href={callbackUrl} > بازیابی کلمه عبور </a> ";

            _emailService.Excute(user.Email, Body, "بازیابی کلمه عبور");
            ViewData["SendEmail"] = true;
            return View();
        }



        [HttpGet]
        public IActionResult RessetPassword(string UserId, string Token)
        {

            return View(new RessetPasswordDto
            {
                UserId = UserId,
                Token = Token
            });
        }


        [HttpPost]
        public IActionResult RessetPassword(RessetPasswordDto resset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = _userManager.FindByIdAsync(resset.UserId).Result;
            if (user == null)
            {
                return NotFound();
            }

            var result=_userManager.ResetPasswordAsync(user,resset.Token,resset.Password).Result;
            if(result.Succeeded)
            {
                ViewData["ResetPass"]=true;
                return View();
            }

           return View(resset);
        }


    }
}
