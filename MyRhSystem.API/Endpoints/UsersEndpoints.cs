using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyRhSystem.Contracts.Common;
using MyRhSystem.Contracts.Users;
using MyRhSystem.Domain.Entities.Users;
using MyRhSystem.Infrastructure.Persistence;

public static class UsersEndpoints
{
    public static RouteGroupBuilder MapUsersEndpoints(this RouteGroupBuilder g)
    {
        g.MapGet("", async (AppDbContext db, IMapper mapper, [AsParameters] PagedRequest req) =>
        {
            var q = db.Set<User>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                var s = req.Search.Trim();
                q = q.Where(u => u.Nome.Contains(s) || u.Email.Contains(s));
            }

            var total = await q.CountAsync();
            var items = await q.OrderBy(u => u.Nome)
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .ProjectTo<UserDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            return Results.Ok(new PagedResponse<UserDto>(items, req.Page, req.PageSize, total));
        });

        g.MapGet("{id:int}", async (AppDbContext db, IMapper mapper, int id) =>
        {
            var ent = await db.Set<User>().FindAsync(id);
            return ent is null ? Results.NotFound() : Results.Ok(mapper.Map<UserDto>(ent));
        });

        g.MapPost("", async (AppDbContext db, IMapper mapper, CreateUserRequest req) =>
        {
            // e-mail único
            var exists = await db.Set<User>().AnyAsync(u => u.Email == req.Email);
            if (exists) return Results.Conflict("E-mail já cadastrado.");

            var ent = mapper.Map<User>(req);
            ent.Password = BCrypt.Net.BCrypt.HashPassword(req.Password);
            ent.CreatedAt = DateTime.UtcNow;

            db.Add(ent);
            await db.SaveChangesAsync();
            return Results.Created($"/api/users/{ent.Id}", mapper.Map<UserDto>(ent));
        });

        g.MapPut("{id:int}", async (AppDbContext db, IMapper mapper, int id, UpdateUserRequest req) =>
        {
            if (id != req.Id) return Results.BadRequest();

            var ent = await db.Set<User>().FirstOrDefaultAsync(u => u.Id == id);
            if (ent is null) return Results.NotFound();

            // e-mail único (excluindo o próprio)
            var emailExists = await db.Set<User>().AnyAsync(u => u.Email == req.Email && u.Id != id);
            if (emailExists) return Results.Conflict("E-mail já cadastrado.");

            ent.Nome = req.Nome;
            ent.Email = req.Email;
            if (!string.IsNullOrWhiteSpace(req.NewPassword))
                ent.Password = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        g.MapDelete("{id:int}", async (AppDbContext db, int id) =>
        {
            var ent = await db.Set<User>().FindAsync(id);
            if (ent is null) return Results.NotFound();
            db.Remove(ent);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return g;
    }
}
