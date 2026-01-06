using LearnIT.Domain.Entities;

namespace LearnIT.Application.DTOs.Course;

public class CourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateCourseDto
{
    public string Title { get; set; }
}

public class UpdateCourseDto
{
    public string Title { get; set; }
}

public class CourseSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public int TotalLessons { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}
