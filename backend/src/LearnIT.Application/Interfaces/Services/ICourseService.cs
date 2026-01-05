namespace LearnIT.Application.Interfaces.Services;

public interface ICourseService
{
    Task PublishAsync(Guid courseId);

    Task UnpublishAsync(Guid courseId);

    Task SoftDeleteAsync(Guid courseId);
}
