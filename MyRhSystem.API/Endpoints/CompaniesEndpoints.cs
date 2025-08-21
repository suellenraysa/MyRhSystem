using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyRhSystem.APP.Shared.Models;
using MyRhSystem.Contracts.Common;
using MyRhSystem.Contracts.Companies;
using MyRhSystem.Domain.Entities.Companies;
using MyRhSystem.Domain.Entities.ValueObjects;
using MyRhSystem.Infrastructure.Persistence;
using System.Security.Claims;

namespace MyRhSystem.API.Endpoints;

public static class CompaniesEndpoints
{
    public static RouteGroupBuilder MapCompaniesEndpoints(this RouteGroupBuilder g)
    {
        g.MapGet("", async (AppDbContext db, IMapper mapper, [AsParameters] PagedRequest req) =>
        {
            var q = db.Set<Company>()
                      .AsNoTracking();
                      

            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                var s = req.Search.Trim();
                q = q.Where(c =>
                    (c.Nome != null && c.Nome.Contains(s)) ||
                    (c.Cnpj != null && c.Cnpj.Contains(s)) ||
                    (c.Email != null && c.Email.Contains(s)) ||
                    (c.Telefone != null && c.Telefone.Contains(s))
                );
            }

            var total = await q.CountAsync();

            var items = await q
                .OrderBy(c => c.Nome)
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .ProjectTo<CompanyDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            return Results.Ok(new PagedResponse<CompanyDTO>(items, req.Page, req.PageSize, total));
        });

        g.MapGet("{id:int}", async (AppDbContext db, IMapper mapper, int id) =>
        {
            var ent = await db.Set<Company>()
                              .AsNoTracking()
                              .Include(c => c.Employees)
                              .Include(c => c.UserCompanies)
                              .FirstOrDefaultAsync(c => c.Id == id);

            return ent is null
                ? Results.NotFound()
                : Results.Ok(mapper.Map<CompanyDTO>(ent));
        });

        g.MapPost("", async (AppDbContext db, IMapper mapper, HttpContext http, CreateCompanyRequest req) =>
        {

            var userIdClaim = http.User.FindFirst("sub")?.Value
                  ?? http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Results.Unauthorized();
            var userId = int.Parse(userIdClaim);

            // --- Regras gerais de unicidade
            if (!string.IsNullOrWhiteSpace(req.Cnpj) &&
                await db.Set<Company>().AnyAsync(c => c.Cnpj == req.Cnpj))
                return Results.Conflict("CNPJ já cadastrado.");

            if (!string.IsNullOrWhiteSpace(req.Email) &&
                await db.Set<Company>().AnyAsync(c => c.Email == req.Email))
                return Results.Conflict("E-mail da empresa já cadastrado.");

            // Representante 1:1 — se vier objeto novo com CPF já existente, bloqueia
            if (req.Representative is not null &&
                await db.Set<LegalRepresentativeModel>().AnyAsync(r => r.CPF == req.Representative.Cpf))
                return Results.Conflict("CPF do representante já cadastrado.");

            // --- Evitar ambiguidade (não aceitar ID e objeto juntos)
            if (req.AddressId.HasValue && req.Address is not null)
                return Results.BadRequest("Envie AddressId OU Address (não ambos).");

            if (req.RepresentativeId.HasValue && req.Representative is not null)
                return Results.BadRequest("Envie RepresentativeId OU Representative (não ambos).");

            // --- Validar IDs existentes (se enviados)
            if (req.AddressId.HasValue &&
                !await db.Set<Address>().AnyAsync(a => a.Id == req.AddressId.Value))
                return Results.BadRequest("AddressId inválido.");

            if (req.RepresentativeId.HasValue)
            {
                // precisa existir...
                var exists = await db.Set<LegalRepresentativeModel>().AnyAsync(r => r.Id == req.RepresentativeId.Value);
                if (!exists) return Results.BadRequest("RepresentativeId inválido.");

                // ...e não pode estar vinculado a outra empresa (1:1)
                var alreadyUsed = await db.Set<Company>()
                    .AnyAsync(c => c.RepresentativeId == req.RepresentativeId.Value);
                if (alreadyUsed) return Results.Conflict("Este representante já está vinculado a outra empresa.");
            }

            
            // --- Montar entidade (aninhados são mapeados)
            var ent = mapper.Map<Company>(req);
            ent.CreatedAt = DateTime.UtcNow;
            ent.CreatedByUserId = userId;

            // Se IDs foram enviados, force usar FK e não inserir entidades duplicadas
            if (req.AddressId.HasValue) { ent.Address = null; ent.AddressId = req.AddressId; }
            if (req.RepresentativeId.HasValue) { ent.Representative = null; ent.RepresentativeId = req.RepresentativeId; }

            // --- Persistir (EF insere Address/Representative aninhados antes, depois Company com FKs)
            db.Add(ent);
            await db.SaveChangesAsync();

            db.Add(new UserCompany { UserId = userId, CompanyId = ent.Id, Role = "Admin", CreatedAt = DateTime.UtcNow });
            await db.SaveChangesAsync();

            var dto = mapper.Map<CompanyDTO>(ent);
            return Results.Created($"/api/companies/{ent.Id}", dto);
        });

        g.MapPut("{id:int}", async (AppDbContext db, IMapper mapper, int id, UpdateCompanyRequest req) =>
        {
            if (id != req.Id) return Results.BadRequest();

            // Carrega a empresa com as navegações 1:1
            var ent = await db.Set<Company>()
                .Include(c => c.Address)
                .Include(c => c.Representative)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (ent is null) return Results.NotFound();

            // ---- Validações de unicidade (excluindo a própria)
            if (!string.IsNullOrWhiteSpace(req.Cnpj))
            {
                var cnpjExists = await db.Set<Company>()
                    .AnyAsync(c => c.Cnpj == req.Cnpj && c.Id != id);
                if (cnpjExists) return Results.Conflict("CNPJ já cadastrado.");
            }

            if (!string.IsNullOrWhiteSpace(req.Email))
            {
                var emailExists = await db.Set<Company>()
                    .AnyAsync(c => c.Email == req.Email && c.Id != id);
                if (emailExists) return Results.Conflict("E-mail da empresa já cadastrado.");
            }

            // Impedir ambiguidade: ID + objeto juntos
            if (req.AddressId.HasValue && req.Address is not null)
                return Results.BadRequest("Envie AddressId OU Address (não ambos).");

            if (req.RepresentativeId.HasValue && req.Representative is not null)
                return Results.BadRequest("Envie RepresentativeId OU Representative (não ambos).");

            // ---- Base: atualizar campos simples da empresa (patch)
            ent.Nome = req.Nome ?? ent.Nome;
            ent.Cnpj = req.Cnpj ?? ent.Cnpj;
            ent.Telefone = req.Telefone ?? ent.Telefone;
            ent.Email = req.Email ?? ent.Email;

            // =======================
            // Address (FK opcional)
            // =======================
            if (req.AddressId.HasValue)
            {
                // trocar para Address existente
                var ok = await db.Set<Address>().AnyAsync(a => a.Id == req.AddressId.Value);
                if (!ok) return Results.BadRequest("AddressId inválido.");

                ent.Address = null;                // evita reinsert indevido
                ent.AddressId = req.AddressId;     // aponta para o novo/endereço existente
            }
            else if (req.Address is not null)
            {
                if (ent.AddressId.HasValue && ent.Address is not null)
                {
                    // editar o Address já vinculado (patch)
                    mapper.Map(req.Address, ent.Address);
                }
                else
                {
                    // criar novo Address e vincular
                    var newAddr = mapper.Map<Address>(req.Address);
                    db.Add(newAddr);
                    await db.SaveChangesAsync();   // gera newAddr.Id

                    ent.AddressId = newAddr.Id;
                    ent.Address = newAddr;
                }
            }
            // se nem AddressId nem Address foram enviados, não mexe no endereço atual

            // =====================================
            // Representative (1:1) — FK deve ser única
            // =====================================
            if (req.RepresentativeId.HasValue)
            {
                // precisa existir e não pode estar vinculado a outra empresa
                var repExists = await db.Set<LegalRepresentativeModel>()
                    .AnyAsync(r => r.Id == req.RepresentativeId.Value);
                if (!repExists) return Results.BadRequest("RepresentativeId inválido.");

                var inUse = await db.Set<Company>()
                    .AnyAsync(c => c.RepresentativeId == req.RepresentativeId.Value && c.Id != id);
                if (inUse) return Results.Conflict("Este representante já está vinculado a outra empresa.");

                ent.Representative = null;                 // evita reinsert/duplicidade
                ent.RepresentativeId = req.RepresentativeId;
            }
            else if (req.Representative is not null)
            {
                if (ent.RepresentativeId.HasValue && ent.Representative is not null)
                {
                    // editar o representante já vinculado (patch)
                    // valida unicidade de CPF se você mantém constraint lógica
                    if (!string.IsNullOrWhiteSpace(req.Representative.Cpf))
                    {
                        var cpfInUse = await db.Set<LegalRepresentativeModel>()
                            .AnyAsync(r => r.CPF == req.Representative.Cpf && r.Id != ent.Representative.Id);
                        if (cpfInUse) return Results.Conflict("CPF do representante já cadastrado.");
                    }

                    mapper.Map(req.Representative, ent.Representative);
                }
                else
                {
                    // criar novo representante e vincular (garantir CPF único)
                    if (!string.IsNullOrWhiteSpace(req.Representative.Cpf))
                    {
                        var cpfInUse = await db.Set<LegalRepresentativeModel>()
                            .AnyAsync(r => r.CPF == req.Representative.Cpf);
                        if (cpfInUse) return Results.Conflict("CPF do representante já cadastrado.");
                    }

                    var newRep = mapper.Map<LegalRepresentativeModel>(req.Representative);
                    db.Add(newRep);
                    await db.SaveChangesAsync();   // gera newRep.Id

                    ent.RepresentativeId = newRep.Id;
                    ent.Representative = newRep;
                }
            }
            // se nem RepresentativeId nem Representative vieram, mantém vínculo atual

            await db.SaveChangesAsync();
            return Results.NoContent();
        });


        return g;
    }


}
