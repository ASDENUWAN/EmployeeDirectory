using EmployeeDirectory.Data;
using EmployeeDirectory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDirectory.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EmployeesController(ApplicationDbContext db) => _db = db;

        // GET: Employees
[Authorize]
public async Task<IActionResult> Index(string searchString)
{
    // Preserve the current search term in ViewData so the view can re‑populate the textbox
    ViewData["CurrentFilter"] = searchString;

    // Start with the full employee set
    var employees = from e in _db.Employees
                    select e;

    // If the user typed something, filter by name or department
    if (!string.IsNullOrWhiteSpace(searchString))
    {
        employees = employees.Where(e =>
            EF.Functions.Like(e.FullName, $"%{searchString}%")
         || EF.Functions.Like(e.Department, $"%{searchString}%"));
    }

    // Execute and pass to the view
    return View(await employees.ToListAsync());
}

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var emp = await _db.Employees.FindAsync(id);
            if (emp == null) return NotFound();
            return View(emp);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Employee m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.Employees.Add(m);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var emp = await _db.Employees.FindAsync(id);
            if (emp == null) return NotFound();
            return View(emp);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Employee m)
        {
            if (id != m.EmployeeId) return BadRequest();
            if (!ModelState.IsValid) return View(m);

            _db.Update(m);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var emp = await _db.Employees.FindAsync(id);
            if (emp == null) return NotFound();
            return View(emp);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emp = await _db.Employees.FindAsync(id);
            _db.Employees.Remove(emp);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
