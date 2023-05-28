using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Employee_Timesheet.Models;

namespace Employee_Timesheet.Controllers
{
    public class TimesheetsController : Controller
    {
        private readonly CompanyTimesheetContext _context;

        public TimesheetsController(CompanyTimesheetContext context)
        {
            _context = context;
        }

        // GET: Timesheets
        public async Task<IActionResult> Index()
        {
            var companyTimesheetContext = _context.Timesheets.Include(t => t.Employee).Include(t => t.Task);
            return View(await companyTimesheetContext.ToListAsync());
        }

        // GET: Timesheets/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Timesheets == null)
            {
                return NotFound();
            }

            var timesheet = await _context.Timesheets
                .Include(t => t.Employee)
                .Include(t => t.Task)
                .FirstOrDefaultAsync(m => m.TimesheetId == id);
            if (timesheet == null)
            {
                return NotFound();
            }

            return View(timesheet);
        }

        // GET: Timesheets/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId");
            ViewData["TaskId"] = new SelectList(_context.Tasks, "TaskId", "TaskId");
            return View();
        }

        // POST: Timesheets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TimesheetId,EmployeeId,TaskId,Date,Type,TotalHoursWorked,Status")] Timesheet timesheet)
        {

            string lastTimesheetId = _context.Timesheets.Max(e => e.TimesheetId); //Fetching last manager id from manager table
            int nextEmpNumber = int.Parse(lastTimesheetId.Substring(3)) + 1; //incrementing numeric part of manager id to 1
            timesheet.TimesheetId = "TST" + nextEmpNumber.ToString("000"); //concatinating incremented number with MGR and assigning to next manager id
            if (ModelState.IsValid)
            {
                _context.Add(timesheet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", timesheet.EmployeeId);
            ViewData["TaskId"] = new SelectList(_context.Tasks, "TaskId", "TaskId", timesheet.TaskId);
            return View(timesheet);
        }

        // GET: Timesheets/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Timesheets == null)
            {
                return NotFound();
            }

            var timesheet = await _context.Timesheets.FindAsync(id);
            if (timesheet == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", timesheet.EmployeeId);
            ViewData["TaskId"] = new SelectList(_context.Tasks, "TaskId", "TaskId", timesheet.TaskId);
            return View(timesheet);
        }

        // POST: Timesheets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TimesheetId,EmployeeId,TaskId,Date,Type,TotalHoursWorked,Status")] Timesheet timesheet)
        {
            if (id != timesheet.TimesheetId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timesheet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimesheetExists(timesheet.TimesheetId))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", timesheet.EmployeeId);
            ViewData["TaskId"] = new SelectList(_context.Tasks, "TaskId", "TaskId", timesheet.TaskId);
            return View(timesheet);
        }

        // GET: Timesheets/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Timesheets == null)
            {
                return NotFound();
            }

            var timesheet = await _context.Timesheets
                .Include(t => t.Employee)
                .Include(t => t.Task)
                .FirstOrDefaultAsync(m => m.TimesheetId == id);
            if (timesheet == null)
            {
                return NotFound();
            }

            return View(timesheet);
        }

        // POST: Timesheets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Timesheets == null)
            {
                return Problem("Entity set 'CompanyTimesheetContext.Timesheets'  is null.");
            }
            var timesheet = await _context.Timesheets.FindAsync(id);
            if (timesheet != null)
            {
                _context.Timesheets.Remove(timesheet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimesheetExists(string id)
        {
          return (_context.Timesheets?.Any(e => e.TimesheetId == id)).GetValueOrDefault();
        }
    }
}
