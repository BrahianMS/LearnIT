using LearnIT.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnIT.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StatsController : ControllerBase
{
    private readonly ICourseRepository _courseRepository;

    public StatsController(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardStats()
    {
        // For simplicity, we are fetching counts. Ideally this should be in a Service.
        // We will misuse count methods or fetch all for now since we don't have dedicated count methods for status.
        // Actually, let's just fetch all and count in memory if dataset is small, or add repository methods.
        // Given constraints, I'll add a simple counting method in repository or just fetch all (not performant but works for MVP).
        // Let's rely on GetTotalCountAsync which exists.

        var totalCourses = await _courseRepository.GetTotalCountAsync(null, null);
        
        // We need breakdown. Let's add GetStatsAsync to IRepository or just fetch all?
        // Let's keep it simple: fetch all.
        var allCourses = await _courseRepository.GetAllAsync(null, null, 1, 1000); // Hacky but fast for MVP
        
        var totalLessons = await _courseRepository.GetTotalLessonsCountAsync();
        
        var stats = new 
        {
            TotalCourses = totalCourses,
            PublishedCourses = allCourses.Count(c => c.Status == Domain.Entities.CourseStatus.Published),
            DraftCourses = allCourses.Count(c => c.Status == Domain.Entities.CourseStatus.Draft),
            TotalLessons = totalLessons
        };

        return Ok(stats);
    }
}
