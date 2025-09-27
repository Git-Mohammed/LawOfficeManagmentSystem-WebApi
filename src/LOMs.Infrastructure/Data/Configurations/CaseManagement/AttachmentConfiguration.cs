using LOMs.Domain.Cases.Attachments;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagement
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            // Primary key
            builder.HasKey(a => a.Id).IsClustered(false);

            // Properties
            builder.Property(a => a.AttachmentName)
                .IsRequired()
                .HasMaxLength(200);

            // Relationships
            builder.HasOne(a => a.Case)
                .WithMany(c => c.Attachments)
                .HasForeignKey(a => a.CaseId)
                .OnDelete(DeleteBehavior.Cascade);
            // Table name and schema

            builder.ToTable("Attachments", SchemaNames.GetName(DatabaseSchema.Case));
        }
    }
}
