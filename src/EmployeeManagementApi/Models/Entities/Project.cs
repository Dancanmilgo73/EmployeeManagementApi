public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public string ProjectCode { get; set; } = string.Empty;
    public List<EmployeeProject> EmployeeProjects { get; set; } = new();
}