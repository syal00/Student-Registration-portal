using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabAssignment6.DataAccess;
using LabAssignment6.Models;

namespace LabAssignment6.Controllers
{
    public class AcademicRecordsController : Controller
    {
        private readonly StudentrecordContext _context;

        public AcademicRecordsController(StudentrecordContext context)
        {
            _context = context;
        }

        // GET: AcademicRecords
        public IActionResult Index(string sortOrder)
        {
            ViewData["CourseSortParam"] = string.IsNullOrEmpty(sortOrder) ? "course_desc" : "";
            ViewData["StudentSortParam"] = sortOrder == "student" ? "student_desc" : "student";
            ViewData["CurrentSort"] = sortOrder;

            var records = _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .ToList();

            // Apply sorting
            var comparer = new AcademicRecordComparer(
                sortOrder switch
                {
                    "course_desc" => "course",
                    "student" => "student",
                    "student_desc" => "student",
                    _ => "course"
                },
                sortOrder != "course_desc" && sortOrder != "student_desc"
            );

            records.Sort(comparer);

            return View(records);
        }


        // GET: AcademicRecords/Create
        public IActionResult Create()
        {
            ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id");
            return View();
        }

        // POST: AcademicRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseCode,StudentId,Grade")] AcademicRecord academicRecord)
        {
            // Remove ModelState validation for null fields
            ModelState.Remove("CourseCodeNavigation");
            ModelState.Remove("Student");
            
            // Check if record already exists
            var existingRecord = await _context.AcademicRecords
                .FirstOrDefaultAsync(m => m.StudentId == academicRecord.StudentId && m.CourseCode == academicRecord.CourseCode);
            
            if (existingRecord != null)
            {
                ModelState.AddModelError("", "An academic record for this student and course already exists.");
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(academicRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code", academicRecord.CourseCode);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", academicRecord.StudentId);
            return View(academicRecord);
        }

        // GET: AcademicRecords/Edit
        public async Task<IActionResult> Edit(string studentId, string courseCode)
        {
            if (studentId == null || courseCode == null)
            {
                return NotFound();
            }

            var academicRecord = await _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.StudentId == studentId && m.CourseCode == courseCode);
            
            if (academicRecord == null)
            {
                return NotFound();
            }
            
            return View(academicRecord);
        }

        // POST: AcademicRecords/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string studentId, string courseCode, [Bind("CourseCode,StudentId,Grade")] AcademicRecord academicRecord)
        {
            if (studentId != academicRecord.StudentId || courseCode != academicRecord.CourseCode)
            {
                return NotFound();
            }

            // Remove ModelState validation for null fields
            ModelState.Remove("CourseCodeNavigation");
            ModelState.Remove("Student");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRecord = await _context.AcademicRecords
                        .FirstOrDefaultAsync(m => m.StudentId == studentId && m.CourseCode == courseCode);
                    
                    if (existingRecord != null)
                    {
                        existingRecord.Grade = academicRecord.Grade;
                        _context.Update(existingRecord);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicRecordExists(studentId, courseCode))
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
            return View(academicRecord);
        }

        // GET: AcademicRecords/EditAll
        public IActionResult EditAll(string sortOrder)
        {
            ViewData["CourseSortParam"] = string.IsNullOrEmpty(sortOrder) ? "course_desc" : "";
            ViewData["StudentSortParam"] = sortOrder == "student" ? "student_desc" : "student";
            ViewData["CurrentSort"] = sortOrder;

            var records = _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .ToList();

            // Apply sorting
            var comparer = new AcademicRecordComparer(
                sortOrder switch
                {
                    "course_desc" => "course",
                    "student" => "student",
                    "student_desc" => "student",
                    _ => "course"
                },
                sortOrder != "course_desc" && sortOrder != "student_desc"
            );

            records.Sort(comparer);

            return View(records);
        }

        // POST: AcademicRecords/EditAll
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAll(IFormCollection form, string sortOrder)
        {
            var allRecords = await _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .ToListAsync();

            // Apply sorting
            ViewData["CourseSortParam"] = string.IsNullOrEmpty(sortOrder) ? "course_desc" : "";
            ViewData["StudentSortParam"] = sortOrder == "student" ? "student_desc" : "student";
            ViewData["CurrentSort"] = sortOrder;

            var comparer = new AcademicRecordComparer(
                sortOrder switch
                {
                    "course_desc" => "course",
                    "student" => "student",
                    "student_desc" => "student",
                    _ => "course"
                },
                sortOrder != "course_desc" && sortOrder != "student_desc"
            );

            allRecords.Sort(comparer);

            bool hasErrors = false;

            // Process form data - look for [index].StudentId, [index].CourseCode, and [index].Grade
            int index = 0;
            while (form.ContainsKey($"[{index}].StudentId") && form.ContainsKey($"[{index}].CourseCode"))
            {
                var studentId = form[$"[{index}].StudentId"].ToString();
                var courseCode = form[$"[{index}].CourseCode"].ToString();
                var gradeValue = form[$"[{index}].Grade"].ToString();

                var record = allRecords.FirstOrDefault(r => r.StudentId == studentId && r.CourseCode == courseCode);
                if (record != null)
                {
                    if (string.IsNullOrWhiteSpace(gradeValue))
                    {
                        record.Grade = null;
                    }
                    else if (int.TryParse(gradeValue, out int grade))
                    {
                        // Validate grade range
                        if (grade < 0 || grade > 100)
                        {
                            ModelState.AddModelError($"[{index}].Grade", "Grade must be between 0 and 100.");
                            hasErrors = true;
                        }
                        else
                        {
                            record.Grade = grade;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError($"[{index}].Grade", "Grade must be a valid number.");
                        hasErrors = true;
                    }
                }
                index++;
            }

            if (hasErrors)
            {
                return View(allRecords);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: AcademicRecords/Delete
        public async Task<IActionResult> Delete(string studentId, string courseCode)
        {
            if (studentId == null || courseCode == null)
            {
                return NotFound();
            }

            var academicRecord = await _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.StudentId == studentId && m.CourseCode == courseCode);
            
            if (academicRecord == null)
            {
                return NotFound();
            }
            
            return View(academicRecord);
        }

        // POST: AcademicRecords/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string studentId, string courseCode)
        {
            var academicRecord = await _context.AcademicRecords
                .FirstOrDefaultAsync(m => m.StudentId == studentId && m.CourseCode == courseCode);
            
            if (academicRecord != null)
            {
                _context.AcademicRecords.Remove(academicRecord);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AcademicRecordExists(string studentId, string courseCode)
        {
            return _context.AcademicRecords.Any(e => e.StudentId == studentId && e.CourseCode == courseCode);
        }
    }
}
