using System.ComponentModel.DataAnnotations;

namespace Identity.Data.Dto
{
    public class ForgotPasswordDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
