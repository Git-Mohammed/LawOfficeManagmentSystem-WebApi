using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases.CourtTypes
{
    public class CourtType : AuditableEntity
    {
        public string Name { get; private set; } = null!;
        public short Code { get; private set; }
        public string? Description { get; private set; }

        private readonly List<Case> _cases = [];
        public IEnumerable<Case> Cases => _cases.AsReadOnly();

        private CourtType() { }

        private CourtType(Guid id, string name, short code, string? description) : base(id)
        {
            Name = name;
            Code = code;
            Description = description;
        }

        /// <summary>
        /// Factory method to create a CourtType with validation.
        /// </summary>
        public static Result<CourtType> Create(Guid id, string name, short code, string? description)
        {
            if (id == Guid.Empty)
                return CourtTypeErrors.InvalidId;

            if (string.IsNullOrWhiteSpace(name))
                return CourtTypeErrors.MissingName;

            if (code <= 0)
                return CourtTypeErrors.InvalidCode;

            var courtType = new CourtType(id, name.Trim(), code, description?.Trim());
            courtType.CreatedAtUtc = DateTimeOffset.UtcNow;
            courtType.CreatedBy = "System";

            return courtType;
        }
    }
}
