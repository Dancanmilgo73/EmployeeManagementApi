
namespace EmployeeManagementApi.Models.DTOs;

public class EmployeeCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Salary { get; set; } = 0;
    public int? DepartmentId { get; set; } = 1; // Default to HR department
}