using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PersonMainInfoDTO
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }

        public Guid? FamilyId { get; set; }

        public string? FamilyName { get; set; }

        public byte[]? Image { get; set; }

        public FactShortInfoDTO? Birth { get; set; }

        public FactShortInfoDTO? Deth { get; set; }

        public IEnumerable<PersonRelativeDTO>? Relatives { get; set; }
    }
}
