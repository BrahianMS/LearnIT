using LearnIT.Application.Exceptions;
using LearnIT.Application.Interfaces.Repositories;
using LearnIT.Application.Interfaces.Services;
using LearnIT.Domain.Entities;

namespace LearnIT.Application.Services;

public class LessonService : ILessonService
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ICourseRepository _courseRepository;

    public LessonService(
        ILessonRepository lessonRepository,
        ICourseRepository courseRepository)
    {
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
    }

    public async Task CreateAsync(Guid courseId, Lesson lesson)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);

        if (course is null)
            throw new NotFoundException("Course not found");

        var orderExists = await _lessonRepository
            .ExistsWithOrderAsync(courseId, lesson.Order);

        if (orderExists)
            throw new BusinessRuleException(
                "Lesson order must be unique within the course");

        lesson.CourseId = courseId;
        lesson.CreatedAt = DateTime.UtcNow;
        lesson.UpdatedAt = DateTime.UtcNow;

        await _lessonRepository.AddAsync(lesson);
        await _lessonRepository.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Guid lessonId)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson is null)
            throw new NotFoundException("Lesson not found");

        lesson.IsDeleted = true;
        lesson.UpdatedAt = DateTime.UtcNow;

        await _lessonRepository.UpdateAsync(lesson);
        await _lessonRepository.SaveChangesAsync();
    }

    public async Task ReorderAsync(Guid lessonId, int newOrder)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson is null)
            throw new NotFoundException("Lesson not found");

        if (lesson.Order == newOrder)
            return;

        var exists = await _lessonRepository
            .ExistsWithOrderAsync(lesson.CourseId, newOrder);

        if (exists)
            throw new BusinessRuleException(
                "Another lesson already has this order");

        lesson.Order = newOrder;
        lesson.UpdatedAt = DateTime.UtcNow;

        await _lessonRepository.UpdateAsync(lesson);
        await _lessonRepository.SaveChangesAsync();
    }
}