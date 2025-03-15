using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class NameToCreateDTO
    {
        public Guid Id { get; set; }
        public Guid? PersonId { get; set; }

        public string? NameType { get; set; }

        public string? Name { get; set; }

        public bool? IsMain { get; set; }
    }
}
