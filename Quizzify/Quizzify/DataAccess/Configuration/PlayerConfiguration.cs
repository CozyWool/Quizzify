using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizzify.DataAccess.Entities;

namespace Quizzify.DataAccess.Configuration;

public class PlayerConfiguration : IEntityTypeConfiguration<PlayerEntity>
{
    void IEntityTypeConfiguration<PlayerEntity>.Configure(EntityTypeBuilder<PlayerEntity> builder)
    {
        builder.HasKey(e => e.PlayerId).HasName("players_pkey");

        builder.ToTable("Players");

        builder.HasIndex(e => e.UserId, "players_user_id_key").IsUnique();

        builder.Property(e => e.PlayerId).HasColumnName("player_id");
        builder.Property(e => e.About).HasColumnName("about");
        builder.Property(e => e.Nickname)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("nickname");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.UserProfilePicture).HasColumnName("user_profile_picture");

        builder.HasOne(d => d.User).WithOne(p => p.Player)
            .HasForeignKey<PlayerEntity>(d => d.UserId)
            .HasConstraintName("user_id_fk");
    }
}