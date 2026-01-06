using System.ComponentModel.DataAnnotations;

namespace LearnIT.Application.DTOs.Lessons;

public class CreateLessonDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int Order { get; set; }
}