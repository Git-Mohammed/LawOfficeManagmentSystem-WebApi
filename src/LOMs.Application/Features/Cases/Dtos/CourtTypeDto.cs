using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Cases.Dtos
{
    public class CourtTypeDto
    {
        public Guid CourtTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public short Code { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
