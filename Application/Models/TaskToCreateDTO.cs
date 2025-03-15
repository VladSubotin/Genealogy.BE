using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class TaskToCreateDTO
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? UserLogin { get; set; }

        public Guid? FamilyId { get; set; }
    }
}
