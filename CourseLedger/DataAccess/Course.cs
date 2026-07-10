using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace CourseLedger.DataAccess;

public partial class Course
{
    public string Code { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? HoursPerWeek { get; set; }

    [Display(Name = "Fee Base")]
    public decimal? FeeBase { get; set; }

    public virtual ICollection<AcademicRecord> AcademicRecords { get; set; } = new List<AcademicRecord>();
}
