using LearnIT.Application.DTOs.Lesson;
using LearnIT.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnIT.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet("~/api/courses/{courseId}/lessons")]
    public async Task<IActionResult> GetByCourse(Guid courseId)
    {
        var lessons = await _lessonService.GetByCourseIdAsync(courseId);
        return Ok(lessons);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLessonDto dto)
    {
        var lesson = await _lessonService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByCourse), new { courseId = dto.CourseId }, lesson);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLessonDto dto)
    {
        var lesson = await _lessonService.UpdateAsync(id, dto);
        return Ok(lesson);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _lessonService.SoftDeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/reorder")]
    public async Task<IActionResult> Reorder(Guid id, [FromBody] ReorderLessonDto dto)
    {
        await _lessonService.ReorderAsync(id, dto.NewOrder);
        return NoContent();
    }
}