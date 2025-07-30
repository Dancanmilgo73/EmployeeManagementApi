public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string OfficeLocation { get; set; } = string.Empty;
    public List<Employee> Employees { get; set; } = new();
    public List<Project> Projects { get; set; } = new();
}