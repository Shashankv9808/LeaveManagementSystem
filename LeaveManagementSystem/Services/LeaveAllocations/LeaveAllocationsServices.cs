using AutoMapper;
using LeaveManagementSystem.Models.LeaveAllocations;
using LeaveManagementSystem.Services.Periods;
using LeaveManagementSystem.Services.UserManager;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.LeaveAllocations
{
    public class LeaveAllocationsServices : ILeaveAllocationsServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IPeriodsService _periods;


        public LeaveAllocationsServices(ApplicationDbContext context, IUserService userService, IMapper mapper, IPeriodsService periods)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _periods = periods;
        }

        public async Task AllocationLeave(string employeeId)
        {
            var leaveTypes = await _context.LeaveTypes
                .Where(q => !q.LeaveAllocations.Any(x=> x.EmployeeId == employeeId))
                .ToListAsync();
            var currentDate = DateTime.Now;
            var periods = await _periods.GetCurrentPeriodAsync();
            var monthsRemaining = periods.EndDate.Month - currentDate.Month;

            foreach (var leaveType in leaveTypes)
            {
                var accuralDays = decimal.Divide(leaveType.NumberOfDays, 12);
                var allocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    PeriodId = periods.PeriodId,
                    LeaveTypeID = leaveType.LeaveTypeID,
                    NumberOfDays = (int)Math.Round(accuralDays * monthsRemaining)
                };
                _context.LeaveAllocations.Add(allocation);
            }
            await _context.SaveChangesAsync();
        }

        public async Task EditAllocation(LeaveAllocationEditVM allocationEditVM)
        {
            //Get all the records and update what is required
            //var leaveAllocation = await GetEmployeeAllocation(allocationEditVM.LeaveAllocationId) ?? 
            //    throw new ArgumentNullException("Leave allocation not found.");
            //leaveAllocation.NumberOfDays = allocationEditVM.NumberOfDays;

            //option 1: _context.Update(leaveAllocation);
            //option 2: _context.Entry(leaveAllocation).State = EntityState.Modified;
            //the save cahnges are mandate for option 1 and 2
            //await _context.SaveChangesAsync();

            //var data = await _context.LeaveAllocations.Where(e => e.LeaveAllocationId == 2).FirstOrDefaultAsync();
            //if(data == null)
            //    throw new ArgumentNullException("Leave allocation not found.");
            //if(data.LeaveType.NumberOfDays <= (data.NumberOfDays + allocationEditVM.NumberOfDays))
            //    throw new ArgumentException("The number of days exceeds the maximum allowed for this leave type.");
            await _context.LeaveAllocations
                .Where(e => e.LeaveAllocationId == allocationEditVM.LeaveAllocationId && e.LeaveType.NumberOfDays >= allocationEditVM.NumberOfDays)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.NumberOfDays, allocationEditVM.NumberOfDays));
        }

        public async Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId)
        {
            var allocation = await _context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefaultAsync(q => q.LeaveAllocationId == allocationId);
            return _mapper.Map<LeaveAllocationEditVM>(allocation);
        }

        public async Task<EmployeeLeaveAllocationVM> GetEmployeeAllocations(string? userId)
        {
            var user = string.IsNullOrEmpty(userId)
                ? await _userService.GetCurrentUserAsync()
                : await _userService.GetUserByID(userId);
            var allocations = await GetAllocationByEmployee(user.Id);
            var allocationVMList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);
            var leaveTypes = await _context.LeaveTypes.CountAsync();
            var employeeVM = new EmployeeLeaveAllocationVM
            {
                EmployeeId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                IsCompletedAllocation = allocations.Count >= leaveTypes,
                LeaveAllocations = allocationVMList
            };
            return employeeVM;
        }
        public async Task<List<EmployeeListVM>> GetEmployees()
        {
            var users = await _userService.GetUsersInRoleAsync(Roles.Employee);
            return _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(users);
        }
        private async Task<List<LeaveAllocation>> GetAllocationByEmployee(string? userId)
        {
            try
            {
                var periods = await _periods.GetCurrentPeriodAsync();
                var allocations = await _context.LeaveAllocations
                    .Include(q => q.LeaveType)
                    .Include(q => q.Period)
                    .Where(q => q.EmployeeId == userId && q.PeriodId == periods.PeriodId)
                    .ToListAsync();

                return allocations;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return new List<LeaveAllocation>();
            }
        }
        public async Task<LeaveAllocation> GetCurrentAllocations(int leaveTypeId, string employeeId)
        {
            try
            {
                var periods = await _periods.GetCurrentPeriodAsync();
                var allocations = await _context.LeaveAllocations.FirstAsync(q => q.LeaveTypeID == leaveTypeId && 
                    q.EmployeeId == employeeId && 
                    q.PeriodId == periods.PeriodId);
                return allocations;
            }
            catch (Exception ex)
            {
                return new LeaveAllocation();
            }
        }
    }
}