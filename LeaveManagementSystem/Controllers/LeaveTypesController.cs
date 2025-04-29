using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models.LeaveTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Controllers
{
    public class LeaveTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            var leaveTypeDM = await _context.LeaveTypes.ToListAsync();
            var leaveTypeROVM = leaveTypeDM.Select(x => new LeaveTypeReadOnlyVM
            {
                ID = x.LeaveTypeID,
                Name = x.LeaveTypeName,
                NumberOfDays = x.NumberOfDays
            });
            return View(leaveTypeROVM);
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveTypeDM = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.LeaveTypeID == id);
            if (leaveTypeDM == null)
            {
                return NotFound();
            }
            var leaveTypeROVM = new LeaveTypeReadOnlyVM
            {
                ID = leaveTypeDM.LeaveTypeID,
                Name = leaveTypeDM.LeaveTypeName,
                NumberOfDays = leaveTypeDM.NumberOfDays
            };
            return View(leaveTypeROVM);
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
            if (ModelState.IsValid)
            {
                var leaveTypeDM = new LeaveType
                {
                    LeaveTypeName = leaveTypeVM.Name,
                    NumberOfDays = leaveTypeVM.Days
                };
                _context.Add(leaveTypeDM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeVM);
        }

        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveTypeDM = await _context.LeaveTypes.FindAsync(id);
            if (leaveTypeDM == null)
            {
                return NotFound();
            }
            var leaveTypeVM = new LeaveTypeEditVM
            {
                ID = leaveTypeDM.LeaveTypeID,
                Name = leaveTypeDM.LeaveTypeName,
                Days = leaveTypeDM.NumberOfDays
            };
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

            if (ModelState.IsValid)
            {
                try
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveTypeExists(leaveTypeVM.ID))
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

            var leaveTypeDM = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.LeaveTypeID == id);
            if (leaveTypeDM == null)
            {
                return NotFound();
            }
            var leaveTypeVM = new LeaveTypeReadOnlyVM
            {
                ID = leaveTypeDM.LeaveTypeID,
                Name = leaveTypeDM.LeaveTypeName,
                NumberOfDays = leaveTypeDM.NumberOfDays
            };
            return View(leaveTypeVM);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType != null)
            {
                _context.LeaveTypes.Remove(leaveType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.LeaveTypeID == id);
        }
    }
}
