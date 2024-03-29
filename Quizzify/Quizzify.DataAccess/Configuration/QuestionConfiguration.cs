﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizzify.DataAccess.Entities;

namespace Quizzify.DataAccess.Configuration;

public class QuestionConfiguration : IEntityTypeConfiguration<QuestionEntity>
{
    void IEntityTypeConfiguration<QuestionEntity>.Configure(EntityTypeBuilder<QuestionEntity> builder)
    {
        builder.HasKey(e => e.QuestionId).HasName("questions_pkey");

        builder.ToTable("Questions");

        builder.Property(e => e.QuestionId).HasColumnName("question_id");
        builder.Property(e => e.AnswerImageUrl).HasColumnName("answer_image_url");
        builder.Property(e => e.AnswerText).HasColumnName("answer_text");
        builder.Property(e => e.QuestionComment).HasColumnName("question_comment");
        builder.Property(e => e.QuestionCost).HasColumnName("question_cost");
        builder.Property(e => e.QuestionImageUrl).HasColumnName("question_image_url");
        builder.Property(e => e.QuestionText).HasColumnName("question_text");
        builder.Property(e => e.QuestionTheme)
            .HasMaxLength(15)
            .HasColumnName("question_theme");
        builder.Property(e => e.RoundId).HasColumnName("round_id");

        builder.HasOne(d => d.Round).WithMany(p => p.Questions)
            .HasForeignKey(d => d.RoundId)
            .HasConstraintName("questions_round_id_fk");
    }
}