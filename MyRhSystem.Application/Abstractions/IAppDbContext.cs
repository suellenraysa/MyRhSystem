using Microsoft.EntityFrameworkCore;
using MyRhSystem.Domain.Entities.Users;

namespace MyRhSystem.Application.Abstractions;

public interface IAppDbContext
{
    DbSet<User> Users { get; }


    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
