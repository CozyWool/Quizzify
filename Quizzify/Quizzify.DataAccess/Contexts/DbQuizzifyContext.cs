using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quizzify.DataAccess.Configuration;
using Quizzify.DataAccess.Entities;

namespace Quizzify.DataAccess.Contexts;

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

    public virtual DbSet<RoundEntity> Rounds { get; set; }

    public virtual DbSet<SecretQuestionEntity> Secretquestions { get; set; }

    public virtual DbSet<UserEntity> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
    }

    public void AddUser(UserEntity newUser)
    {
        Users.Add(newUser);
        SaveChanges();
    }

    public void AddPackage(PackageEntity newPackage)
    {
        Packages.Add(newPackage);
        SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PackageConfiguration());

        modelBuilder.ApplyConfiguration(new PlayerConfiguration());

        modelBuilder.ApplyConfiguration(new QuestionConfiguration());
        
        modelBuilder.ApplyConfiguration(new ThemeConfiguration());

        modelBuilder.ApplyConfiguration(new RoundConfiguration());

        modelBuilder.ApplyConfiguration(new SecretQuestionConfiguration());

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}