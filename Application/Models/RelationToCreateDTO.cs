using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class RelationToCreateDTO
    {
        public Guid? PersonIsId { get; set; }

        public string? RelationType { get; set; }

        public Guid? ToPersonId { get; set; }
    }
}
