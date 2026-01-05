using System.ComponentModel.DataAnnotations;

namespace LearnIT.Application.DTOs.Courses;

public class UpdateCourseDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;
}