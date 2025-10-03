using LOMs.Domain.People.Employees;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.UserManagement;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id)
               .IsClustered(false);
        builder.Property(e => e.Id)
               .ValueGeneratedNever();

        builder.Property(e => e.Role)
               .HasConversion<string>()
               .HasMaxLength(50)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(e => e.UserId)
               .IsRequired();

        builder.Property(e => e.Email)
               .HasMaxLength(100)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(e => e.IsActive)
               .IsRequired();

        builder.Property(e => e.PersonId)
               .IsRequired();
        builder.HasIndex(e => e.PersonId)
               .IsUnique()
               .HasDatabaseName("UX_Employees_PersonId");

        builder.HasIndex(e => e.UserId)
               .IsUnique()
               .HasDatabaseName("UX_Employees_UserId");

        builder.HasIndex(e => e.Email)
               .IsUnique()
               .HasDatabaseName("UX_Employees_Email");

        builder.HasOne(e => e.Person)
               .WithOne(p => p.Employee)
               .HasForeignKey<Employee>(e => e.PersonId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Employees", SchemaNames.GetName(DatabaseSchema.User));
    }
}
