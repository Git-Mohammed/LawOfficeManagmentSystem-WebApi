using LOMs.Domain.Common.Constants;
using LOMs.Domain.Identity;
namespace LOMs.Infrastructure.Data.Seeds
{
    public static class SeedData
    {
        public static class CourtTypes
        {
            public static readonly Guid PublicId = Guid.Parse("b3a1f8b4-1e6a-4b3e-9c3d-0a9f8d8a1b11");
            public static readonly Guid CriminalId = Guid.Parse("c4f2a9d7-2b7e-4a4f-8b2e-1c2d3e4f5a66");
            public static readonly Guid LabourId = Guid.Parse("d5e3b0c8-3c8f-4c5f-9d3f-2d3e4f5a6b77");
            public static readonly Guid PersonalStatusId = Guid.Parse("e6f4c1d9-4d9f-4d6f-ae4f-3e4f5a6b7c88");
            public static readonly Guid CommercialId = Guid.Parse("f7a5d2ea-5eaf-4e7f-bf5f-4f5a6b7c8d99");
            public static readonly Guid AdministrativeId = Guid.Parse("a8b6e3fb-6fb0-4f8f-cf6f-5a6b7c8d9e10");
            public static readonly Guid SemiJudicialId = Guid.Parse("1816e3fb-6fb0-428f-cf6f-5a7b7c829e10");
            public static readonly Guid OtherId = Guid.Parse("b9c7f40c-70c1-4f9f-df7f-6b7c8d9e0f21");
            public static readonly string defulatUser = LOMsConstants.SystemUser;
            // Static timestamp baked into the migration
            private static readonly DateTimeOffset SeedTime = new DateTimeOffset(2025, 10, 02, 0, 0, 0, TimeSpan.Zero);

            public static readonly object[] Data = new[]
            {
                new {
                    Id              = PublicId,
                    Name            = "العامة",
                    Code            = (short)100,
                    Description     = "المحكمة العامة",
                    CreatedAtUtc    = SeedTime,
                    CreatedBy       = defulatUser,
                    LastModifiedUtc = SeedTime,
                    LastModifiedBy  = defulatUser
                },
                new {
                    Id              = CriminalId,
                    Name            = "الجنائية",
                    Code            = (short)200,
                    Description     = "المحكمة الجنائية",
                    CreatedAtUtc    = SeedTime,
                    CreatedBy       = defulatUser,
                    LastModifiedUtc = SeedTime,
                    LastModifiedBy  = defulatUser
                },
                new {
                    Id              = LabourId,
                    Name            = "العمالية",
                    Code            = (short)300,
                    Description     = "المحكمة العمالية",
                    CreatedAtUtc    = SeedTime,
                    CreatedBy       = defulatUser,
                    LastModifiedUtc = SeedTime,
                    LastModifiedBy  = defulatUser
                },
                new {
                    Id              = PersonalStatusId,
                    Name            = "الأحوال الشخصية",
                    Code            = (short)400,
                    Description     = "محكمة الأحوال الشخصية",
                    CreatedAtUtc    = SeedTime,
                    CreatedBy       = defulatUser,
                    LastModifiedUtc = SeedTime,
                    LastModifiedBy  = defulatUser
                },
                new {
                    Id              = CommercialId,
                    Name            = "التجارية",
                    Code            = (short)500,
                    Description     = "المحكمة التجارية",
                    CreatedAtUtc    = SeedTime,
                    CreatedBy       = defulatUser,
                    LastModifiedUtc = SeedTime,
                    LastModifiedBy  = defulatUser
                },
                new {
                    Id              = AdministrativeId,
                    Name            = "الإدارية",
                    Code            = (short)600,
                    Description     = "المحكمة الإدارية",
                    CreatedAtUtc    = SeedTime,
                    CreatedBy       = defulatUser,
                    LastModifiedUtc = SeedTime,
                    LastModifiedBy  = defulatUser
                },
                new {
                    Id              = SemiJudicialId,
                    Name            = "اللجان شبه القضائية",
                    Code            = (short)700,
                    Description     = "اللجان شبه القضائية",
                    CreatedAtUtc    = SeedTime,
                    CreatedBy       = defulatUser,
                    LastModifiedUtc = SeedTime,
                    LastModifiedBy  = defulatUser
                },
                new {
                    Id              = OtherId,
                    Name            = "أخرى",
                    Code            = (short)32000,
                    Description     = "أخرى",
                    CreatedAtUtc    = SeedTime,
                    CreatedBy       = defulatUser,
                    LastModifiedUtc = SeedTime,
                    LastModifiedBy  = defulatUser
                }
            };
        }
        public static class Roles
        {
            public static readonly string[] Names =
            {
                Role.GeneralManager.ToString(),
                Role.ExecutiveManager.ToString(),
                Role.Consultant.ToString(),
                Role.Lawyer.ToString()
            };
        }

        public static class DefaultUser
        {
            public const string NationalId = "0000000000";
            public const string Email      = "arammtech@gmail.com";
            public const string Password   = "Admin123@";
            public const Role TheRole       = Role.GeneralManager;
        }
    }
}
