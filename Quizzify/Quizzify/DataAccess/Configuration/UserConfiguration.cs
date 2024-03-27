using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizzify.DataAccess.Entities;

namespace Quizzify.DataAccess.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    void IEntityTypeConfiguration<UserEntity>.Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(e => e.UserId).HasName("users_pkey");

        builder.ToTable("Users");

        builder.HasIndex(e => e.Email, "users_email_key").IsUnique();

        builder.HasIndex(e => e.Login, "users_login_key").IsUnique();

        builder.Property(e => e.UserId)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("user_id");
        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("email");
        builder.Property(e => e.GoogleAuthorization).HasColumnName("google_authorization");
        builder.Property(e => e.Login)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("login");
        builder.Property(e => e.PasswordHash)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("password_hash");
        builder.Property(e => e.SecretAnswerHash)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("secret_answer_hash");
        builder.Property(e => e.SelectedSecretQuestionId).HasColumnName("selected_secret_question_id");
        builder.Property(e => e.TwofaAuthMethod).HasColumnName("twofa_auth_method");

        builder.HasOne(d => d.SelectedSecretQuestion).WithMany(p => p.Users)
            .HasForeignKey(d => d.SelectedSecretQuestionId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("secret_question_fk");
    }
}