using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizzify.DataAccess.Entities;

namespace Quizzify.DataAccess.Configuration;

public class SecretQuestionConfiguration : IEntityTypeConfiguration<SecretQuestionEntity>
{
    void IEntityTypeConfiguration<SecretQuestionEntity>.Configure(EntityTypeBuilder<SecretQuestionEntity> builder)
    {
        builder.HasKey(e => e.SecretQId).HasName("secretquestions_pkey");

        builder.ToTable("SecretQuestions");

        builder.Property(e => e.SecretQId).HasColumnName("secret_q_id");
        builder.Property(e => e.QuestionText)
            .IsRequired()
            .HasColumnName("question_text");
    }
}