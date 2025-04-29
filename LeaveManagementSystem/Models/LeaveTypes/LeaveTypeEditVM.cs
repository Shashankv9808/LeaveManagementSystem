using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Models.LeaveTypes
{
    public class LeaveTypeEditVM
    {
        public int ID { get; set; }
        [Required]
        [Length(4, 150, ErrorMessage = "Violated the length requirements")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(1, 90, ErrorMessage = "Violated the range requirements")]
        [Display(Name = "Maximum Allocation of Days")]
        public int Days { get; set; }
    }
}
