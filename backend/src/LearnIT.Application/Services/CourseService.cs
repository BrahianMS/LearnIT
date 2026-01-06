using LearnIT.Application.DTOs.Common;
using LearnIT.Application.DTOs.Course;
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

    public async Task<CourseSummaryDto> GetSummaryAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdWithLessonsAsync(courseId);
        if (course is null) throw new NotFoundException("Curso no encontrado");

        return new CourseSummaryDto
        {
            Id = course.Id,
            Title = course.Title,
            Status = (int)course.Status,
            TotalLessons = course.Lessons.Count(l => !l.IsDeleted),
            LastUpdatedAt = course.UpdatedAt
        };
    }

    public async Task<CourseDto> GetByIdAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course is null) throw new NotFoundException("Curso no encontrado");

        return MapToDto(course);
    }

    public async Task<PaginatedResult<CourseDto>> GetAllAsync(string? searchTerm, string? status, int page, int pageSize)
    {
        CourseStatus? courseStatus = null;
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<CourseStatus>(status, true, out var parsedStatus))
        {
            courseStatus = parsedStatus;
        }

        var courses = await _courseRepository.GetAllAsync(searchTerm, courseStatus, page, pageSize);
        var totalCount = await _courseRepository.GetTotalCountAsync(searchTerm, courseStatus);

        return new PaginatedResult<CourseDto>
        {
            Items = courses.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
    {
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Status = CourseStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _courseRepository.AddAsync(course);
        await _courseRepository.SaveChangesAsync();

        return MapToDto(course);
    }

    public async Task<CourseDto> UpdateAsync(Guid courseId, UpdateCourseDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course is null) throw new NotFoundException("Curso no encontrado");

        course.Title = dto.Title;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);
        await _courseRepository.SaveChangesAsync();

        return MapToDto(course);
    }

    public async Task PublishAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdWithLessonsAsync(courseId);

        if (course is null)
            throw new NotFoundException("Curso no encontrado");

        var hasActiveLessons = course.Lessons.Any(l => !l.IsDeleted);

        if (!hasActiveLessons)
            throw new BusinessRuleException(
                "Un curso debe tener al menos una lección activa para ser publicado");

        course.Status = CourseStatus.Published;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);
        await _courseRepository.SaveChangesAsync();
    }

    public async Task UnpublishAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);

        if (course is null)
            throw new NotFoundException("Curso no encontrado");

        course.Status = CourseStatus.Draft;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);
        await _courseRepository.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);

        if (course is null)
            throw new NotFoundException("Curso no encontrado");

        course.IsDeleted = true;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);
        await _courseRepository.SaveChangesAsync();
    }

    private static CourseDto MapToDto(Course course)
    {
        return new CourseDto
        {
            Id = course.Id,
            Title = course.Title,
            Status = (int)course.Status,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt
        };
    }
}