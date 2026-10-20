using Backend.Domain.Entities;

namespace Backend.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, string? displayName = null);
}
