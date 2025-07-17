using LeaveManagementSystem.Models.LeaveTypes;
using LeaveManagementSystem.Models.Periods;

namespace LeaveManagementSystem.Models.LeaveAllocations
{
    public class LeaveAllocationVM
    {
        public int LeaveAllocationId { get; set; }
        [Display(Name = "Number of Days")]
        public int NumberOfDays { get; set; }
        [Display(Name = "Allocation Period")]
        public PeriodVM Period { get; set; } = new PeriodVM();
        public LeaveTypeReadOnlyVM LeaveType { get; set; } = new LeaveTypeReadOnlyVM();
    }
}
