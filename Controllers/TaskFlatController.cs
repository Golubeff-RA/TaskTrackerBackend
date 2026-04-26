using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourApp.DTOs.Tasks;
using YourApp.Extensions;
using YourApp.Services.Interfaces;

namespace YourApp.Controllers
{
    /// <summary>
    /// Операции над конкретной задачей
    /// </summary>
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TasksFlatController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksFlatController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Рандомная активная задача из всех проектов пользователя.
        /// Возвращает null если нет активных задач.
        /// </summary>
        [HttpGet("random")]
        [ProducesResponseType(typeof(TaskResponseDto), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetRandom()
        {
            var userId = User.GetUserId();
            var task = await _taskService.GetRandomTaskAsync(userId);
            return Ok(task);
        }

        /// <summary>
        /// Изменить задачу
        /// </summary>
        [HttpPatch("{taskId}")]
        [ProducesResponseType(typeof(TaskResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Update(Guid taskId, [FromBody] UpdateTaskDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                var task = await _taskService.UpdateTaskAsync(userId, taskId, dto);
                return Ok(task);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Завершить задачу
        /// </summary>
        [HttpPost("{taskId}/close")]
        [ProducesResponseType(typeof(TaskResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Close(Guid taskId)
        {
            var userId = User.GetUserId();
            var task = await _taskService.CloseTaskAsync(userId, taskId);

            if (task == null)
                return NotFound(new { error = "Task not found" });

            return Ok(task);
        }

        /// <summary>
        /// Заблокировать задачу
        /// </summary>
        [HttpPost("{taskId}/block")]
        [ProducesResponseType(typeof(TaskResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Block(Guid taskId, [FromBody] BlockTaskDto dto)
        {
            var userId = User.GetUserId();
            var task = await _taskService.BlockTaskAsync(userId, taskId, dto);

            if (task == null)
                return NotFound(new { error = "Task not found" });

            return Ok(task);
        }
    }
}