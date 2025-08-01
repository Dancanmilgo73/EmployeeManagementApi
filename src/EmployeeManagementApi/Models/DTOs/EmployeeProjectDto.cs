

using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementApi.Models.DTOs;
public class EmployeeProjectDto
{
    [Required(ErrorMessage = "Employee ID must be provided and cannot be empty")]
    [Range(1, int.MaxValue, ErrorMessage = "Employee ID must be a valid ID")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Project ID must be provided and cannot be empty")]
    [Range(1, int.MaxValue, ErrorMessage = "Project ID must be a valid ID")]
    public int ProjectId { get; set; }
    [Required(ErrorMessage = "Role must be provided and cannot be empty")]
    [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters")]
    [RegularExpression("^(Developer|Manager|Tester)$", ErrorMessage = "Role must be either 'Developer', 'Manager', or 'Tester'")]
    public string Role { get; set; } = string.Empty;
}