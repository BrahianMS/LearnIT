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

    public async Task<List<Course>> GetAllAsync(string? searchTerm, CourseStatus? status, int page, int pageSize)
    {
        var query = _context.Courses.AsQueryable();

        query = query.Where(c => !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowerTerm = searchTerm.ToLower();
            query = query.Where(c => c.Title.ToLower().Contains(lowerTerm));
        }

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        return await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? searchTerm, CourseStatus? status)
    {
        var query = _context.Courses.AsQueryable();

        query = query.Where(c => !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowerTerm = searchTerm.ToLower();
            query = query.Where(c => c.Title.ToLower().Contains(lowerTerm));
        }

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        return await query.CountAsync();
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