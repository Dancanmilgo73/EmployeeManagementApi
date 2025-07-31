using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementApi.Models.Entities;
public class Department : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string OfficeLocation { get; set; } = string.Empty;
    public List<Employee> Employees { get; set; } = new();
    public List<Project> Projects { get; set; } = new();
}