using System;
using System.Collections.Generic;

namespace Employee_Timesheet.Models;

public partial class Project
{
    public string ProjectId { get; set; } = null!;

    public string ProjectName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
