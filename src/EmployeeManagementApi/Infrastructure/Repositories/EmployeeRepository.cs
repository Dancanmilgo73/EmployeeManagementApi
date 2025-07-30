using Microsoft.EntityFrameworkCore;
using EmployeeManagementApi.Infrastructure.Data;
using EmployeeManagementApi.Infrastructure.Repositories.Interfaces;
using EmployeeManagementApi.Models.Entities;
namespace EmployeeManagementApi.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;
    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Employee> GetByIdAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
            throw new InvalidOperationException($"Employee with id {id} not found.");
        return employee;
    }
    public async Task<IEnumerable<Employee>> GetAllAsync() => await _context.Employees.ToListAsync();
    public async Task<Employee> AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }
    public async Task<Employee> UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await GetByIdAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
    public async Task<IEnumerable<Employee>> GetEmployeesByProjectIdAsync(int projectId)
    {
        return await _context.EmployeeProjects
            .Where(ep => ep.ProjectId == projectId && ep.Employee != null)
            .Select(ep => ep.Employee!)
            .ToListAsync();
    }
}