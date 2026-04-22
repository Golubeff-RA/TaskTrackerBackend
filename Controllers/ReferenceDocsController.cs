using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourApp.DTOs.ReferenceDocs;
using YourApp.Extensions;
using YourApp.Services.Interfaces;

namespace YourApp.Controllers
{
    /// <summary>
    /// Управление справочными документами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReferenceDocsController : ControllerBase
    {
        private readonly IReferenceDocService _service;

        public ReferenceDocsController(IReferenceDocService service)
        {
            _service = service;
        }

        /// <summary>
        /// Получить список всех справок пользователя
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ReferenceDocResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.GetUserId();
            var docs = await _service.GetAllAsync(userId);
            return Ok(docs);
        }

        /// <summary>
        /// Получить справку по ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReferenceDocResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var doc = await _service.GetByIdAsync(userId, id);
                return Ok(doc);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Создать справку
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ReferenceDocResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create([FromBody] CreateReferenceDocDto dto)
        {
            var userId = User.GetUserId();
            var doc = await _service.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = doc.DocUuid }, doc);
        }

        /// <summary>
        /// Изменить справку
        /// </summary>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ReferenceDocResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
       public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReferenceDocDto dto)
       {
            try
            {
                var userId = User.GetUserId();
                var doc = await _service.UpdateAsync(userId, id, dto);
                return Ok(doc);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
        
        /// <summary>
        /// Удалить справку (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.GetUserId();
            var result = await _service.DeleteAsync(userId, id);

            if (!result)
                return NotFound(new { message = "Reference was not found" });

            return Ok(new { message = "Reference has been deleted" });
        }
    }
}
