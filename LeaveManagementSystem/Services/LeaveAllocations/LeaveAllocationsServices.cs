﻿using AutoMapper;
using LeaveManagementSystem.Models.LeaveAllocations;
using LeaveManagementSystem.Services.UserManager;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.LeaveAllocations
{
    public class LeaveAllocationsServices : ILeaveAllocationsServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public LeaveAllocationsServices(ApplicationDbContext context, IUserService userService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task AllocationLeave(string employeeId)
        {
            var leaveTypes = await _context.LeaveTypes
                .Where(q => !q.LeaveAllocations.Any(x=> x.EmployeeId == employeeId))
                .ToListAsync();
            var currentDate = DateTime.Now;
            var periods = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
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

            await _context.LeaveAllocations
                .Where(e => e.LeaveAllocationId == allocationEditVM.LeaveAllocationId && e.LeaveType.NumberOfDays <= (e.NumberOfDays + allocationEditVM.NumberOfDays))
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
                var currentDate = DateTime.Now;
                var allocations = await _context.LeaveAllocations
                    .Include(q => q.LeaveType)
                    .Include(q => q.Period)
                    .Where(q => q.EmployeeId == userId && q.Period != null && q.Period.EndDate.Year == currentDate.Year)
                    .ToListAsync();

                return allocations;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return new List<LeaveAllocation>();
            }
        }

    }
}