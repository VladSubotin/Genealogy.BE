using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PersonImageDTO
    {
        public Guid Id { get; set; }

        public byte[]? Image { get; set; }
    }
}
