using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Auth.Dtos
{
    public class LoginDto
    {
        public string Email { get; init; } = null!;
        public  string Password { get; init; } = null!;
    }

}
