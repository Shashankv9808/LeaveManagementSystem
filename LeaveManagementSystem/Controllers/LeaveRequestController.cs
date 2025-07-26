using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models.LeaveRequests;
using LeaveManagementSystem.Services.LeaveRequests;
using LeaveManagementSystem.Services.LeaveTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Controllers
{
    [Authorize]
    public class LeaveRequestController(ILeaveTypesServices _leaveTypesServices, ILeaveRequestService _leaveRequestService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var leaveRequests = await _leaveRequestService.GetEmployeeLeaveRequests();
            return View(leaveRequests);
        }
        public async Task<IActionResult> Create()
        {
            var leaveTypes = await _leaveTypesServices.GetAllAsync();
            var leaveTypesSelectList = new SelectList(leaveTypes, "ID", "Name");
            var leaveRequestCreateVM = new LeaveRequestCreateVM
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                LeaveTypes = leaveTypesSelectList
            };
            return View(leaveRequestCreateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestCreateVM leaveRequestCreateVM)
        {
            if (await _leaveRequestService.RequestDatesExceedAllocation(leaveRequestCreateVM))
            {
                ModelState.AddModelError(string.Empty, "You have exceeded your allocation.");
                ModelState.AddModelError(nameof(leaveRequestCreateVM.EndDate), "The requested dates exceed your leave allocation.");
            }
            if (ModelState.IsValid)
            {
                await _leaveRequestService.CreateLeaveRequest(leaveRequestCreateVM);
                return RedirectToAction(nameof(Index));
            }
            var leaveTypes = await _leaveTypesServices.GetAllAsync();
            leaveRequestCreateVM.LeaveTypes = new SelectList(leaveTypes, "ID", "Name");
            return View(leaveRequestCreateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int leaveRequestId)
        {
            try
            {
                await _leaveRequestService.CancelLeaveRequest(leaveRequestId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while processing your request: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ListRequests()
        {
            return View();
        }
        public async Task<IActionResult> Review(int leaveRequestId)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review()
        {
            return View();
        }
    }
}
