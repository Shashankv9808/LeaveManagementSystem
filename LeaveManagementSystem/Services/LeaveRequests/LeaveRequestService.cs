
using AutoMapper;
using LeaveManagementSystem.Models.LeaveAllocations;
using LeaveManagementSystem.Models.LeaveRequests;
using LeaveManagementSystem.Services.LeaveAllocations;
using LeaveManagementSystem.Services.UserManager;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.LeaveRequests
{
    public class LeaveRequestService(IMapper _mapper, IUserService _userService, ApplicationDbContext _context, ILeaveAllocationsServices _leaveAllocationsServices) : ILeaveRequestService
    {
        async Task ILeaveRequestService.CancelLeaveRequest(int leaveRequestId)
        {
            try
            {
                var leaveRequest = _context.LeaveRequests.FirstOrDefault(q => q.LeaveRequestID == leaveRequestId);
                if (leaveRequest == null)
                {
                    throw new InvalidOperationException("Leave request not found");
                }
                leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Cancelled;
                await UpdateAllocationDays(leaveRequest, false);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while canceling the leave request: {ex.Message}");
            }
        }

        async Task ILeaveRequestService.CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(model);
            var user = await _userService.GetCurrentUserAsync();
            leaveRequest.EmployeeId = user?.Id ?? throw new InvalidOperationException("User not found");
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Pending;
            _context.LeaveRequests.Add(leaveRequest);
            await UpdateAllocationDays(leaveRequest, true);
            await _context.SaveChangesAsync();

        }

        async Task<EmployeeLeaveRequestListVM> ILeaveRequestService.AdminGetAllLeaveRequests()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .ToListAsync();
            var leaveRequestModel = leaveRequests.Select(leaveRequest => new LeaveReqestReadOnlyVM
            {
                LeaveRequestID = leaveRequest.LeaveRequestID,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                LeaveType = leaveRequest.LeaveType.LeaveTypeName,
                LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequest.LeaveRequestStatusId,
                NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber,
                Employee = new EmployeeListVM
                {
                    EmployeeId = leaveRequest.EmployeeId,
                    FirstName = leaveRequest.Employee.FirstName ?? string.Empty,
                    LastName = leaveRequest.Employee.LastName ?? string.Empty,
                    Email = leaveRequest.Employee.Email ?? string.Empty
                }
            }).ToList();
            var model = new EmployeeLeaveRequestListVM
            {
                ApprovedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved),
                PendingRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Pending),
                RejectedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Rejected),
                TotalRequests = leaveRequests.Count,
                LeaveRequests = leaveRequestModel
            };
            return model;
        }
        async Task<List<LeaveReqestReadOnlyVM>> ILeaveRequestService.GetEmployeeLeaveRequests()
        {
            var user = await _userService.GetCurrentUserAsync();
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .Where(q => q.EmployeeId == user.Id)
                .ToListAsync();
            var model = leaveRequests.Select(leaveRequests => new LeaveReqestReadOnlyVM
            {
                LeaveRequestID = leaveRequests.LeaveRequestID,
                StartDate = leaveRequests.StartDate,
                EndDate = leaveRequests.EndDate,
                LeaveType = leaveRequests.LeaveType.LeaveTypeName,
                LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequests.LeaveRequestStatusId,
                NumberOfDays = leaveRequests.EndDate.DayNumber - leaveRequests.StartDate.DayNumber,
            }).ToList();
            return model;
        }

        async Task ILeaveRequestService.ReviewLeaveRequest(int leaveRequestId, bool approved)
        {
            var leaveRequest = _context.LeaveRequests
                .FirstOrDefault(q => q.LeaveRequestID == leaveRequestId);
            if (leaveRequest == null)
            {
                throw new InvalidOperationException("Leave request not found");
            }
            leaveRequest.LeaveRequestStatusId = approved ? (int)LeaveRequestStatusEnum.Approved : (int)LeaveRequestStatusEnum.Rejected;
            var user = await _userService.GetCurrentUserAsync();
            leaveRequest.ReviewerId = user?.Id ?? throw new InvalidOperationException("User not found");
            if (!approved)
            {
                await UpdateAllocationDays(leaveRequest, false);
            }
            await _context.SaveChangesAsync();
        }
        public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM leaveRequestCreateVM)
        {
            var user = await _userService.GetCurrentUserAsync();
            var numberOfDays = leaveRequestCreateVM.EndDate.DayNumber - leaveRequestCreateVM.StartDate.DayNumber;
            var currentDate = DateTime.Now.Date;
            var period = await _context.Periods.FirstOrDefaultAsync(q => q.EndDate.Year == currentDate.Year);
            var allocationToDeduct = await _context.LeaveAllocations
                .FirstAsync(q => q.LeaveTypeID == leaveRequestCreateVM.LeaveTypeId && q.EmployeeId == user.Id && period.PeriodId == period.PeriodId);
            return allocationToDeduct.NumberOfDays < numberOfDays;
        }

        public Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int leaveRequestId)
        {
            var leaveRequest = _context.LeaveRequests
                .Include(q => q.LeaveType)
                .FirstOrDefault(q => q.LeaveRequestID == leaveRequestId);
            if (leaveRequest == null)
            {
                throw new InvalidOperationException("Leave request not found");
            }
            var user = _userService.GetUserByID(leaveRequest.EmployeeId).Result;
            if (user == null)
            {
                throw new InvalidOperationException("Logged in user not found");
            }
            var model = new ReviewLeaveRequestVM
            {
                LeaveRequestID = leaveRequest.LeaveRequestID,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                LeaveType = leaveRequest.LeaveType.LeaveTypeName,
                LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequest.LeaveRequestStatusId,
                RequestComments = leaveRequest.RequestComments ?? string.Empty,
                Employee = new EmployeeListVM
                {
                    EmployeeId = leaveRequest.EmployeeId,
                    Email = user.Email ?? string.Empty,
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty,

                }
            };
            return Task.FromResult(model);
        }
        private async Task UpdateAllocationDays(LeaveRequest leaveRequest, bool deductDays)
        {
            var allocation = await _leaveAllocationsServices.GetCurrentAllocations(leaveRequest.LeaveTypeId, leaveRequest.EmployeeId);
            var numberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;
            if (deductDays)
            {
                allocation.NumberOfDays -= numberOfDays;
            }
            else
            {
                allocation.NumberOfDays += numberOfDays;
            }
            _context.Entry(allocation).State = EntityState.Modified;
        }
    }
}
