using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PersonProfileDTO
    {
        public Guid Id { get; set; }

        public string? MyRole { get; set; }

        public string? Prefix { get; set; }

        public string? Suffix { get; set; }

        public string? FullName { get; set; }

        public IEnumerable<PersonNameDTO>? Names { get; set; }

        public string? Gender { get; set; }

        public string? Nationality { get; set; }

        public string? Religion { get; set; }

        public string? Biography { get; set; }

        public Guid? FamilyId { get; set; }

        public string? FamilyName { get; set; }

        public IEnumerable<PersonFactDTO>? Facts { get; set; }

        public IEnumerable<PersonImageDTO>? Images { get; set; }

        public IEnumerable<PersonRelativeDTO>? Relatives { get; set; }
    }
}
