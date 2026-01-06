using LearnIT.Application.DTOs.Lesson;
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

    public async Task<List<LessonDto>> GetByCourseIdAsync(Guid courseId)
    {
        var lessons = await _lessonRepository.GetByCourseIdAsync(courseId);
        return lessons.Select(MapToDto).ToList();
    }

    public async Task<LessonDto> CreateAsync(CreateLessonDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(dto.CourseId);
        if (course is null) throw new NotFoundException("Course not found");

        var orderExists = await _lessonRepository.ExistsWithOrderAsync(dto.CourseId, dto.Order);
        if (orderExists)
            throw new BusinessRuleException("Lesson order must be unique within the course");

        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = dto.CourseId,
            Title = dto.Title,
            Order = dto.Order,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _lessonRepository.AddAsync(lesson);
        await _lessonRepository.SaveChangesAsync();

        return MapToDto(lesson);
    }

    public async Task<LessonDto> UpdateAsync(Guid lessonId, UpdateLessonDto dto)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId);
        if (lesson is null) throw new NotFoundException("Lesson not found");

        lesson.Title = dto.Title;
        lesson.UpdatedAt = DateTime.UtcNow;

        await _lessonRepository.UpdateAsync(lesson);
        await _lessonRepository.SaveChangesAsync();

        return MapToDto(lesson);
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

        var conflictingLesson = await _lessonRepository.GetByOrderAsync(lesson.CourseId, newOrder);

        if (conflictingLesson != null)
        {
            // Swap logic with temp value to avoid Unique Constraint / Circular Dependency issues
            int originalOrder = lesson.Order;
            
            // Step 1: Move conflicting lesson to temp
            conflictingLesson.Order = -1; // Assuming -1 is safe/unused
            await _lessonRepository.UpdateAsync(conflictingLesson); 
            await _lessonRepository.SaveChangesAsync();

            // Step 2: Move target lesson to new spot
            lesson.Order = newOrder;
            lesson.UpdatedAt = DateTime.UtcNow;
            await _lessonRepository.UpdateAsync(lesson);
            await _lessonRepository.SaveChangesAsync();

            // Step 3: Move conflicting lesson to original spot
            conflictingLesson.Order = originalOrder;
            conflictingLesson.UpdatedAt = DateTime.UtcNow;
            await _lessonRepository.UpdateAsync(conflictingLesson);
            await _lessonRepository.SaveChangesAsync();
        }
        else 
        {
            // No conflict, just move
            lesson.Order = newOrder;
            lesson.UpdatedAt = DateTime.UtcNow;
            await _lessonRepository.UpdateAsync(lesson);
            await _lessonRepository.SaveChangesAsync();
        }
    }

    private static LessonDto MapToDto(Lesson lesson)
    {
        return new LessonDto
        {
            Id = lesson.Id,
            CourseId = lesson.CourseId,
            Title = lesson.Title,
            Order = lesson.Order,
            CreatedAt = lesson.CreatedAt,
            UpdatedAt = lesson.UpdatedAt
        };
    }
}