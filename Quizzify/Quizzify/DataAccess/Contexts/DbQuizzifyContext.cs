using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quizzify.DataAssecc.Configuration;
using Quizzify.DataAssecc.Entities;

namespace Quizzify.DataAssecc.Contexts;

public partial class DbQuizzifyContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbQuizzifyContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbQuizzifyContext(DbContextOptions<DbQuizzifyContext> options)
        : base(options)
    {
    }
    public virtual DbSet<PackageEntity> Packages { get; set; }

    public virtual DbSet<PlayerEntity> Players { get; set; }

    public virtual DbSet<QuestionEntity> Questions { get; set; }

    public virtual DbSet<Round> Rounds { get; set; }

    public virtual DbSet<SecretQuestion> Secretquestions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PackageConfiguration());

        modelBuilder.ApplyConfiguration(new PlayerConfiguration());

        modelBuilder.ApplyConfiguration(new QuestionConfiguration());

        modelBuilder.ApplyConfiguration(new RoundConfiguration());

        modelBuilder.ApplyConfiguration(new SecretQuestionConfiguration());

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
