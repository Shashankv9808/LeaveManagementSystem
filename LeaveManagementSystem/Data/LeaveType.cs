using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Data
{
    public class LeaveType : BaseEntity
    {
        [Key]
        public int LeaveTypeID { get; set; }
        [Column(TypeName = "varchar(150)")]
        public required string LeaveTypeName { get; set; }
        public int NumberOfDays { get; set; }
        public List<LeaveAllocation>? LeaveAllocations { get; set; }
    }
}
