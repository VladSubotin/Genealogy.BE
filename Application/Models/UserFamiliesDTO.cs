using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserFamiliesDTO
    {
        public Guid Id { get; set; }
        public Guid FamilyId { get; set; }
        public string? FamilyName { get; set; }
        public string? Role { get; set; }
        public byte[]? FamilyProfileIcon { get; set; }
    }
}
