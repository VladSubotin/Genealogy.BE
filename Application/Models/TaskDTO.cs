using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class TaskDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateOnly? CreationDate { get; set; }

        public bool? IsDone { get; set; }

        public string? UserLogin { get; set; }
        public string? UserName { get; set; }

        public Guid? FamilyId { get; set; }
        public string? FamilyName { get; set; }
        public IEnumerable<StepDTO> Steps { get; set; }
    }
}
