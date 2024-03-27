using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizzify.DataAssecc.Entities;

namespace Quizzify.DataAssecc.Configuration;

public class PackageConfiguration : IEntityTypeConfiguration<PackageEntity>
{
    void IEntityTypeConfiguration<PackageEntity>.Configure(EntityTypeBuilder<PackageEntity> builder)
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

