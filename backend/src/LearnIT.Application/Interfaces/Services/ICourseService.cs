using LearnIT.Application.DTOs.Common;
using LearnIT.Application.DTOs.Course;

namespace LearnIT.Application.Interfaces.Services;

public interface ICourseService
{
    Task<CourseSummaryDto> GetSummaryAsync(Guid courseId);
    Task<CourseDto> GetByIdAsync(Guid courseId);
    Task<PaginatedResult<CourseDto>> GetAllAsync(string? searchTerm, string? status, int page, int pageSize);
    Task<CourseDto> CreateAsync(CreateCourseDto dto);
    Task<CourseDto> UpdateAsync(Guid courseId, UpdateCourseDto dto);

    Task PublishAsync(Guid courseId);

    Task UnpublishAsync(Guid courseId);

    Task SoftDeleteAsync(Guid courseId);
}
