using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class StepToAddDTO
    {
        public Guid? TaskId { get; set; }

        public string? Purpose { get; set; }

        public string? Source { get; set; }

        public string? SourceLocation { get; set; }

        public DateOnly? Term { get; set; }

        public string? Result { get; set; }
    }
}
