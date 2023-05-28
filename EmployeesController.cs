﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Employee_Timesheet.Models;
using Microsoft.IdentityModel.Tokens;

namespace Employee_Timesheet.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly CompanyTimesheetContext _context;

        public EmployeesController(CompanyTimesheetContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var companyTimesheetContext = _context.Employees.Include(e => e.Manager);
            return View(await companyTimesheetContext.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId");
            return View();
        }

        public CompanyTimesheetContext Get_context()
        {
            return _context;
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,EmployeeName,Email,PhoneNumber,Position,DateOfJoining,ManagerId,Password")] Employee employee, CompanyTimesheetContext _context)
        {
            string lastEmployeeId = _context.Employees.Max(e => e.EmployeeId); //Fetching last employee id from employee table
            int nextEmpNumber = int.Parse(lastEmployeeId.Substring(3)) + 1; //incrementing numeric part of employee id to 1
            employee.EmployeeId = "EMP" + nextEmpNumber.ToString("000"); //concatinating incremented number with EMP and assigning to next emp id
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //foreach (var error in ModelState)
            //{
            //    // Print the error message.
            //    Console.WriteLine(error.Value);
            //}
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", employee.ManagerId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", employee.ManagerId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EmployeeId,EmployeeName,Email,PhoneNumber,Position,DateOfJoining,ManagerId,Password")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", employee.ManagerId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'CompanyTimesheetContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(string id)
        {
          return (_context.Employees?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
