using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabAssignment6.DataAccess;

namespace LabAssignment6.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly StudentrecordContext _context;

        public EmployeesController(StudentrecordContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.Roles)
                .ToListAsync();
            return View(employees);
        }


        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewBag.Roles = _context.Roles.ToList();
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UserName,Password")] Employee employee, int[] selectedRoles)
        {
            // Remove ModelState validation for navigation properties
            ModelState.Remove("Roles");

            // Validate Username uniqueness
            if (!string.IsNullOrWhiteSpace(employee.UserName))
            {
                var existingUser = await _context.Employees
                    .FirstOrDefaultAsync(e => e.UserName == employee.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "Network ID must be unique.");
                }
            }

            // Validate that at least one role is selected
            if (selectedRoles == null || selectedRoles.Length == 0)
            {
                ModelState.AddModelError("Roles", "Employee must have at least one role.");
            }

            if (ModelState.IsValid)
            {
                if (selectedRoles != null)
                {
                    foreach (var roleId in selectedRoles)
                    {
                        var role = await _context.Roles.FindAsync(roleId);
                        if (role != null)
                        {
                            employee.Roles.Add(role);
                        }
                    }
                }
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Roles = _context.Roles.ToList();
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Roles)
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (employee == null)
            {
                return NotFound();
            }
            
            ViewBag.Roles = _context.Roles.ToList();
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserName,Password")] Employee employee, int[] selectedRoles)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            // Remove ModelState validation for navigation properties
            ModelState.Remove("Roles");

            // Validate Username uniqueness (excluding current employee)
            if (!string.IsNullOrWhiteSpace(employee.UserName))
            {
                var existingUser = await _context.Employees
                    .FirstOrDefaultAsync(e => e.UserName == employee.UserName && e.Id != id);
                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "Network ID must be unique.");
                }
            }

            // Validate that at least one role is selected
            if (selectedRoles == null || selectedRoles.Length == 0)
            {
                ModelState.AddModelError("Roles", "Employee must have at least one role.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEmployee = await _context.Employees
                        .Include(e => e.Roles)
                        .FirstOrDefaultAsync(e => e.Id == id);

                    if (existingEmployee != null)
                    {
                        existingEmployee.Name = employee.Name;
                        existingEmployee.UserName = employee.UserName;
                        existingEmployee.Password = employee.Password;

                        // Update roles
                        existingEmployee.Roles.Clear();
                        if (selectedRoles != null)
                        {
                            foreach (var roleId in selectedRoles)
                            {
                                var role = await _context.Roles.FindAsync(roleId);
                                if (role != null)
                                {
                                    existingEmployee.Roles.Add(role);
                                }
                            }
                        }

                        _context.Update(existingEmployee);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            ViewBag.Roles = _context.Roles.ToList();
            return View(employee);
        }


        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
