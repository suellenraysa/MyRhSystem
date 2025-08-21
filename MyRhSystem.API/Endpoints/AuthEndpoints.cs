using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyRhSystem.API.Helpers;
using MyRhSystem.Contracts.Auth;
using MyRhSystem.Contracts.Users;
using MyRhSystem.Domain.Entities.Users;
using MyRhSystem.Infrastructure.Persistence;

namespace MyRhSystem.API.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder g)
    {
        g.MapGet("has-user", async (AppDbContext db) =>
        {
            var any = await db.Set<User>().AsNoTracking().AnyAsync();
            return Results.Ok(new HasAnyUserResponse(any));
        });

        g.MapPost("login", async (AppDbContext db, IMapper mapper, IConfiguration cfg, LoginRequest req) =>
        {
            var user = await db.Set<User>().FirstOrDefaultAsync(u => u.Email == req.Email);
            if (user is null) return Results.Unauthorized();

            var ok = BCrypt.Net.BCrypt.Verify(req.Password, user.Password);
            if (!ok) return Results.Unauthorized();

            var dto = mapper.Map<UserDto>(user);

            var token = JwtTokenService.IssueToken(cfg, user);

            return Results.Ok(new LoginResponse(dto, token));
        });

        return g;
    }
}
