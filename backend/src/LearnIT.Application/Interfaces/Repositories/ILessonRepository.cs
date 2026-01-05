using LearnIT.Domain.Entities;

namespace LearnIT.Application.Interfaces.Repositories;

public interface ILessonRepository
{
    Task<Lesson?> GetByIdAsync(Guid id);

    Task<List<Lesson>> GetByCourseIdAsync(Guid courseId);

    Task<bool> ExistsWithOrderAsync(Guid courseId, int order);

    Task AddAsync(Lesson lesson);

    Task UpdateAsync(Lesson lesson);

    Task SaveChangesAsync();
}