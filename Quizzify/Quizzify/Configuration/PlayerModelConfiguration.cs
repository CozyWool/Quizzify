using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizzify.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerModelConfiguration : IEntityTypeConfiguration<Player>
{
    void IEntityTypeConfiguration<Player>.Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(e => e.PlayerId).HasName("players_pkey");

        builder.ToTable("players");

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
            .HasForeignKey<Player>(d => d.UserId)
            .HasConstraintName("user_id_fk");
    }
}
