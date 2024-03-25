using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizzify.DataAssecc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class RoundConfiguration : IEntityTypeConfiguration<Round>
{
    void IEntityTypeConfiguration<Round>.Configure(EntityTypeBuilder<Round> builder)
    {
        builder.HasKey(e => e.RoundId).HasName("rounds_pkey");

        builder.ToTable("rounds");

        builder.Property(e => e.RoundId).HasColumnName("round_id");
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("name");
        builder.Property(e => e.PackageId).HasColumnName("package_id");
        builder.Property(e => e.RoundType)
            .IsRequired()
            .HasMaxLength(7)
            .HasColumnName("round_type");

        builder.HasOne(d => d.Package).WithMany(p => p.Rounds)
            .HasForeignKey(d => d.PackageId)
            .HasConstraintName("rounds_package_id_fk");
    }
}
