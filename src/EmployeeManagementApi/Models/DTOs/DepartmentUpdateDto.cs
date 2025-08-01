using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementApi.Models.DTOs;
public class DepartmentUpdateDto
{
    [Required(ErrorMessage = "Name must be provided and cannot be empty")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "Location must be provided and cannot be empty")]
    [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
    public string Location { get; set; } = string.Empty;
}
