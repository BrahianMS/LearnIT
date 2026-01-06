namespace LearnIT.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}
