using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizzify.DataContext;

namespace EntityFrameworkExample.DataAccess.Configurations;

public class PackageModelConfiguration : IEntityTypeConfiguration<Package>
{
    void IEntityTypeConfiguration<Package>.Configure(EntityTypeBuilder<Package> builder)
    {
        builder.HasKey(e => e.PackageId).HasName("packages_pkey");

        builder.ToTable("packages");

        builder.Property(e => e.PackageId).HasColumnName("package_id");

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_DATE")
            .HasColumnName("created_at");

        builder.Property(e => e.Difficulty).HasColumnName("difficulty");

        builder.Property(e => e.PackageName)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("package_name");
    }
}

