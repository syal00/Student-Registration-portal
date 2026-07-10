using CourseLedger.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseLedger.Controllers;

public class RolesController : Controller
{
    private readonly StudentrecordContext _context;

    public RolesController(StudentrecordContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Roles.ToListAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Role1")] Role role)
    {
        ModelState.Remove("Employees");

        if (await _context.Roles.AnyAsync(r => r.Id == role.Id))
        {
            ModelState.AddModelError("Id", "A role with this ID already exists.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(role);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(role);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Role1")] Role role)
    {
        if (id != role.Id)
        {
            return NotFound();
        }

        ModelState.Remove("Employees");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(role);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(role.Id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(role);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var role = await _context.Roles
            .Include(r => r.Employees)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (role == null)
        {
            return NotFound();
        }

        return View(role);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var role = await _context.Roles
            .Include(r => r.Employees)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (role == null)
        {
            return NotFound();
        }

        role.Employees.Clear();
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RoleExists(int id)
    {
        return _context.Roles.Any(e => e.Id == id);
    }
}
