
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementApi.Models.DTOs;

public class EmployeeUpdateDto
{
    [Required(ErrorMessage = "First name must be provided and cannot be empty")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name must be provided and cannot be empty")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email must be provided and cannot be empty")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number")]
    public decimal Salary { get; set; } = 0;

    [Required(ErrorMessage = "Department ID must be provided and cannot be empty")]
    [Range(1, int.MaxValue, ErrorMessage = "Department ID must be a valid ID (Use ID 1 for HR department)")]
    public int DepartmentId { get; set; } = 1; 
}