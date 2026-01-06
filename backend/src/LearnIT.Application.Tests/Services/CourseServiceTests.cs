using LearnIT.Application.DTOs.Course;
using LearnIT.Application.Exceptions;
using LearnIT.Application.Interfaces.Repositories;
using LearnIT.Application.Services;
using LearnIT.Domain.Entities;
using Moq;
using Xunit;

namespace LearnIT.Application.Tests.Services;

public class CourseServiceTests
{
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly CourseService _courseService;

    public CourseServiceTests()
    {
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _courseService = new CourseService(_courseRepositoryMock.Object);
    }

    [Fact]
    public async Task PublishCourse_WithLessons_ShouldSucceed()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Course
        {
            Id = courseId,
            Status = CourseStatus.Draft,
            Lessons = new List<Lesson>
            {
                new Lesson { Id = Guid.NewGuid(), IsDeleted = false }
            }
        };

        _courseRepositoryMock.Setup(r => r.GetByIdWithLessonsAsync(courseId))
            .ReturnsAsync(course);

        // Act
        await _courseService.PublishAsync(courseId);

        // Assert
        Assert.Equal(CourseStatus.Published, course.Status);
        _courseRepositoryMock.Verify(r => r.UpdateAsync(course), Times.Once);
        _courseRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task PublishCourse_WithoutLessons_ShouldFail()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Course
        {
            Id = courseId,
            Status = CourseStatus.Draft,
            Lessons = new List<Lesson>() // No lessons
        };

        _courseRepositoryMock.Setup(r => r.GetByIdWithLessonsAsync(courseId))
            .ReturnsAsync(course);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => _courseService.PublishAsync(courseId));
        _courseRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Course>()), Times.Never);
    }

    [Fact]
    public async Task DeleteCourse_ShouldBeSoftDelete()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Course
        {
            Id = courseId,
            IsDeleted = false
        };

        _courseRepositoryMock.Setup(r => r.GetByIdAsync(courseId))
            .ReturnsAsync(course);

        // Act
        await _courseService.SoftDeleteAsync(courseId);

        // Assert
        Assert.True(course.IsDeleted);
        _courseRepositoryMock.Verify(r => r.UpdateAsync(course), Times.Once);
    }
}