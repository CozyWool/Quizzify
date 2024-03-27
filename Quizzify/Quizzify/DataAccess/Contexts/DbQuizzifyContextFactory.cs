using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Quizzify.DataAssecc.Contexts;
public class DbQuizzifyContextFactory : IDesignTimeDbContextFactory<DbQuizzifyContext>
{
    public DbQuizzifyContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DbQuizzifyContext>();
        optionsBuilder.UseNpgsql(builder =>
        {
            builder.MigrationsAssembly(typeof(DbQuizzifyContext).Assembly.FullName);
        });
        return new DbQuizzifyContext(optionsBuilder.Options);
    }
}