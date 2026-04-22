using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourApp.DTOs.Marks;
using YourApp.Extensions;
using YourApp.Services.Interfaces;

namespace YourApp.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/marks")]
    [Authorize]
    public class MarksController : ControllerBase
    {
        private readonly IMarkService _markService;

        public MarksController(IMarkService markService)
        {
            _markService = markService;
        }

        /// <summary>
        /// Получить список пометок проекта
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<MarkResponseDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAll(Guid projectId)
        {
            try
            {
                var userId = User.GetUserId();
                var marks = await _markService.GetMarksByProjectAsync(userId, projectId);
                return Ok(marks);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Создать пометку в проекте
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(MarkResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create(Guid projectId, [FromBody] CreateMarkDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                var mark = await _markService.CreateMarkAsync(userId, projectId, dto);
                return Ok(mark);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Изменить пометку
        /// </summary>
        [HttpPatch("{markId}")]
        [ProducesResponseType(typeof(MarkResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Update(Guid projectId, Guid markId, [FromBody] UpdateMarkDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                var mark = await _markService.UpdateMarkAsync(userId, markId, dto);
                return Ok(mark);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Удалить пометку (soft delete)
        /// </summary>
        [HttpDelete("{markId}")]
        [ProducesResponseType(typeof(MarkResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(Guid projectId, Guid markId)
        {
            var userId = User.GetUserId();
            var mark = await _markService.DeleteMarkAsync(userId, markId);

            if (mark == null)
                return NotFound(new { error = "Mark not found" });

            return Ok(mark);
        }
    }
}