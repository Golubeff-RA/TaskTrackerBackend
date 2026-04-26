using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourApp.DTOs.Tasks;
using YourApp.Extensions;
using YourApp.Services.Interfaces;

namespace YourApp.Controllers
{
    /// <summary>
    /// Управление задачами проекта
    /// </summary>
    [ApiController]
    [Route("api/projects/{projectId}/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Получить список задач проекта
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<TaskResponseDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAll(Guid projectId)
        {
            try
            {
                var userId = User.GetUserId();
                var tasks = await _taskService.GetTasksByProjectAsync(userId, projectId);
                return Ok(tasks);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        /// <summary>Создать задачу в проекте</summary>
        [HttpPost]
        [ProducesResponseType(typeof(TaskResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create(Guid projectId, [FromBody] CreateTaskDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                var task = await _taskService.CreateTaskAsync(userId, projectId, dto);
                return Ok(task);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}