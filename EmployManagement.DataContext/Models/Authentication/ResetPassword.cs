using EmployManagementDataBase.Dto.Base;
using System.ComponentModel.DataAnnotations;

namespace EmployManagementDataBase.Models.Authentication
{
    public class ResetPassword:BaseEntity
    {
        public string? token { get; set; } = null;

        public DateTime? ChangeDate { get; set; }
        public string? email { get; set; } = null;

        [Required]
        public string? Password { get; set; } = null;

        [Compare("Password", ErrorMessage = "The password and confirmation password not matche")]
        public string? passwordConfirmation { get; set; } = null;
    }
}
