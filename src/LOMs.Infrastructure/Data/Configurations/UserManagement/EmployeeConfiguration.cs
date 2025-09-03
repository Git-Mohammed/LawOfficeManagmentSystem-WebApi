using LOMs.Domain.People.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.UserManagement;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id).IsClustered(false);
        builder.Property(e => e.Role)
            .HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
        
        builder.HasOne(e=>e.Person)
            .WithOne(p=> p.Employee)
            .HasForeignKey<Employee>(e=>e.PersonId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(e => e.PersonId);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Email);
        builder.ToTable("Employees","UserManagement");
    }
}