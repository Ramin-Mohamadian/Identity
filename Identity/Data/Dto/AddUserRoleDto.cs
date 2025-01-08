using Microsoft.AspNetCore.Mvc.Rendering;

namespace Identity.Data.Dto
{
    public class AddUserRoleDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public string Role { get; set; }
        public List<SelectListItem> Roles   { get; set; }
    }
}
