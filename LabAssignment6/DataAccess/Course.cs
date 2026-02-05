using System;
using System.Collections.Generic;

namespace LabAssignment6.DataAccess;

public partial class Course
{
    public string Code { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? HoursPerWeek { get; set; }

    public decimal? FeeBase { get; set; }

    public virtual ICollection<AcademicRecord> AcademicRecords { get; set; } = new List<AcademicRecord>();

    public virtual ICollection<Student> StudentStudentNums { get; set; } = new List<Student>();
}
