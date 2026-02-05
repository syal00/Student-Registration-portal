using System.ComponentModel.DataAnnotations;

namespace LabAssignment6.Models;

public class NameFormatAttribute : ValidationAttribute
{
    public NameFormatAttribute()
    {
        ErrorMessage = "Employee Name must be in the format: FirstName LastName";
    }

    public override bool IsValid(object? value)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return true; // Let Required attribute handle empty values
        }

        var name = value.ToString()!.Trim();
        var nameParts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        
        return nameParts.Length == 2;
    }
}

















