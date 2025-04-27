using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Data
{
    public class LeaveType
    {
        [Key]
        public int LeaveTypeID { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string LeaveTypeName { get; set; }
        public int NumberOfDays { get; set; }
    }
}
