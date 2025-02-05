﻿using Identity.Areas.Admin.Data.Dtos;
using Identity.Data.Dto;
using Identity.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Identity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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



        [HttpGet]
        public IActionResult EditUser(string id)
        {
            var user=_userManager.FindByIdAsync(id).Result;

            EditeUserDto userDto = new EditeUserDto()
            {
                Id = user.Id,
                Email=user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName=user.UserName,
                PhoneNumber=user.PhoneNumber
            };


            return View(userDto);
        }





        [HttpPost]
        public IActionResult EditUser(EditeUserDto edite)
        {
            var user=_userManager.FindByIdAsync(edite.Id).Result;

            if(user == null) 
                return NotFound();

            user.Email = edite.Email;
            user.FirstName = edite.FirstName;
            user.LastName = edite.LastName;
            user.UserName = edite.UserName;
            user.PhoneNumber = edite.PhoneNumber;


            var result=_userManager.UpdateAsync(user).Result;



            if (result.Succeeded)
            {
                return Redirect("/Admin/Users/Index");
            }

            return View();
        }




        [HttpGet]
        public IActionResult AddUserRole(string Id)
        {

            var user = _userManager.FindByIdAsync(Id).Result;

            var roles = new List<SelectListItem>(
                _roleManager.Roles.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Name
                }).ToList()
                );
            return View(new AddUserRoleDto
            {
                Id = Id,
                Roles = roles,
                Email = user.Email,
                UserName = user.UserName
            });
        }

        [HttpPost]
        public ActionResult AddUserRole(AddUserRoleDto addUserRoleDto)
        {
            var user = _userManager.FindByIdAsync(addUserRoleDto.Id).Result;

            var result = _userManager.AddToRoleAsync(user, addUserRoleDto.Role).Result;



            return RedirectToAction("UserRoles", "Users", new { Id = user.Id, area = "admin" });
        }




        public IActionResult UserRoles(string Id)
        {
            var user = _userManager.FindByIdAsync(Id).Result;
            var roles = _userManager.GetRolesAsync(user).Result;
            return View(roles);
        }


    }
}
