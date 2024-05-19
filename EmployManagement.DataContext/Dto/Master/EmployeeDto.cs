
using EmployManagementDataBase.Dto.Base;

namespace EmployManagementDataBase.Dto.Master
{
    public class EmployeeDto :BaseEntity
    {

        public string? FullName { get; set; }
        public string? Contact { get; set; }

        public string? Email { get; set; }

        public DateTime? JoinDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }

        public string ?DepatmentName { get; set; }
        public int DepatmentId { get; set; }

        public string? Manager { get; set; }
        public int ManagerId { get; set; }

    }
}
