using LearnIT.Application.Exceptions;
using LearnIT.Application.Interfaces.Repositories;
using LearnIT.Application.Interfaces.Services;
using LearnIT.Domain.Entities;

namespace LearnIT.Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task PublishAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdWithLessonsAsync(courseId);

        if (course is null)
            throw new NotFoundException("Course not found");

        var hasActiveLessons = course.Lessons.Any(l => !l.IsDeleted);

        if (!hasActiveLessons)
            throw new BusinessRuleException(
                "A course must have at least one active lesson to be published");

        course.Status = CourseStatus.Published;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);
        await _courseRepository.SaveChangesAsync();
    }

    public async Task UnpublishAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);

        if (course is null)
            throw new NotFoundException("Course not found");

        course.Status = CourseStatus.Draft;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);
        await _courseRepository.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);

        if (course is null)
            throw new NotFoundException("Course not found");

        course.IsDeleted = true;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);
        await _courseRepository.SaveChangesAsync();
    }
}