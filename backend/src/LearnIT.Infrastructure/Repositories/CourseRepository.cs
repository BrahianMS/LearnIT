using LearnIT.Application.Interfaces.Repositories;
using LearnIT.Domain.Entities;
using LearnIT.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LearnIT.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly LearnITDbContext _context;

    public CourseRepository(LearnITDbContext context)
    {
        _context = context;
    }

    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }

    public async Task<Course?> GetByIdWithLessonsAsync(Guid id)
    {
        return await _context.Courses
            .Include(c => c.Lessons.Where(l => !l.IsDeleted))
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }

    public async Task AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
    }

    public Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}