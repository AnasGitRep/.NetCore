using EmployManagement.Dto.Base;
using EmployManagement.Models.Other;

namespace EmployManagement.Models.Master
{
    public class Manager: BaseEntity
    {
        public string? name { get; set; }
        public Depatment? Department { get; set; }
        public int? DepartmentId { get; set; }
    }
}
