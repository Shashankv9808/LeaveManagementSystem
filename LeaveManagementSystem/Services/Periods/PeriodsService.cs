using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.Periods
{
    public class PeriodsService(ApplicationDbContext _context) : IPeriodsService
    {
        public async Task<Period> GetCurrentPeriodAsync()
        {
            var currentDate = DateTime.Now.Date;
            var period = await _context.Periods.FirstOrDefaultAsync(q => q.EndDate.Year == currentDate.Year);
            if (period == null)
            {
                throw new InvalidOperationException("Current period not found.");
            }
            return period;
        }
    }
}
