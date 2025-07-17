namespace LeaveManagementSystem.Data
{
    public class LeaveRequestStatus : BaseEntity
    {
        public int LeaveRequestStatusID { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

    }
}