using LOMs.Domain.Identity;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using LOMs.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.UserManagement;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens", SchemaNames.GetName(DatabaseSchema.User));

        builder.HasKey(rt => rt.Id)
               .IsClustered(false);
        builder.Property(rt => rt.Id)
               .ValueGeneratedNever();

        builder.Property(rt => rt.Token)
               .IsRequired()
               .HasMaxLength(255);
        builder.HasIndex(rt => rt.Token)
               .IsUnique()
               .HasDatabaseName("UX_RefreshTokens_Token");

        builder.Property(rt => rt.UserId)
               .IsRequired();
        builder.HasIndex(rt => rt.UserId)
               .HasDatabaseName("IX_RefreshTokens_UserId");

        builder.Property(rt => rt.ExpiresOnUtc)
               .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
