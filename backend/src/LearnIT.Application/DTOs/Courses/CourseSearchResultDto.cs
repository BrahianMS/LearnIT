namespace LearnIT.Application.DTOs.Courses;

public class CourseSearchResultDto
{
    public int TotalItems { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }

    public List<CourseItemDto> Items { get; set; } = new();
}

public class CourseItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime UpdatedAt { get; set; }
}
