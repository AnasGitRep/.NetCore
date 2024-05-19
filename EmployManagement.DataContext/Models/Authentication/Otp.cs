

using EmployManagementDataBase.Dto.Base;

namespace EmployManagementDataBase.Models.Authentication
{
    public class Otp:BaseEntity
    {
        public string? OTP { get; set; }

        public string? UserId { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
