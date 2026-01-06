using LearnIT.Application.Interfaces.Repositories;
using LearnIT.Domain.Entities;
using LearnIT.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LearnIT.Infrastructure.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly LearnITDbContext _context;

    public LessonRepository(LearnITDbContext context)
    {
        _context = context;
    }

    public async Task<Lesson?> GetByIdAsync(Guid id)
    {
        return await _context.Lessons
            .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted);
    }

    public async Task<List<Lesson>> GetByCourseIdAsync(Guid courseId)
    {
        return await _context.Lessons
            .Where(l => l.CourseId == courseId && !l.IsDeleted)
            .OrderBy(l => l.Order)
            .ToListAsync();
    }

    public async Task<bool> ExistsWithOrderAsync(Guid courseId, int order)
    {
        return await _context.Lessons
            .AnyAsync(l =>
                l.CourseId == courseId &&
                l.Order == order &&
                !l.IsDeleted
            );
    }

    public async Task<Lesson?> GetByOrderAsync(Guid courseId, int order)
    {
        return await _context.Lessons
            .FirstOrDefaultAsync(l => l.CourseId == courseId && l.Order == order && !l.IsDeleted);
    }

    public async Task AddAsync(Lesson lesson)
    {
        await _context.Lessons.AddAsync(lesson);
    }

    public Task UpdateAsync(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}