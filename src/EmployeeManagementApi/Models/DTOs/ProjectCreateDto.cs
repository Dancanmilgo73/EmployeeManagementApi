namespace EmployeeManagementApi.Models.DTOs;

public class ProjectCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string ProjectCode { get; set; } = string.Empty;
    public decimal Budget { get; set; }
}