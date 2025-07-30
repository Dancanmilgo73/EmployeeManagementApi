using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
namespace EmployeeManagementApi.Infrastructure.Repositories.Interfaces;
public interface IUnitOfWork : IDisposable
{
    IEmployeeRepository Employees { get; }
    IDepartmentRepository Departments { get; }
    IProjectRepository Projects { get; }
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<int> SaveChangesAsync();
}