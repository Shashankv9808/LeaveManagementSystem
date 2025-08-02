using LeaveManagementSystem.Models.LeaveAllocations;

namespace LeaveManagementSystem.Services.LeaveAllocations
{
    public interface ILeaveAllocationsServices
    {
        Task AllocationLeave(string employeeId);
        Task<EmployeeLeaveAllocationVM> GetEmployeeAllocations(string? userId);
        Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId);
        Task<List<EmployeeListVM>> GetEmployees();
        Task EditAllocation(LeaveAllocationEditVM allocationEditVM);
        Task<LeaveAllocation> GetCurrentAllocations(int leaveTypeId, string employeeId);
    }
}
