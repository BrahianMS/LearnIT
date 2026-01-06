namespace LearnIT.Application.DTOs.Lessons;

public class LessonDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public int Order { get; set; }
}