using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizzify.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizzify.Configuration
{
    internal class SecretquestionModelConfiguration : IEntityTypeConfiguration<Secretquestion>
    {
        void IEntityTypeConfiguration<Secretquestion>.Configure(EntityTypeBuilder<Secretquestion> builder)
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
