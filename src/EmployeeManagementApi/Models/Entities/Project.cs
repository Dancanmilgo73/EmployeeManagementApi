using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementApi.Models.Entities;
public class Project : BaseEntity
{
    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Project name cannot be empty.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive value.")]
    public decimal Budget { get; set; }
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Project code cannot be empty.")]
    public string? ProjectCode { get; set; }
    public List<EmployeeProject> EmployeeProjects { get; set; } = new();
}