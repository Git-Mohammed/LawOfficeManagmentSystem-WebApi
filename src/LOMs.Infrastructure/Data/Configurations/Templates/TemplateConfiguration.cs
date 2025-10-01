using LOMs.Domain.Templates;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Infrastructure.Data.Configurations.Templates
{
    public class TemplateConfiguration: IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
            // Primary key
            builder.HasKey(a => a.Id).IsClustered(false);

            // Properties
            builder.Property(a => a.TemplateName)
                .IsRequired()
                .HasMaxLength(200);

           
            // Table name and schema

            builder.ToTable("Templates", "Templates");
        }
    }
}
