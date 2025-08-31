using LOMs.Domain.Cases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagment;

public class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> builder)
    {
        builder.HasKey(c => c.Id).IsClustered(false);

        builder.Property(c => c.CaseNumber)
            .HasMaxLength(100);

        builder.Property(c => c.CaseNotes)
            .HasMaxLength(1000);

        builder.Property(c => c.ClientRequests)
            .HasMaxLength(1000);

        builder.Property(c => c.AssignedOfficer)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("اسم الموظف المسؤول عن متابعة القضية");

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<int>() // يخزن كـ int
            .HasComment("الحالة الحالية للقضية: 0 = مسودة، 1 = قيد الانتظار، 2 = قيد المعالجة، 3 = منتهية، 4 = ملغية");

        builder.Property(c => c.Role)
            .IsRequired()
            .HasConversion<int>() // يخزن كـ int
            .HasComment("دور العميل في القضية: 1 = مدعي، 2 = مدعى عليه");

        builder.Property(c => c.CourtType)
            .IsRequired()
            .HasConversion<int>() // يخزن كـ int
            .HasComment("نوع المحكمة: 100 = عامة، 200 = جزئية، 300 = عمالية، 400 = أحوال شخصية، 600 = إدارية، 700 = لجان شبه قضائية، 800 = أخرى");


        builder.HasOne(c => c.ClientFile)
            .WithMany(cf => cf.Cases)
            .HasForeignKey(c => c.ClientFileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
