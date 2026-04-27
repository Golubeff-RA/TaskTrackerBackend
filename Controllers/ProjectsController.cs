using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourApp.DTOs.Projects;
using YourApp.Extensions;
using YourApp.Services.Interfaces;

namespace YourApp.Controllers
{
    /// <summary>
    /// Управление проектами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Получить список всех проектов пользователя
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ProjectResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.GetUserId();
            var projects = await _projectService.GetAllProjectsAsync(userId);
            return Ok(projects);
        }

        /// <summary>
        /// Получить проект по ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var project = await _projectService.GetProjectByIdAsync(userId, id);
                return Ok(project);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Создать проект
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
        {
            var userId = User.GetUserId();
            var project = await _projectService.CreateProjectAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = project.ProjectUuid }, project);
        }

        /// <summary>
        /// Изменить проект
        /// </summary>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ProjectResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                var project = await _projectService.UpdateProjectAsync(userId, id, dto);
                return Ok(project);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Закрыть проект
        /// </summary>
        [HttpPatch("{id}/close")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Close(Guid id)
        {
            var userId = User.GetUserId();
            var project = await _projectService.CloseProjectAsync(userId, id);
            if (project == null)
                return NotFound(new { error = "Проект не найден" });
                
            return Ok(project);
        }
    }
}
