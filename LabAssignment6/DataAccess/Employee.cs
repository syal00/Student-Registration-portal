using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LabAssignment6.Models;

namespace LabAssignment6.DataAccess;

public partial class Employee
{
    public int Id { get; set; }

    [Display(Name = "Employee Name")]
    [Required(ErrorMessage = "Employee Name is required.")]
    [NameFormat]
    public string Name { get; set; } = null!;

    [Display(Name = "Network ID")]
    [Required(ErrorMessage = "Network ID is required.")]
    [MinLength(3, ErrorMessage = "Network ID must be at least 3 characters.")]
    public string UserName { get; set; } = null!;

    [Display(Name = "Password")]
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(5, ErrorMessage = "Password must be at least 5 characters.")]
    public string Password { get; set; } = null!;

    [Display(Name = "Job Title(s)")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
