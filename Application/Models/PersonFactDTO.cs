using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PersonFactDTO
    {
        public Guid Id { get; set; }

        public string? FactType { get; set; }

        public IEnumerable<PersonFactVersionDTO>? Versions { get; set; }
    }
}
