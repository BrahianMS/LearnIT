using System.ComponentModel.DataAnnotations;

namespace LearnIT.Application.DTOs.Lessons;


public class UpdateLessonDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;
}