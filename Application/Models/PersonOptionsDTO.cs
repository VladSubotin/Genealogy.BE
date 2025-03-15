using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PersonOptionsDTO
    {
        public Guid? FamilyId { get; set; }

        public string? Prefix { get; set; }
        public string? FirstName { get; set; }
        public string? MidleName { get; set; }
        public string? LastName { get; set; }
        public string? Suffix { get; set; }

        public string? Gender { get; set; }

        public IEnumerable<PersonFactOpitonsDTO>? Facts { get; set; }

        public IEnumerable<PersonRelativeOptionsDTO>? Relatives { get; set; }
    }
}
