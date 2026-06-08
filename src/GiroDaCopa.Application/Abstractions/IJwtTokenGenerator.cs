namespace GiroDaCopa.Application.Abstractions;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAt) GenerateToken(
        Guid userId,
        string username,
        string role);
}
