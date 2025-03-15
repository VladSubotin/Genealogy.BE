using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class StepToUpdateDTO
    {
        public Guid Id { get; set; }

        public string? Purpose { get; set; }

        public string? Source { get; set; }

        public string? SourceLocation { get; set; }

        public DateOnly? Term { get; set; }

        public string? Result { get; set; }

        public bool? IsDone { get; set; }
    }
}
