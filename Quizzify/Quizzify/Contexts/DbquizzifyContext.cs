using System;
using System.Collections.Generic;
using EntityFrameworkExample.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quizzify.Configuration;
using Quizzify.DataContext;

namespace Quizzify.DataAccess;

public partial class DbquizzifyContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbquizzifyContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbquizzifyContext(DbContextOptions<DbquizzifyContext> options)
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
        modelBuilder.ApplyConfiguration(new PackageModelConfiguration());

        modelBuilder.ApplyConfiguration(new PlayerModelConfiguration());

        modelBuilder.ApplyConfiguration(new QuestionModelConfiguration());

        modelBuilder.ApplyConfiguration(new RoundModelConfiguration());

        modelBuilder.ApplyConfiguration(new SecretquestionModelConfiguration());

        modelBuilder.ApplyConfiguration(new UserModelConfiguration());
    }
}
