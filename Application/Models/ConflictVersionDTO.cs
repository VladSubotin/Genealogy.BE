using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ConflictVersionDTO
    {
        public Guid VersionId { get; set; }

        public Guid ConflictVersionId { get; set; }

        public PersonRelativeDTO? ConflictRelative { get; set; }
    }
}
