using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PersonToUpdateDTO
    {
        public Guid Id { get; set; }

        public string? Prefix { get; set; }

        public string? Suffix { get; set; }

        public string? Gender { get; set; }

        public string? Nationality { get; set; }

        public string? Religion { get; set; }

        public string? Biography { get; set; }
    }
}
