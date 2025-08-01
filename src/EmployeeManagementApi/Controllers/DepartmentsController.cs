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
        try
        {
            var department = await _departmentService.GetByIdAsync(id);
            return department == null ? NotFound() : Ok(department);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DepartmentCreateDto departmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var departmentId = await _departmentService.CreateAsync(departmentDto);
            return CreatedAtAction(nameof(GetById), new { id = departmentId }, departmentDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DepartmentUpdateDto departmentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var updatedDepartmentId = await _departmentService.UpdateAsync(id, departmentDto);
            return updatedDepartmentId == null ? NotFound() : CreatedAtAction(nameof(GetById), new { id = updatedDepartmentId }, departmentDto);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _departmentService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
}