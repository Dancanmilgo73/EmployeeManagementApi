using EmployeeManagementApi.Infrastructure.Data;
using EmployeeManagementApi.Infrastructure.Repositories;
using EmployeeManagementApi.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
namespace EmployeeManagementApi.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IEmployeeRepository Employees { get; }
    public IDepartmentRepository Departments { get; }
    public IProjectRepository Projects { get; }
    private bool _disposed = false;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Employees = new EmployeeRepository(context);
        Departments = new DepartmentRepository(context);
        Projects = new ProjectRepository(context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    public async Task<IDbContextTransaction> BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
}