using System.ComponentModel.DataAnnotations;

namespace LearnIT.Application.DTOs.Courses;

public class CreateCourseDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;
}