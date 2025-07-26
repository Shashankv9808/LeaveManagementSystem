
using AutoMapper;
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

        Task<EmployeeLeaveRequestListVM> ILeaveRequestService.AdminGetAllLeaveRequests()
        {
            throw new NotImplementedException();
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

        Task ILeaveRequestService.ReviewLeaveRequest(ReviewLeaveRequestVM model)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM leaveRequestCreateVM)
        {
            var user = await _userService.GetCurrentUserAsync();
            var numberOfDays = leaveRequestCreateVM.EndDate.DayNumber - leaveRequestCreateVM.StartDate.DayNumber;
            var allocationToDeduct = await _context.LeaveAllocations
                .FirstAsync(q => q.LeaveTypeID == leaveRequestCreateVM.LeaveTypeId && q.EmployeeId == user.Id);
            return allocationToDeduct.NumberOfDays < numberOfDays;
        }
    }
}
