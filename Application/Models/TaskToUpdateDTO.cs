using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class TaskToUpdateDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? IsDone { get; set; }

        public string? UserLogin { get; set; }
    }
}
