using LOMs.Domain.Cases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagment;

public class ClientCaseConfiguration : IEntityTypeConfiguration<ClientCase>
{
    public void Configure(EntityTypeBuilder<ClientCase> builder)
    {
        builder.HasKey(cc => new { cc.CaseId, cc.ClientId, cc.ClientFileId });

        builder.HasOne(cc => cc.Case)
            .WithMany(c => c.CaseClients)
            .HasForeignKey(cc => cc.CaseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cc => cc.Client)
            .WithMany(c => c.CaseClients)
            .HasForeignKey(cc => cc.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cc => cc.ClientFile)
            .WithMany(c => c.CaseClients)
            .HasForeignKey(cc => cc.ClientFileId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
