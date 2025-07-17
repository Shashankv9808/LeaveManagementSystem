namespace LeaveManagementSystem.Data
{
    public class Period : BaseEntity
    {
        [Key]
        public int PeriodId { get; set; }
        public required string PeriodName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

    }
}
