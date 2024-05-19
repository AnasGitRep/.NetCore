
using EmployManagement.Data.Models.Other;
using EmployManagementDataBase.Dto.Base;

namespace EmployManagementDataBase.Models.Master
{
    public class Manager: BaseEntity
    {
        public string? name { get; set; }
        public Depatment? Department { get; set; }
        public int? DepartmentId { get; set; }
    }
}
