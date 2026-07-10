using System.ComponentModel.DataAnnotations;

namespace CourseLedger.DataAccess;

public partial class Role
{
    [Required]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Role Name")]
    [MaxLength(100)]
    public string? Role1 { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
