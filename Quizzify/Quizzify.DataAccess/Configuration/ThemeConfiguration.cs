using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizzify.DataAccess.Entities;

namespace Quizzify.DataAccess.Configuration;

public class ThemeConfiguration : IEntityTypeConfiguration<ThemeEntity>
{
    void IEntityTypeConfiguration<ThemeEntity>.Configure(EntityTypeBuilder<ThemeEntity> builder)
    {
        builder.HasKey(e => e.ThemeId).HasName("themes_pkey");

        builder.ToTable("Themes");

        builder.Property(e => e.ThemeId).HasColumnName("theme_id");
        builder.Property(e => e.ThemeName).IsRequired()
            .HasColumnName("theme_name");
        builder.Property(e => e.RoundId).HasColumnName("round_id");
        

        builder.HasOne(d => d.Round).WithMany(p => p.Themes)
            .HasForeignKey(d => d.RoundId)
            .HasConstraintName("themes_round_id_fk");
    }
}