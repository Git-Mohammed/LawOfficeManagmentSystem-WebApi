namespace LOMs.Domain.Cases.Enums
{
    public enum CourtType 
    {
        /// <summary>المحكمة العامة</summary>
        GeneralCourt = 100,

        /// <summary>المحكمة الجزئية</summary>
        PartialCourt = 200,

        /// <summary>المحكمة العمالية</summary>
        LaborCourt = 300,

        /// <summary>محكمة الأحوال الشخصية</summary>
        PersonalStatusCourt = 400,

        /// <summary>المحكمة الإدارية</summary>
        AdministrativeCourt = 600,

        /// <summary>محكمة اللجان شبه القضائية</summary>
        QuasiJudicialCommitteeCourt = 700,

        /// <summary>أخرى</summary>
        Other = 800
    }
}
