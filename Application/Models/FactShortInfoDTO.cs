using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class FactShortInfoDTO
    {
        public string? Place { get; set; }

        public DateOnly? DateFrom { get; set; }

        public DateOnly? DateTo { get; set; }
    }
}
