using EmployeeManagementApi.Infrastructure.Data;
using EmployeeManagementApi.Infrastructure.Repositories.Interfaces;
using EmployeeManagementApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementApi.Infrastructure.Repositories;
public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;
    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Department> GetByIdAsync(int id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null)
            throw new InvalidOperationException($"Department with id {id} not found.");
        return department;
    }
    public async Task<IEnumerable<Department>> GetAllAsync() => await _context.Departments.ToListAsync();
    public async Task AddAsync(Department department)
    {
        await _context.Departments.AddAsync(department);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Department department)
    {
        _context.Departments.Update(department);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var department = await GetByIdAsync(id);
        if (department != null)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}
