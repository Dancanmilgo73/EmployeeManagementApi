using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementApi.Models.Entities;
public class Employee : BaseEntity
{
    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "FirstName cannot be empty.")]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "LastName cannot be empty.")]
    public string LastName { get; set; } = string.Empty;
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Range(0, double.MaxValue)]
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }
    [Required]
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public List<EmployeeProject> EmployeeProjects { get; set; } = new();
}