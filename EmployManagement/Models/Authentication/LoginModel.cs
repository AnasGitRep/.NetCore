using System.ComponentModel.DataAnnotations;

namespace EmployManagement.Models.Authentication
{
    public class LoginModel
    {

        [Required(ErrorMessage ="username is requires")]
        public string ?UserName { get; set; }

        [Required(ErrorMessage ="Password is required")]
        public string? Password { get; set; }   
    }
}
