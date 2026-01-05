using LearnIT.Domain.Entities;

namespace LearnIT.Application.Interfaces.Repositories;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id);

    Task<Course?> GetByIdWithLessonsAsync(Guid id);

    Task AddAsync(Course course);

    Task UpdateAsync(Course course);

    Task SaveChangesAsync();
}