using System.ComponentModel.DataAnnotations;

namespace Identity.Data.Dto
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }


        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password  { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}
