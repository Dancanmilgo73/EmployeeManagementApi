using EmployeeManagementApi.Application.Interfaces;
using EmployeeManagementApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _departmentService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        return department == null ? NotFound() : Ok(department);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DepartmentDto departmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var department = await _departmentService.CreateAsync(departmentDto);
        return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DepartmentDto departmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updatedDepartment = await _departmentService.UpdateAsync(id, departmentDto);
        return updatedDepartment == null ? NotFound() : Ok(updatedDepartment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _departmentService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}