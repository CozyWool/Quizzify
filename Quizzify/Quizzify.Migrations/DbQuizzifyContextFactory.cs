using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Quizzify.DataAccess.Contexts;

namespace Quizzify.Migrations;

public class DbQuizzifyContextFactory : IDesignTimeDbContextFactory<DbQuizzifyContext>
{
    public DbQuizzifyContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DbQuizzifyContext>();
        optionsBuilder.UseNpgsql(builder =>
        {
            builder.MigrationsAssembly(typeof(DbQuizzifyContextFactory).Assembly.FullName);
        });
        return new DbQuizzifyContext(optionsBuilder.Options);
    }
}