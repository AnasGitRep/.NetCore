using System.ComponentModel.DataAnnotations;

namespace EmployManagementDataBase.Models.Authentication
{
    public class RegisterModel
    {

        [Required(ErrorMessage ="user name is required")]
        public string ?UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage ="Email is required")]
        public string ?Email { get; set; }

        [Required(ErrorMessage ="Password is required")]
        public string ?Password { get; set; }

        [Required]
        public string? Role { get; set; }

        
    }
}
