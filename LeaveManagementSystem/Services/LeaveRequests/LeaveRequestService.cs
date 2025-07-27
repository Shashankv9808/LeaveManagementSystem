
using AutoMapper;
using LeaveManagementSystem.Models.LeaveAllocations;
using LeaveManagementSystem.Models.LeaveRequests;
using LeaveManagementSystem.Services.UserManager;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.LeaveRequests
{
    public class LeaveRequestService(IMapper _mapper, IUserService _userService, ApplicationDbContext _context) : ILeaveRequestService
    {
        Task ILeaveRequestService.CancelLeaveRequest(int leaveRequestId)
        {
            try
            {
                var leaveRequest = _context.LeaveRequests.FirstOrDefault(q => q.LeaveRequestID == leaveRequestId);
                if (leaveRequest == null)
                {
                    throw new InvalidOperationException("Leave request not found");
                }
                leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Cancelled;
                var user = _userService.GetCurrentUserAsync().Result;
                var allocationToUpdate = _context.LeaveAllocations
                    .FirstOrDefault(q => q.LeaveTypeID == leaveRequest.LeaveTypeId && q.EmployeeId == user.Id);
                if (allocationToUpdate != null)
                {
                    var numberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;
                    allocationToUpdate.NumberOfDays += numberOfDays;
                }
                else
                {
                    throw new InvalidOperationException("Leave Allocation is not found.");
                }
                return _context.SaveChangesAsync();
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
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            var allocationToDeduct = await _context.LeaveAllocations
                .FirstAsync(q => q.LeaveTypeID == model.LeaveTypeId && q.EmployeeId == user.Id);
            allocationToDeduct.NumberOfDays -= numberOfDays;
            await _context.SaveChangesAsync();

        }

        async Task<EmployeeLeaveRequestListVM> ILeaveRequestService.AdminGetAllLeaveRequests()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .ToListAsync();
            var leaveRequestModel = leaveRequests.Select(leaveRequest => new LeaveReqestReadOnlyVM
            {
                LeaveRequestID = leaveRequest.LeaveRequestID,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                LeaveType = leaveRequest.LeaveType.LeaveTypeName,
                LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequest.LeaveRequestStatusId,
                NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber,
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
                var allocationToUpdate = await _context.LeaveAllocations
                    .FirstAsync(q => q.LeaveTypeID == leaveRequest.LeaveTypeId && q.EmployeeId == leaveRequest.EmployeeId);
                var numberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;
                allocationToUpdate.NumberOfDays += numberOfDays;
            }
            await _context.SaveChangesAsync();
        }
        public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM leaveRequestCreateVM)
        {
            var user = await _userService.GetCurrentUserAsync();
            var numberOfDays = leaveRequestCreateVM.EndDate.DayNumber - leaveRequestCreateVM.StartDate.DayNumber;
            var allocationToDeduct = await _context.LeaveAllocations
                .FirstAsync(q => q.LeaveTypeID == leaveRequestCreateVM.LeaveTypeId && q.EmployeeId == user.Id);
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
    }
}
