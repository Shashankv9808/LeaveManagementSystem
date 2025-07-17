namespace LeaveManagementSystem.Models.Periods
{
    public class PeriodVM
    {
        public int PeriodId { get; set; }
        [Display(Name = "Period Name")]
        public string PeriodName { get; set; } = string.Empty;
        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }
    }
}
