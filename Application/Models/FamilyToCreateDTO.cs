using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class FamilyToCreateDTO
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public byte[]? ProfileIcon { get; set; }

        public int? PrivacyLevel { get; set; }
        public string? AdminLogin { get; set; }
    }
}
