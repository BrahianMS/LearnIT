using LearnIT.Application.Exceptions;
using LearnIT.Application.Interfaces.Repositories;
using LearnIT.Application.Services;
using LearnIT.Domain.Entities;
using Moq;
using Xunit;

namespace LearnIT.Application.Tests.Services;

public class LessonServiceTests
{
    [Fact]
    public async Task CreateLesson_WithUniqueOrder_ShouldSucceed()
    {
        var courseId = Guid.NewGuid();

        var course = new Course
        {
            Id = courseId,
            Title = "Course"
        };

        var lesson = new Lesson
        {
            Title = "Lesson 1",
            Order = 1
        };

        var lessonRepositoryMock = new Mock<ILessonRepository>();
        var courseRepositoryMock = new Mock<ICourseRepository>();

        courseRepositoryMock
            .Setup(repo => repo.GetByIdAsync(courseId))
            .ReturnsAsync(course);

        lessonRepositoryMock
            .Setup(repo => repo.ExistsWithOrderAsync(courseId, lesson.Order))
            .ReturnsAsync(false);

        var service = new LessonService(
            lessonRepositoryMock.Object,
            courseRepositoryMock.Object
        );

        await service.CreateAsync(courseId, lesson);

        lessonRepositoryMock.Verify(
            repo => repo.AddAsync(It.IsAny<Lesson>()),
            Times.Once
        );

        lessonRepositoryMock.Verify(
            repo => repo.SaveChangesAsync(),
            Times.Once
        );
    }
    
    [Fact]
    public async Task CreateLesson_WithDuplicateOrder_ShouldFail()
    {
        var courseId = Guid.NewGuid();

        var course = new Course
        {
            Id = courseId,
            Title = "Course"
        };

        var lesson = new Lesson
        {
            Title = "Lesson 1",
            Order = 1
        };

        var lessonRepositoryMock = new Mock<ILessonRepository>();
        var courseRepositoryMock = new Mock<ICourseRepository>();

        courseRepositoryMock
            .Setup(repo => repo.GetByIdAsync(courseId))
            .ReturnsAsync(course);

        lessonRepositoryMock
            .Setup(repo => repo.ExistsWithOrderAsync(courseId, lesson.Order))
            .ReturnsAsync(true);

        var service = new LessonService(
            lessonRepositoryMock.Object,
            courseRepositoryMock.Object
        );

        await Assert.ThrowsAsync<BusinessRuleException>(
            () => service.CreateAsync(courseId, lesson)
        );

        lessonRepositoryMock.Verify(
            repo => repo.SaveChangesAsync(),
            Times.Never
        );
    }
}