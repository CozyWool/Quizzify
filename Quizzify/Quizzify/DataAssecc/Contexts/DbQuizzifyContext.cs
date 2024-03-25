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
    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Round> Rounds { get; set; }

    public virtual DbSet<Secretquestion> Secretquestions { get; set; }

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

        modelBuilder.ApplyConfiguration(new SecretquestionConfiguration());

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
