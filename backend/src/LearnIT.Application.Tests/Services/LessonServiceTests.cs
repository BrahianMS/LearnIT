using LearnIT.Application.DTOs.Lesson;
using LearnIT.Application.Exceptions;
using LearnIT.Application.Interfaces.Repositories;
using LearnIT.Application.Services;
using LearnIT.Domain.Entities;
using Moq;
using Xunit;

namespace LearnIT.Application.Tests.Services;

public class LessonServiceTests
{
    private readonly Mock<ILessonRepository> _lessonRepositoryMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly LessonService _lessonService;

    public LessonServiceTests()
    {
        _lessonRepositoryMock = new Mock<ILessonRepository>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _lessonService = new LessonService(_lessonRepositoryMock.Object, _courseRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateLesson_WithUniqueOrder_ShouldSucceed()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var dto = new CreateLessonDto
        {
            CourseId = courseId,
            Title = "Lesson 1",
            Order = 1
        };

        _courseRepositoryMock.Setup(r => r.GetByIdAsync(courseId))
            .ReturnsAsync(new Course { Id = courseId });

        _lessonRepositoryMock.Setup(r => r.ExistsWithOrderAsync(courseId, dto.Order))
            .ReturnsAsync(false);

        // Act
        var result = await _lessonService.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Title, result.Title);
        _lessonRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Lesson>()), Times.Once);
    }

    [Fact]
    public async Task CreateLesson_WithDuplicateOrder_ShouldFail()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var dto = new CreateLessonDto
        {
            CourseId = courseId,
            Title = "Lesson 2",
            Order = 1
        };

        _courseRepositoryMock.Setup(r => r.GetByIdAsync(courseId))
            .ReturnsAsync(new Course { Id = courseId });

        _lessonRepositoryMock.Setup(r => r.ExistsWithOrderAsync(courseId, dto.Order))
            .ReturnsAsync(true); // Duplicate order

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => _lessonService.CreateAsync(dto));
        _lessonRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Lesson>()), Times.Never);
    }
}