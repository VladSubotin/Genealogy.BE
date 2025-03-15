using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ConflictVerDTO
    {
        public string? Message { get; set; }

        public Guid ConflictVersionId { get; set; }

        public PersonRelativeDTO? ConflictRelative { get; set; }
    }
}
