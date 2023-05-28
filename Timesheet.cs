using System;
using System.Collections.Generic;

namespace Employee_Timesheet.Models;

public partial class Timesheet
{
    public string TimesheetId { get; set; } = null!;

    public string EmployeeId { get; set; } = null!;

    public string TaskId { get; set; } = null!;

    public DateTime? Date { get; set; }

    public string? Type { get; set; }

    public decimal? TotalHoursWorked { get; set; }

    public bool? Status { get; set; }

    public virtual Employee? Employee { get; set; } 

    public virtual Task? Task { get; set; } 
}
