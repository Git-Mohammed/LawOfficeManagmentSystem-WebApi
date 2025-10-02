using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;
using System.Text.RegularExpressions;

namespace LOMs.Domain.People.Phones
{
    public sealed class Phone : AuditableEntity
    {
        public string Code { get; private set; } = null!;
        public string Number { get; private set; } = null!;
        public Guid PersonId { get;  }

        public Person? Person { get;  set; }
        private Phone() { }

        private Phone(Guid id, string number,string code) : base(id)
        {
            Number = number;
            Code = code;
        }

        public static Result<Phone> Create(Guid id, string number, string code)
        {
            if (string.IsNullOrWhiteSpace(number))
                return PhoneErrors.NumberRequired;

            if (!Regex.IsMatch(number, @"^\+?\d{7,15}$"))
                return PhoneErrors.NumberInvalid;

            if (string.IsNullOrWhiteSpace(code))
                return PhoneErrors.CodeRequired;

            if ((code.Length < 1 || code.Length > 4) || code.StartsWith("+") || code.StartsWith("00"))
                return PhoneErrors.CodeInvalid;

            if (number.Length < 7)
                return PhoneErrors.NumberTooShort;

            if (number.Length > 15)
                return PhoneErrors.NumberTooLong;


            return new Phone(id, number, code);
        }
        
        public Result<Updated> Update(string number, string code)
        {
            if (string.IsNullOrWhiteSpace(number) || !Regex.IsMatch(number, @"^\+?\d{7,15}$"))
                return PersonErrors.InvalidPhoneNumber;

            if (string.IsNullOrWhiteSpace(code))
                return Error.Validation("Phone.Code", "Code cannot be empty.");

            Number = number;
            Code = code;
            
            return Result.Updated;
        }

    }
}
