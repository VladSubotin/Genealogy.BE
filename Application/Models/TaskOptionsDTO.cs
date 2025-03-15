using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class TaskOptionsDTO
    {
        public string? UserLogin { get; set; }
        public Guid? FamilyId { get; set; }
        public bool? IsDone { get; set; }
        public string? TaskName { get; set; }
    }
}
