using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Quizzify.DataAccess;

namespace EntityFrameworkExample.Migrations;

public class DbquizzifyContextFactory : IDesignTimeDbContextFactory<DbquizzifyContext>
{
    public DbquizzifyContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DbquizzifyContext>();
        optionsBuilder.UseNpgsql(builder =>
        {
            builder.MigrationsAssembly(typeof(DbquizzifyContext).Assembly.FullName);
        });
        return new DbquizzifyContext(optionsBuilder.Options);
    }
}