using Identity.Areas.Admin.Data.Dtos.Roles;
using Identity.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        public RoleController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.Select(p => new RoleListDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();


            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddNewRoleDto addNewRole)
        {
            if (!ModelState.IsValid)
            {
                return View(addNewRole);
            }
            Role role = new Role()
            {

                Name = addNewRole.Name,
            };


            var result = _roleManager.CreateAsync(role).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Role", new { area = "Admin" });
            }

            return View();
        }

        [HttpGet]
        public IActionResult EditRole(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;

            EditRoleDto editRoleDto = new EditRoleDto()
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(editRoleDto);
        }


        [HttpPost]
        public IActionResult EditRole(EditRoleDto editRoleDto)
        {
            var role = _roleManager.FindByIdAsync(editRoleDto.Id).Result;
            if (role == null)
            {
                return NotFound();
            }

            role.Name = editRoleDto.Name;
            role.Id= editRoleDto.Id;
            var result=_roleManager.UpdateAsync(role).Result;
            if (result.Succeeded)
            {
                return Redirect("/Admin/Role");
            }

            return View();
        }

    }
}
