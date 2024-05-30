using System.ComponentModel.DataAnnotations;

namespace Store.APIs.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName {  get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&amp;*()_+]).*$",ErrorMessage =
            "password must contain 1 upper,1 lower,1 digit, 1 special char")]
        public string Password { get; set; }
    }
}
