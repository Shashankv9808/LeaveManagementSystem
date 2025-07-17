using LeaveManagementSystem.Models.LeaveAllocations;
using LeaveManagementSystem.Services.LeaveAllocations;
using LeaveManagementSystem.Services.LeaveTypes;
using Microsoft.AspNetCore.Authorization;

namespace LeaveManagementSystem.Controllers
{
    [Authorize]
    public class LeaveAllocationsController : Controller
    {
        private readonly ILeaveAllocationsServices _leaveAllocationsServices;
        private readonly ILeaveTypesServices _leaveTypesServices;
        public LeaveAllocationsController(ILeaveAllocationsServices leaveAllocationsServices,ILeaveTypesServices leaveTypesServices)
        {
            _leaveAllocationsServices = leaveAllocationsServices;
            _leaveTypesServices = leaveTypesServices;
        }
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Index()
        {
            var employees = await _leaveAllocationsServices.GetEmployees();
            if (employees == null)
                return NotFound();
            return View(employees);
        }
        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateLeave(string? EmployeeId)
        {
            if (string.IsNullOrEmpty(EmployeeId))
                return BadRequest("User ID cannot be null or empty.");

            await _leaveAllocationsServices.AllocationLeave(EmployeeId);
            return RedirectToAction(nameof(Details), new {userId = EmployeeId });
        }
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> EditAllocation(int? id)
        {
            if (id == null)
                return NotFound();
                var leaveAllocation = await _leaveAllocationsServices.GetEmployeeAllocation(id.Value);
            if (leaveAllocation == null)
                return NotFound();
            return View(leaveAllocation);
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM allocation)
        {
            if(await _leaveTypesServices.DaysExceedMaximum(allocation.LeaveType.ID, allocation.NumberOfDays))
            {
                ModelState.AddModelError("NumberOfDays", "The number of days exceeds the maximum allowed for this leave type.");
            }
            if(ModelState.IsValid)
            {
                await _leaveAllocationsServices.EditAllocation(allocation);
                return RedirectToAction(nameof(Details), new { userId = allocation.Employee.EmployeeId });
            }

            var days = allocation.NumberOfDays;
            allocation = await _leaveAllocationsServices.GetEmployeeAllocation(allocation.LeaveAllocationId);
            allocation.NumberOfDays = days; // Preserve the number of days entered by the user
            return View(allocation);

        }

        public async Task<IActionResult> Details(string? userId)
        {
            var leaveAllocations = await _leaveAllocationsServices.GetEmployeeAllocations(userId);
            if (leaveAllocations == null)
                return NotFound();
            return View(leaveAllocations);
        }
    }
}
