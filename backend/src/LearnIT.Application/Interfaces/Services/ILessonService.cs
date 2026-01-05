using LearnIT.Domain.Entities;

namespace LearnIT.Application.Interfaces.Services;

public interface ILessonService
{
    Task CreateAsync(Guid courseId, Lesson lesson);

    Task SoftDeleteAsync(Guid lessonId);

    Task ReorderAsync(Guid lessonId, int newOrder);
}
