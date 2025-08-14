namespace MyRhSystem.Contracts.Users;

public record UserDto(
    int Id,
    string Nome,
    string Email,
    DateTime CreatedAt);
