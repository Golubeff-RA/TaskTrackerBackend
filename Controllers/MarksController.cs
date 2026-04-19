using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourApp.DTOs.Marks;
using YourApp.Extensions;
using YourApp.Services.Interfaces;

namespace YourApp.Controllers
{
    /// <summary>
    /// Управление пометками проекта
    /// </summary>
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
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Создать пометку в проекте
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(MarkResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create(Guid projectId, [FromBody] CreateMarkDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                var mark = await _markService.CreateMarkAsync(userId, projectId, dto);
                return StatusCode(201, mark);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Изменить пометку
        /// </summary>
        [HttpPut("{markId}")]
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
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Удалить пометку (soft delete)
        /// </summary>
        [HttpDelete("{markId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(Guid projectId, Guid markId)
        {
            var userId = User.GetUserId();
            var result = await _markService.DeleteMarkAsync(userId, markId);

            if (!result)
                return NotFound(new { message = "The tag was not found" });

            return Ok(new { message = "The mark has been deleted" });
        }
    }
}
