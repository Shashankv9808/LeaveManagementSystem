using LeaveManagementSystem.Models.LeaveTypes;

namespace LeaveManagementSystem.Services.LeaveTypes
{
    public interface ILeaveTypesServices
    {
        Task<bool> CheckIfLeaveTypeNameExists(string name);
        Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeVM);
        Task Create(LeaveTypeCreateVM leaveTypeVM);
        Task<List<LeaveTypeReadOnlyVM>> GetAllAsync();
        Task<T?> GetT<T>(int id) where T : class, new();
        bool LeaveTypeExists(int id);
        Task Remove(int id);
        Task Update(LeaveTypeEditVM leaveTypeVM);
        Task<bool> DaysExceedMaximum(int leaveTypeId, int days);
    }
}