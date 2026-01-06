using LearnIT.Domain.Entities;

namespace LearnIT.Application.Interfaces.Repositories;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id);

    Task<Course?> GetByIdWithLessonsAsync(Guid id);

    Task<List<Course>> GetAllAsync(string? searchTerm, CourseStatus? status, int page, int pageSize);

    Task<int> GetTotalCountAsync(string? searchTerm, CourseStatus? status);

    Task<int> GetTotalLessonsCountAsync();

    Task AddAsync(Course course);

    Task UpdateAsync(Course course);

    Task SaveChangesAsync();
}