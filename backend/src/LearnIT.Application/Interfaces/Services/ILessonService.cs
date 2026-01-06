using LearnIT.Application.DTOs.Lesson;

namespace LearnIT.Application.Interfaces.Services;

public interface ILessonService
{
    Task<List<LessonDto>> GetByCourseIdAsync(Guid courseId);
    Task<LessonDto> CreateAsync(CreateLessonDto dto);
    Task<LessonDto> UpdateAsync(Guid lessonId, UpdateLessonDto dto);

    Task SoftDeleteAsync(Guid lessonId);

    Task ReorderAsync(Guid lessonId, int newOrder);
}
