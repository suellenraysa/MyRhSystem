using MyRhSystem.Contracts.Users;

namespace MyRhSystem.Contracts.Auth;

public record LoginResponse(UserDto User, string? token);
