using LeaveManagementSystem.Models.LeaveTypes;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.LeaveTypes;

public class LeaveTypesServices : ILeaveTypesServices
{
    private readonly ApplicationDbContext _context;

    public LeaveTypesServices(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<LeaveTypeReadOnlyVM>> GetAllAsync()
    {
        var leaveTypeDM = await _context.LeaveTypes.ToListAsync();
        var leaveTypeROVM = leaveTypeDM.Select(x => new LeaveTypeReadOnlyVM
        {
            ID = x.LeaveTypeID,
            Name = x.LeaveTypeName,
            NumberOfDays = x.NumberOfDays
        });
        return leaveTypeROVM.ToList();
    }

    public async Task<T?> GetT<T>(int id) where T : class, new()
    {
        var data = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.LeaveTypeID == id);
        if (data == null)
        {
            return null;
        }

        var leaveTypeVM = new T();
        if (leaveTypeVM is LeaveTypeReadOnlyVM vm)
        {
            vm.ID = data.LeaveTypeID;
            vm.Name = data.LeaveTypeName;
            vm.NumberOfDays = data.NumberOfDays;
        }
        else if (leaveTypeVM is LeaveTypeCreateVM createVm)
        {
            createVm.Name = data.LeaveTypeName;
            createVm.Days = data.NumberOfDays;
        }
        else if (leaveTypeVM is LeaveTypeEditVM updateVm)
        {
            updateVm.ID = data.LeaveTypeID;
            updateVm.Name = data.LeaveTypeName;
            updateVm.Days = data.NumberOfDays;
        }
        return leaveTypeVM;
    }

    public async Task Remove(int id)
    {
        var leaveType = await _context.LeaveTypes.FindAsync(id);
        if (leaveType != null)
        {
            _context.LeaveTypes.Remove(leaveType);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Update(LeaveTypeEditVM leaveTypeVM)
    {
        var leaveTypeDM = new LeaveType
        {
            LeaveTypeID = leaveTypeVM.ID,
            LeaveTypeName = leaveTypeVM.Name,
            NumberOfDays = leaveTypeVM.Days
        };
        _context.Update(leaveTypeDM);
        await _context.SaveChangesAsync();
    }
    public async Task Create(LeaveTypeCreateVM leaveTypeVM)
    {
        var leaveTypeDM = new LeaveType
        {
            LeaveTypeName = leaveTypeVM.Name,
            NumberOfDays = leaveTypeVM.Days
        };
        _context.Add(leaveTypeDM);
        await _context.SaveChangesAsync();
    }

    public bool LeaveTypeExists(int id)
    {
        return _context.LeaveTypes.Any(e => e.LeaveTypeID == id);
    }
    public async Task<bool> CheckIfLeaveTypeNameExists(string name)
    {
        return await _context.LeaveTypes.AnyAsync(e => e.LeaveTypeName.ToLower().Equals(name.ToLower()));
    }
    public async Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeVM)
    {
        return await _context.LeaveTypes.AnyAsync(e => e.LeaveTypeName.ToLower().Equals(leaveTypeVM.Name.ToLower()) && leaveTypeVM.ID != e.LeaveTypeID);
    }
    public async Task<bool> DaysExceedMaximum(int leaveTypeId, int days)
    {
        var leaveType = await _context.LeaveTypes.FindAsync(leaveTypeId);
        if (leaveType == null)
        {
            return false; // Leave type not found
        }
        return days > leaveType.NumberOfDays;
    }
}
