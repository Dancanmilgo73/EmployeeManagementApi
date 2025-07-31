using EmployeeManagementApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
//Add a namespace declaration
namespace EmployeeManagementApi.Infrastructure.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<EmployeeProject> EmployeeProjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeProject>()
            .HasKey(ep => new { ep.EmployeeId, ep.ProjectId });
        //Seeding initial data
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "HR", OfficeLocation = "New York" },
            new Department { Id = 2, Name = "IT", OfficeLocation = "San Francisco" }
        );
        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, FirstName = "John", LastName = "Doe", DepartmentId = 1, Salary = 50000 },
            new Employee { Id = 2, FirstName = "Jane", LastName = "Smith", DepartmentId = 2, Salary = 60000 }
        );
        modelBuilder.Entity<Project>().HasData(
            new Project { Id = 1, Name = "Project A", Budget = 10000, ProjectCode = "CY-CJLSXZ7-1" },
            new Project { Id = 2, Name = "Project B", Budget = 20000, ProjectCode = "CY-CJWEXZ7-2" }
        );
        modelBuilder.Entity<EmployeeProject>().HasData(
            new EmployeeProject { EmployeeId = 1, ProjectId = 1 },
            new EmployeeProject { EmployeeId = 2, ProjectId = 2 }
        );
    }
}