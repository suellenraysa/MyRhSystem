using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyRhSystem.Contracts.Common;
using MyRhSystem.Contracts.Employees;
using MyRhSystem.Domain.Entities.Employees;
using MyRhSystem.Infrastructure.Persistence;

public static class EmployeesEndpoints
{
    public static RouteGroupBuilder MapEmployeesEndpoints(this RouteGroupBuilder g)
    {
        // GET paginado
        g.MapGet("", async (AppDbContext db, IMapper mapper, [AsParameters] PagedRequest req) =>
        {
            var q = db.Set<Employee>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                var s = req.Search.Trim();
                q = q.Where(e =>
                    e.Nome.Contains(s) ||
                    e.Email!.Contains(s) ||
                    e.Departamento.Contains(s) ||
                    e.Cargo.Contains(s));
            }

            var total = await q.CountAsync();

            var items = await q
                .OrderBy(e => e.Nome)
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .ProjectTo<EmployeeDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            return Results.Ok(new PagedResponse<EmployeeDto>(items, req.Page, req.PageSize, total));
        });

        // GET detalhes
        g.MapGet("{id:int}", async (AppDbContext db, IMapper mapper, int id) =>
        {
            var ent = await db.Set<Employee>()
                .AsNoTracking()
                .Include(e => e.Dependents)
                .Include(e => e.Contacts)
                .Include(e => e.EmployeeBenefits).ThenInclude(b => b.BenefitType)
                .FirstOrDefaultAsync(e => e.Id == id);

            return ent is null ? Results.NotFound() : Results.Ok(mapper.Map<EmployeeDetailsDto>(ent));
        });

        // POST create
        g.MapPost("", async (AppDbContext db, IMapper mapper, CreateEmployeesRequest req) =>
        {
            // exemplo de unicidade opcional (ajuste se precisar)
            if (!string.IsNullOrWhiteSpace(req.Email))
            {
                var exists = await db.Set<Employee>().AnyAsync(e => e.Email == req.Email);
                if (exists) return Results.Conflict("E-mail já cadastrado para outro funcionário.");
            }

            var ent = mapper.Map<Employee>(req);
            db.Add(ent);
            await db.SaveChangesAsync();

            var dto = mapper.Map<EmployeeDetailsDto>(ent);
            return Results.Created($"/api/employees/{ent.Id}", dto);
        });

        // PUT update
        g.MapPut("{id:int}", async (AppDbContext db, IMapper mapper, int id, UpdateEmployeesRequest req) =>
        {
            if (id != req.Id) return Results.BadRequest();

            var ent = await db.Set<Employee>()
                .Include(e => e.Dependents)
                .Include(e => e.Contacts)
                .Include(e => e.EmployeeBenefits)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ent is null) return Results.NotFound();

            // Se fizer unicidade de e-mail (exclui o próprio)
            if (!string.IsNullOrWhiteSpace(req.Email))
            {
                var emailExists = await db.Set<Employee>().AnyAsync(e => e.Email == req.Email && e.Id != id);
                if (emailExists) return Results.Conflict("E-mail já cadastrado para outro funcionário.");
            }

            // Atualiza campos simples
            mapper.Map(req, ent);

            // Se for atualizar coleções (dependentes/contatos/benefícios) faça aqui a lógica de upsert/exclusão

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // DELETE
        g.MapDelete("{id:int}", async (AppDbContext db, int id) =>
        {
            var ent = await db.Set<Employee>().FindAsync(id);
            if (ent is null) return Results.NotFound();

            db.Remove(ent);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return g;
    }
}
