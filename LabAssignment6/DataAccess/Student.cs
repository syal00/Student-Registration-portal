using System;
using System.Collections.Generic;

namespace LabAssignment6.DataAccess;

public partial class Student
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<AcademicRecord> AcademicRecords { get; set; } = new List<AcademicRecord>();

    public virtual ICollection<Course> CourseCourses { get; set; } = new List<Course>();
}
