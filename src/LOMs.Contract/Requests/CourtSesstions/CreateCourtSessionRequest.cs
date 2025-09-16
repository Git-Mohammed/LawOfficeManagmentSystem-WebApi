using System;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.CourtSessions
{
    public class CreateCourtSessionRequest
    {
        [Required(ErrorMessage = "Assigned employee ID is required.")]
        public Guid AssignedEmployeeId { get; set; }

        [Required(ErrorMessage = "Scheduled date is required.")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset ScheduledAt { get; set; }

        [Required(ErrorMessage = "Assigned tasks are required.")]
        public string AssignedTasks { get; set; } = string.Empty;
    }
}
