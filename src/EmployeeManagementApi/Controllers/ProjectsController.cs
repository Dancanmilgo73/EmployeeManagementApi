using EmployeeManagementApi.Application.Interfaces;
using EmployeeManagementApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _projectService.GetAllAsync());
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var project = await _projectService.GetByIdAsync(id);
            return project == null ? NotFound() : Ok(project);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProjectCreateDto projectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var projectId = await _projectService.CreateAsync(projectDto);
            return CreatedAtAction(nameof(GetById), new { id = projectId }, projectDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProjectDto projectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var updatedProjectId = await _projectService.UpdateAsync(id, projectDto);
            return updatedProjectId == null ? NotFound() : CreatedAtAction(nameof(GetById), new { id = updatedProjectId }, projectDto);
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
            var deleted = await _projectService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignEmployeeToProject([FromBody] EmployeeProjectDto employeeProjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _projectService.AssignEmployeeToProjectAsync(employeeProjectDto);
            return result ? Ok() : BadRequest("Failed to assign employee to project.");
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
    [HttpPost("remove")]
    public async Task<IActionResult> RemoveEmployeeFromProject([FromBody] EmployeeProjectDto employeeProjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _projectService.RemoveEmployeeFromProjectAsync(employeeProjectDto);
            return result ? Ok() : BadRequest("Failed to remove employee from project.");
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
    [HttpGet("total-budget")]
    public async Task<IActionResult> GetTotalBudget()
    {
        try
        {
            var totalBudget = await _projectService.GetTotalBudgetAsync();
            return Ok(new { TotalBudget = totalBudget });
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
}