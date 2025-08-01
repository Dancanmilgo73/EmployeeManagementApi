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
            .HasKey(ep => new { ep.EmployeeId, ep.ProjectId });// Configure Project -> Department relationship (disable cascade)
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Department)
            .WithMany(d => d.Projects)
            .HasForeignKey(p => p.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<EmployeeProject>()
            .HasOne(ep => ep.Employee)
            .WithMany(e => e.EmployeeProjects)
            .HasForeignKey(ep => ep.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<EmployeeProject>()
            .HasOne(ep => ep.Project)
            .WithMany(p => p.EmployeeProjects)
            .HasForeignKey(ep => ep.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);


        //Seeding initial data
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "HR", OfficeLocation = "New York" },
            new Department { Id = 2, Name = "IT", OfficeLocation = "San Francisco" }
        );
        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, FirstName = "John", Email = "john.doe@example.com", LastName = "Doe", DepartmentId = 1, Salary = 50000 },
            new Employee { Id = 2, FirstName = "Jane", Email = "jane.smith@example.com", LastName = "Smith", DepartmentId = 2, Salary = 60000 }
        );
        modelBuilder.Entity<Project>().HasData(
            new Project { Id = 1, Name = "Project A", Budget = 10000, ProjectCode = "CY-CJLSXZ7-1", DepartmentId = 1 },
            new Project { Id = 2, Name = "Project B", Budget = 20000, ProjectCode = "CY-CJWEXZ7-2", DepartmentId = 2 },
            new Project { Id = 3, Name = "Project C", Budget = 10000, ProjectCode = "CY-CJLSXZ7-3", DepartmentId = 1 },
            new Project { Id = 4, Name = "Project D", Budget = 20000, ProjectCode = "CY-CJWEXZ7-4", DepartmentId = 2 }
        );
        modelBuilder.Entity<EmployeeProject>().HasData(
            new EmployeeProject { EmployeeId = 1, ProjectId = 1, Role = "Developer" },
            new EmployeeProject { EmployeeId = 2, ProjectId = 2, Role = "Manager" }
        );
    }
}