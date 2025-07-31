

namespace EmployeeManagementApi.Models.DTOs;
public class EmployeeProjectDto
{
    public int EmployeeId { get; set; }
    public int ProjectId { get; set; }
    public string Role { get; set; } = string.Empty;
}