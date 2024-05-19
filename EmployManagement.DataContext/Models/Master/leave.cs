

using EmployManagementDataBase.Dto.Base;

namespace EmployManagementDataBase.Models.Master
{
    public class Leave: BaseEntity
    {
        public string ? LeaveType {get;set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public LeaveStatus? Status { get; set; }

        public string ?ApprovedBy { get; set; }

    }


    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
