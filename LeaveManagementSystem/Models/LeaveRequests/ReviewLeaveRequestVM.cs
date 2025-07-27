using LeaveManagementSystem.Models.LeaveAllocations;

namespace LeaveManagementSystem.Models.LeaveRequests
{
    public class ReviewLeaveRequestVM : LeaveReqestReadOnlyVM
    {
        public EmployeeListVM Employee { get; set; } = new EmployeeListVM();
        public string RequestComments { get; set; }
    }
}