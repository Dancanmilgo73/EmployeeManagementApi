using EmployeeManagementApi.Application.Interfaces;
using EmployeeManagementApi.Infrastructure.Repositories.Interfaces;
using EmployeeManagementApi.Models.DTOs;
using EmployeeManagementApi.Models.Entities;

namespace EmployeeManagementApi.Application.Services;
public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        try
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            return departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Location = d.OfficeLocation
            });
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while retrieving departments.");
        }
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        try
        {         
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null) return null;

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Location = department.OfficeLocation
            };           
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while retrieving the department.");
        }
    }

    public async Task<int> CreateAsync(DepartmentCreateDto departmentDto)
    {
        try
        {
            var department = new Department
            {
                Name = departmentDto.Name,
                OfficeLocation = departmentDto.Location
            };

            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();

            return department.Id;
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while creating the department.");
        }
    }

    public async Task<int?> UpdateAsync(int id, DepartmentDto departmentDto)
    {
        try
        {
            var existingDepartment = await _unitOfWork.Departments.GetByIdAsync(id);
            if (existingDepartment == null) return null;

            existingDepartment.Name = departmentDto.Name;
            existingDepartment.OfficeLocation = departmentDto.Location;
            await _unitOfWork.SaveChangesAsync();

            return existingDepartment.Id;
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while updating the department.");
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var deleted = await _unitOfWork.Departments.DeleteAsync(id);
            if (!deleted) return false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (System.Exception)
        {
            throw new ApplicationException("An error occurred while deleting the department.");
        }
    }
}