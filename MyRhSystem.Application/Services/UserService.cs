using Microsoft.EntityFrameworkCore;
using MyRhSystem.Domain.Entities.Users;
using MyRhSystem.Infrastructure.Persistence;
using System.Diagnostics;

namespace MyRhSystem.Application.Services;

public class UserService
{
    private readonly ApplicationDbContext _db;
    public UserService(ApplicationDbContext db) => _db = db;

    public Task<List<User>> GetAllAsync()
        => _db.Users.ToListAsync();

    public Task<User?> GetByIdAsync(int id)
        => _db.Users.FindAsync(id).AsTask();

    public async Task AddAsync(User u)
    {
        _db.Users.Add(u);
        Debug.WriteLine($">>> Seed1 start: Users before = {await _db.Users.CountAsync()}");
        await _db.SaveChangesAsync();
        Debug.WriteLine($">>> Seed1 end: Users after = {await _db.Users.CountAsync()}");
    }

    public async Task UpdateAsync(User u)
    {
        _db.Users.Update(u);
        Debug.WriteLine($">>> 2Seed start: Users before = {await _db.Users.CountAsync()}");
        await _db.SaveChangesAsync();
        Debug.WriteLine($">>>2 Seed end: Users after = {await _db.Users.CountAsync()}");
    }

    public async Task DeleteAsync(int id)
    {
        var u = await _db.Users.FindAsync(id);
        if (u != null)
        {
            _db.Users.Remove(u);
            await _db.SaveChangesAsync();
        }
    }
}
