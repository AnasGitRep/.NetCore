using EmployManagement.Dto.Base;
using EmployManagement.Models.Other;

namespace EmployManagement.Models.Master
{
    public class Employe: BaseEntity
    {
        public string ? FullName{ get; set; }
        public string? Contact { get; set; }

        public string? Email { get; set; }

        public DateTime? JoinDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }

        public Depatment ? Depatment { get; set; }
        public int DepatmentId {  get; set; }

        public Manager ?Manager { get; set; }
        public int ManagerId { get; set; }
    }
}
