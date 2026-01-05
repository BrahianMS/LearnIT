namespace LearnIT.Application.DTOs.Courses;

public class CourseSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int TotalLessons { get; set; }
    public DateTime UpdatedAt { get; set; }
}