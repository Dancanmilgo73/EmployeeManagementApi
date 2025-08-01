using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementApi.Models.DTOs;

public class ProjectUpdateDto
{
    [Required(ErrorMessage = "Project name must be provided and cannot be empty")]
    [StringLength(100, ErrorMessage = "Project name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive number")]
    public decimal Budget { get; set; } = 0;
}