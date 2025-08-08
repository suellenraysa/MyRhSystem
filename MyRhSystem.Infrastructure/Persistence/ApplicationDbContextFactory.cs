using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualBasic;

namespace MyRhSystem.Infrastructure.Persistence;

public class ApplicationDbContextFactory
       : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Substituído FileSystem.AppDataDirectory por Environment.GetFolderPath
        var dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "hrsystem.db"
        );

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;

        return new ApplicationDbContext(options);
    }
}