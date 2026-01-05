using LearnIT.Application.Exceptions;
using LearnIT.Application.Interfaces.Repositories;
using LearnIT.Application.Services;
using LearnIT.Domain.Entities;
using Moq;
using Xunit;

namespace LearnIT.Application.Tests.Services;

public class CourseServiceTests
{
    [Fact]
    public async Task PublishCourse_WithLessons_ShouldSucceed()
    {
        var courseId = Guid.NewGuid();

        var course = new Course
        {
            Id = courseId,
            Title = "Test Course",
            Status = CourseStatus.Draft,
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = Guid.NewGuid(),
                    Title = "Lesson 1",
                    IsDeleted = false
                }
            }
        };

        var courseRepositoryMock = new Mock<ICourseRepository>();

        courseRepositoryMock
            .Setup(repo => repo.GetByIdWithLessonsAsync(courseId))
            .ReturnsAsync(course);

        var service = new CourseService(courseRepositoryMock.Object);

        await service.PublishAsync(courseId);

        Assert.Equal(CourseStatus.Published, course.Status);

        courseRepositoryMock.Verify(
            repo => repo.SaveChangesAsync(),
            Times.Once
        );
    }
    
    [Fact]
    public async Task PublishCourse_WithoutLessons_ShouldFail()
    {
        var courseId = Guid.NewGuid();

        var course = new Course
        {
            Id = courseId,
            Title = "Test Course",
            Status = CourseStatus.Draft,
            Lessons = new List<Lesson>()
        };

        var courseRepositoryMock = new Mock<ICourseRepository>();

        courseRepositoryMock
            .Setup(repo => repo.GetByIdWithLessonsAsync(courseId))
            .ReturnsAsync(course);

        var service = new CourseService(courseRepositoryMock.Object);
        
        await Assert.ThrowsAsync<BusinessRuleException>(
            () => service.PublishAsync(courseId)
        );
        
        courseRepositoryMock.Verify(
            repo => repo.SaveChangesAsync(),
            Times.Never
        );
    }
    
    [Fact]
    public async Task DeleteCourse_ShouldBeSoftDelete()
    {
        var courseId = Guid.NewGuid();

        var course = new Course
        {
            Id = courseId,
            Title = "Test Course",
            IsDeleted = false
        };

        var courseRepositoryMock = new Mock<ICourseRepository>();

        courseRepositoryMock
            .Setup(repo => repo.GetByIdAsync(courseId))
            .ReturnsAsync(course);

        var service = new CourseService(courseRepositoryMock.Object);

        await service.SoftDeleteAsync(courseId);

        Assert.True(course.IsDeleted);

        courseRepositoryMock.Verify(
            repo => repo.SaveChangesAsync(),
            Times.Once
        );
    }
}