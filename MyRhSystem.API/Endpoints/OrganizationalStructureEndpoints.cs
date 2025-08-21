using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyRhSystem.Contracts.Departments;
using MyRhSystem.Contracts.JobRole;
using MyRhSystem.Domain.Entities.Departments;
using MyRhSystem.Domain.Entities.JobRoles;
using MyRhSystem.Infrastructure.Persistence;

namespace MyRhSystem.API.Endpoints;

public static class OrganizationalStructureEndpoints
{
    public static IEndpointRouteBuilder MapOrganizationalStructureEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/org/{companyId:int}")
                   .WithTags("OrganizationalStructure");

        // ---------------- Departments ----------------
        g.MapGet("/departments", async (int companyId, AppDbContext db, IMapper mapper, CancellationToken ct) =>
        {
            var data = await db.Departments
                .Where(d => d.CompanyId == companyId)
                .OrderBy(d => d.Nome)
                .ProjectTo<DepartmentDto>(mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return Results.Ok(data);
        });

        g.MapPost("/departments", async (int companyId, DepartmentDto dto, AppDbContext db, IMapper mapper, CancellationToken ct) =>
        {
            var entity = mapper.Map<Department>(dto);
            entity.CompanyId = companyId;

            db.Departments.Add(entity);
            await db.SaveChangesAsync(ct);

            var outDto = mapper.Map<DepartmentDto>(entity);
            return Results.Created($"/api/org/{companyId}/departments/{entity.Id}", outDto);
        });

        g.MapPut("/departments/{id:int}", async (int companyId, int id, DepartmentDto dto, AppDbContext db, IMapper mapper, CancellationToken ct) =>
        {
            var entity = await db.Departments.FirstOrDefaultAsync(d => d.Id == id && d.CompanyId == companyId, ct);
            if (entity is null) return Results.NotFound();

            entity.Nome = dto.Nome;
            await db.SaveChangesAsync(ct);

            return Results.Ok(mapper.Map<DepartmentDto>(entity));
        });

        g.MapDelete("/departments/{id:int}", async (int companyId, int id, AppDbContext db, CancellationToken ct) =>
        {
            var entity = await db.Departments.FirstOrDefaultAsync(d => d.Id == id && d.CompanyId == companyId, ct);
            if (entity is null) return Results.NotFound();

            db.Departments.Remove(entity);
            await db.SaveChangesAsync(ct);
            return Results.NoContent();
        });

        // ---------------- Levels ----------------
        g.MapGet("/levels", async (int companyId, AppDbContext db, IMapper mapper, CancellationToken ct) =>
        {
            var data = await db.JobLevels
                .Where(l => l.CompanyId == companyId)
                .OrderBy(l => l.Nome)
                .ProjectTo<JobLevelDto>(mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return Results.Ok(data);
        });

        g.MapPost("/levels", async (int companyId, JobLevelDto dto, AppDbContext db, IMapper mapper, CancellationToken ct) =>
        {
            var entity = mapper.Map<JobLevels>(dto);
            entity.CompanyId = companyId;

            db.JobLevels.Add(entity);
            await db.SaveChangesAsync(ct);

            return Results.Created($"/api/org/{companyId}/levels/{entity.Id}", mapper.Map<JobLevelDto>(entity));
        });

        g.MapPut("/levels/{id:int}", async (int companyId, int id, JobLevelDto dto, AppDbContext db, IMapper mapper, CancellationToken ct) =>
        {
            var entity = await db.JobLevels.FirstOrDefaultAsync(l => l.Id == id && l.CompanyId == companyId, ct);
            if (entity is null) return Results.NotFound();

            entity.Nome = dto.Nome;
            await db.SaveChangesAsync(ct);

            return Results.Ok(mapper.Map<JobLevelDto>(entity));
        });

        g.MapDelete("/levels/{id:int}", async (int companyId, int id, AppDbContext db, CancellationToken ct) =>
        {
            var inUse = await db.JobRoles.AnyAsync(r => r.CompanyId == companyId && r.LevelId == id, ct);
            if (inUse) return Results.Conflict("Nível em uso por algum cargo.");

            var entity = await db.JobLevels.FirstOrDefaultAsync(l => l.Id == id && l.CompanyId == companyId, ct);
            if (entity is null) return Results.NotFound();

            db.JobLevels.Remove(entity);
            await db.SaveChangesAsync(ct);
            return Results.NoContent();
        });

        // ---------------- Roles ----------------
        g.MapGet("/roles", async (int companyId, int? departmentId, AppDbContext db, IMapper mapper, CancellationToken ct) =>
        {
            var query = db.JobRoles
                .Include(r => r.Department)
                .Include(r => r.Level)
                .Where(r => r.CompanyId == companyId);

            if (departmentId is not null)
                query = query.Where(r => r.DepartmentId == departmentId);

            var data = await query
                .OrderBy(r => r.Nome)
                .ProjectTo<JobRoleDto>(mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return Results.Ok(data);
        });

        g.MapPost("/roles", async (int companyId, JobRoleDto dto, AppDbContext db, IMapper mapper, CancellationToken ct) =>
        {
            // valida se dept/level são da mesma empresa
            var okDept = await db.Departments.AnyAsync(d => d.Id == dto.DepartmentId && d.CompanyId == companyId, ct);
            var okLvl = await db.JobLevels.AnyAsync(l => l.Id == dto.LevelId && l.CompanyId == companyId, ct);
            if (!okDept || !okLvl) return Results.BadRequest("Departamento/Nível inválidos para esta empresa.");

            var entity = mapper.Map<JobRole>(dto);
            entity.CompanyId = companyId;

            db.JobRoles.Add(entity);
            await db.SaveChangesAsync(ct);

            return Results.Created($"/api/org/{companyId}/roles/{entity.Id}", mapper.Map<JobRoleDto>(entity));
        });

        g.MapPut("/roles/{id:int}", async (int companyId, int id, JobRoleDto dto, AppDbContext db, IMapper mapper, CancellationToken ct) =>
        {
            var entity = await db.JobRoles.FirstOrDefaultAsync(r => r.Id == id && r.CompanyId == companyId, ct);
            if (entity is null) return Results.NotFound();

            if (!await db.Departments.AnyAsync(d => d.Id == dto.DepartmentId && d.CompanyId == companyId, ct))
                return Results.BadRequest("Departamento inválido.");

            if (!await db.JobLevels.AnyAsync(l => l.Id == dto.LevelId && l.CompanyId == companyId, ct))
                return Results.BadRequest("Nível inválido.");

            entity.Nome = dto.Nome;
            entity.DepartmentId = dto.DepartmentId;
            entity.LevelId = dto.LevelId;
            entity.SalarioBase = dto.SalarioBase;
            entity.SalarioMaximo = dto.SalarioMaximo;
            entity.Requisitos = dto.Requisitos;
            entity.Responsabilidades = dto.Responsabilidades;

            await db.SaveChangesAsync(ct);
            return Results.Ok(mapper.Map<JobRoleDto>(entity));
        });

        g.MapDelete("/roles/{id:int}", async (int companyId, int id, AppDbContext db, CancellationToken ct) =>
        {
            var entity = await db.JobRoles.FirstOrDefaultAsync(r => r.Id == id && r.CompanyId == companyId, ct);
            if (entity is null) return Results.NotFound();

            db.JobRoles.Remove(entity);
            await db.SaveChangesAsync(ct);
            return Results.NoContent();
        });

        return app;
    }
}
