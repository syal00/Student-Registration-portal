using System.ComponentModel.DataAnnotations;

namespace CourseLedger.DataAccess;

public partial class Student
{
    [Required]
    [MaxLength(16)]
    public string Id { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    public virtual ICollection<AcademicRecord> AcademicRecords { get; set; } = new List<AcademicRecord>();
}
