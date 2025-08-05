using LeaveManagementSystem.Models.LeaveTypes;
using LeaveManagementSystem.Services.LeaveTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class LeaveTypesController(ILeaveTypesServices _leaveTypesServices, ILogger<LeaveTypesController> _logger) : Controller
    {

        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index action called at {Time}", DateTime.UtcNow);
            return View(await _leaveTypesServices.GetAllAsync());
        }

        // GET: LeaveTypes/Details/5    
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var leaveType = await _leaveTypesServices.GetT<LeaveTypeReadOnlyVM>(id.Value);
            if (id == null)
            {
                _logger.LogWarning("LeaveType with ID {Id} not found", id);
                return NotFound();
            }
            return View(leaveType);
        }

        // GET: LeaveTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeVM)
        {
            _logger.LogInformation("Create action called at {Time} with LeaveType: {@LeaveType}", DateTime.UtcNow, leaveTypeVM);
            if (await _leaveTypesServices.CheckIfLeaveTypeNameExists(leaveTypeVM.Name))
            {
                _logger.LogWarning("Leave Type Name {Name} already exists", leaveTypeVM.Name);
                ModelState.AddModelError(nameof(leaveTypeVM.Name), "Leave Type Name already exists");
            }
            if (ModelState.IsValid)
            {
                await _leaveTypesServices.Create(leaveTypeVM);
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError("ModelState is invalid for LeaveType: {@LeaveType}", leaveTypeVM);
            return View(leaveTypeVM);
        }

        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var leaveTypeVM = await _leaveTypesServices.GetT<LeaveTypeEditVM>(id.Value);
            if (leaveTypeVM == null)
            {
                return NotFound();
            }
            return View(leaveTypeVM);
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveTypeVM)
        {
            if (id != leaveTypeVM.ID)
            {
                return NotFound();
            }

            if (await _leaveTypesServices.CheckIfLeaveTypeNameExistsForEdit(leaveTypeVM))
            {
                ModelState.AddModelError(nameof(leaveTypeVM.Name), "Leave Type Name already exists");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _leaveTypesServices.Update(leaveTypeVM);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_leaveTypesServices.LeaveTypeExists(leaveTypeVM.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeVM);
        }

        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var leaveTypeVM = await _leaveTypesServices.GetT<LeaveTypeReadOnlyVM>(id.Value);
            if (leaveTypeVM == null)
            {
                return NotFound();
            }
            return View(leaveTypeVM);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _leaveTypesServices.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
