using CourseLedger.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseLedger.Controllers;

public class StudentsController : Controller
{
    private readonly StudentrecordContext _context;

    public StudentsController(StudentrecordContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Students.ToListAsync());
    }

    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] Student student)
    {
        if (ModelState.IsValid)
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }

    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }
        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] Student student)
    {
        if (id != student.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.Id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }

    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var student = await _context.Students
            .Include(s => s.AcademicRecords)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
        {
            return NotFound();
        }

        if (student.AcademicRecords.Any())
        {
            _context.AcademicRecords.RemoveRange(student.AcademicRecords);
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StudentExists(string id)
    {
        return _context.Students.Any(e => e.Id == id);
    }
}
