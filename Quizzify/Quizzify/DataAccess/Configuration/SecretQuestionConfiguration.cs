using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizzify.DataAssecc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizzify.DataAssecc.Configuration
{
    internal class SecretQuestionConfiguration : IEntityTypeConfiguration<SecretQuestion>
    {
        void IEntityTypeConfiguration<SecretQuestion>.Configure(EntityTypeBuilder<SecretQuestion> builder)
        {
            builder.HasKey(e => e.SecretQId).HasName("secretquestions_pkey");

            builder.ToTable("secretquestions");

            builder.Property(e => e.SecretQId).HasColumnName("secret_q_id");
            builder.Property(e => e.QuestionText)
                .IsRequired()
                .HasColumnName("question_text");
        }
    }
}
