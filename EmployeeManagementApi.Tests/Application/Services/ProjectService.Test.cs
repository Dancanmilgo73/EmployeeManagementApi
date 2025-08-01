
using EmployeeManagementApi.Application.Interfaces;
using EmployeeManagementApi.Infrastructure.Repositories.Interfaces;
using EmployeeManagementApi.Models.DTOs;
using EmployeeManagementApi.Models.Entities;
using Moq;

namespace EmployeeManagementApi.Application.Services.Tests
{
    public class ProjectServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProjectRepository> _projectRepoMock;
        private readonly Mock<IEmployeeRepository> _employeeRepoMock;
        private readonly Mock<IDepartmentRepository> _departmentRepoMock;
        private readonly Mock<IRandomStringGeneratorService> _randomStringGeneratorMock;
        private readonly ProjectService _service;

        public ProjectServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _projectRepoMock = new Mock<IProjectRepository>();
            _employeeRepoMock = new Mock<IEmployeeRepository>();
            _departmentRepoMock = new Mock<IDepartmentRepository>();
            _randomStringGeneratorMock = new Mock<IRandomStringGeneratorService>();

            _unitOfWorkMock.SetupGet(u => u.Projects).Returns(_projectRepoMock.Object);
            _unitOfWorkMock.SetupGet(u => u.Employees).Returns(_employeeRepoMock.Object);
            _unitOfWorkMock.SetupGet(u => u.Departments).Returns(_departmentRepoMock.Object);

            _service = new ProjectService(_unitOfWorkMock.Object, _randomStringGeneratorMock.Object);
        }

        [Fact]
        public async Task CreateAsync_CreatesProjectAndReturnsId()
        {
            var projectDto = new ProjectCreateDto { Name = "Test", Budget = 1000 };
            var project = new Project { Id = 5, Name = "Test", Budget = 1000 };

            var transactionMock = new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>();
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(transactionMock.Object);
            _projectRepoMock.Setup(r => r.AddAsync(It.IsAny<Project>()))
                .Callback<Project>(p => p.Id = project.Id)
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _randomStringGeneratorMock.Setup(r => r.GenerateRandomStringAsync(10)).ReturnsAsync("RANDOMCODE");

            var result = await _service.CreateAsync(projectDto);

            Assert.Equal(5, result);
            transactionMock.Verify(t => t.CommitAsync(It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }
        //TOD: Revisit and fix

        // [Fact]
        // public async Task CreateAsync_RollsBackAndThrows_OnError()
        // {
        //     var projectDto = new ProjectCreateDto { Name = "Test", Budget = 1000 };
        //     var transactionMock = new Mock<ITransaction>();
        //     _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(transactionMock.Object);
        //     _projectRepoMock.Setup(r => r.AddAsync(It.IsAny<Project>())).ThrowsAsync(new Exception("DB error"));

        //     await Assert.ThrowsAsync<ApplicationException>(() => _service.CreateAsync(projectDto));
        //     transactionMock.Verify(t => t.RollbackAsync(), Times.Once);
        // }

        [Fact]
        public async Task GetAllAsync_ReturnsProjectDtos()
        {
            var projects = new List<Project>
            {
                new Project { Id = 1, Name = "Alpha", ProjectCode = "A-1", Budget = 100 },
                new Project { Id = 2, Name = "Beta", ProjectCode = "B-2", Budget = 200 }
            };
            _projectRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(projects);

            var result = await _service.GetAllAsync();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Name == "Alpha" && p.ProjectCode == "A-1" && p.Budget == 100);
            Assert.Contains(result, p => p.Name == "Beta" && p.ProjectCode == "B-2" && p.Budget == 200);
        }

        [Fact]
        public async Task GetAllAsync_ThrowsApplicationException_OnError()
        {
            _projectRepoMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("DB error"));
            await Assert.ThrowsAsync<ApplicationException>(() => _service.GetAllAsync());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProjectDto_WhenFound()
        {
            var project = new Project { Id = 1, Name = "Alpha", ProjectCode = "A-1", Budget = 100 };
            _projectRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(project);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Alpha", result.Name);
            Assert.Equal("A-1", result.ProjectCode);
            Assert.Equal(100, result.Budget);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _projectRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Project?)null);
            var result = await _service.GetByIdAsync(1);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsApplicationException_OnError()
        {
            _projectRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("DB error"));
            await Assert.ThrowsAsync<ApplicationException>(() => _service.GetByIdAsync(1));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProjectAndReturnsId()
        {
            var project = new Project { Id = 2, Name = "Old", Budget = 50 };
            var dto = new ProjectUpdateDto { Name = "New", Budget = 150 };
            _projectRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(project);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.UpdateAsync(2, dto);

            Assert.Equal(2, result);
            Assert.Equal("New", project.Name);
            Assert.Equal(150, project.Budget);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenNotFound()
        {
            _projectRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Project?)null);
            var dto = new ProjectUpdateDto { Name = "Any", Budget = 0 };
            var result = await _service.UpdateAsync(99, dto);
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsApplicationException_OnError()
        {
            _projectRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("DB error"));
            var dto = new ProjectUpdateDto { Name = "Any", Budget = 0 };
            await Assert.ThrowsAsync<ApplicationException>(() => _service.UpdateAsync(1, dto));
        }

        [Fact]
        public async Task DeleteAsync_DeletesProjectAndReturnsTrue()
        {
            _projectRepoMock.Setup(r => r.DeleteAsync(3)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.DeleteAsync(3);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotDeleted()
        {
            _projectRepoMock.Setup(r => r.DeleteAsync(4)).ReturnsAsync(false);
            var result = await _service.DeleteAsync(4);
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsApplicationException_OnError()
        {
            _projectRepoMock.Setup(r => r.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new Exception("DB error"));
            await Assert.ThrowsAsync<ApplicationException>(() => _service.DeleteAsync(1));
        }

        [Fact]
        public async Task AssignEmployeeToProjectAsync_AssignsAndReturnsTrue()
        {
            var dto = new EmployeeProjectDto { EmployeeId = 1, ProjectId = 2 };
            var project = new Project { Id = 2 };
            var employee = new Employee { Id = 1 };

            _projectRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(project);
            _employeeRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);
            _projectRepoMock.Setup(r => r.AssignEmployeeToProjectAsync(dto)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.AssignEmployeeToProjectAsync(dto);

            Assert.True(result);
        }

        [Fact]
        public async Task AssignEmployeeToProjectAsync_Throws_WhenProjectNotFound()
        {
            var dto = new EmployeeProjectDto { EmployeeId = 1, ProjectId = 2 };
            _projectRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Project?)null);
            await Assert.ThrowsAsync<ApplicationException>(() => _service.AssignEmployeeToProjectAsync(dto));
        }

        [Fact]
        public async Task AssignEmployeeToProjectAsync_Throws_WhenEmployeeNotFound()
        {
            var dto = new EmployeeProjectDto { EmployeeId = 1, ProjectId = 2 };
            var project = new Project { Id = 2 };
            _projectRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(project);
            _employeeRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Employee?)null);
            await Assert.ThrowsAsync<ApplicationException>(() => _service.AssignEmployeeToProjectAsync(dto));
        }

        [Fact]
        public async Task AssignEmployeeToProjectAsync_ThrowsApplicationException_OnError()
        {
            var dto = new EmployeeProjectDto { EmployeeId = 1, ProjectId = 2 };
            var project = new Project { Id = 2 };
            var employee = new Employee { Id = 1 };
            _projectRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(project);
            _employeeRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);
            _projectRepoMock.Setup(r => r.AssignEmployeeToProjectAsync(dto)).ThrowsAsync(new Exception("DB error"));
            await Assert.ThrowsAsync<ApplicationException>(() => _service.AssignEmployeeToProjectAsync(dto));
        }
        //TODO: Revisit and fix

        // [Fact]
        // public async Task RemoveEmployeeFromProjectAsync_RemovesAndReturnsTrue()
        // {
        //     var dto = new EmployeeProjectDto { EmployeeId = 1, ProjectId = 2 };
        //     _projectRepoMock.Setup(r => r.RemoveEmployeeFromProjectAsync(dto)).ReturnsAsync(true);
        //     _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        //     var result = await _service.RemoveEmployeeFromProjectAsync(dto);

        //     Assert.True(result);
        // }

        // [Fact]
        // public async Task RemoveEmployeeFromProjectAsync_ReturnsFalse_WhenNotRemoved()
        // {
        //     var dto = new EmployeeProjectDto { EmployeeId = 1, ProjectId = 2 };
        //     _projectRepoMock.Setup(r => r.RemoveEmployeeFromProjectAsync(dto)).ReturnsAsync(false);
        //     var result = await _service.RemoveEmployeeFromProjectAsync(dto);
        //     Assert.False(result);
        // }

        // [Fact]
        // public async Task RemoveEmployeeFromProjectAsync_ThrowsApplicationException_OnError()
        // {
        //     var dto = new EmployeeProjectDto { EmployeeId = 1, ProjectId = 2 };
        //     _projectRepoMock.Setup(r => r.RemoveEmployeeFromProjectAsync(dto)).ThrowsAsync(new Exception("DB error"));
        //     await Assert.ThrowsAsync<ApplicationException>(() => _service.RemoveEmployeeFromProjectAsync(dto));
        // }

        [Fact]
        public async Task GetTotalBudgetAsync_ReturnsSumOfBudgets()
        {
            var projects = new List<Project>
            {
                new Project { Budget = 100 },
                new Project { Budget = 200 }
            };
            _departmentRepoMock.Setup(r => r.GetProjectsByDepartmentIdAsync(1)).ReturnsAsync(projects);

            var result = await _service.GetTotalBudgetAsync(1);

            Assert.Equal(300, result);
        }

        [Fact]
        public async Task GetTotalBudgetAsync_ThrowsApplicationException_OnError()
        {
            _departmentRepoMock.Setup(r => r.GetProjectsByDepartmentIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("DB error"));
            await Assert.ThrowsAsync<ApplicationException>(() => _service.GetTotalBudgetAsync(1));
        }
    }
}