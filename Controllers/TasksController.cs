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
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Получить задачу по ID
        /// </summary>
        [HttpGet("{taskId}")]
        [ProducesResponseType(typeof(TaskResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetById(Guid projectId, Guid taskId)
        {
            try
            {
                var userId = User.GetUserId();
                var task = await _taskService.GetTaskByIdAsync(userId, taskId);
                return Ok(task);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Создать задачу в проекте
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(TaskResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create(Guid projectId, [FromBody] CreateTaskDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                var task = await _taskService.CreateTaskAsync(userId, projectId, dto);
                return CreatedAtAction(nameof(GetById), new { projectId, taskId = task.TaskUuid }, task);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Изменить задачу
        /// </summary>
        [HttpPut("{taskId}")]
        [ProducesResponseType(typeof(TaskResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Update(Guid projectId, Guid taskId, [FromBody] UpdateTaskDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                var task = await _taskService.UpdateTaskAsync(userId, taskId, dto);
                return Ok(task);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Закрыть задачу (статус → CLOSED)
        /// </summary>
        [HttpPatch("{taskId}/close")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Close(Guid projectId, Guid taskId)
        {
            var userId = User.GetUserId();
            var result = await _taskService.CloseTaskAsync(userId, taskId);

            if (!result)
                return NotFound(new { message = "Issue not found" });

            return Ok(new { message = "The task is closed" });
        }

        /// <summary>
        /// Заблокировать задачу (статус → BLOCKED)
        /// </summary>
        [HttpPatch("{taskId}/block")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Block(Guid projectId, Guid taskId, [FromBody] BlockTaskDto dto)
        {
            var userId = User.GetUserId();
            var result = await _taskService.BlockTaskAsync(userId, taskId, dto);

            if (!result)
                return NotFound(new { message = "Issue not found" });

            return Ok(new { message = "The task is blocked" });
        }
    }
}
