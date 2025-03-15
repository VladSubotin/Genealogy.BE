using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class FamilyMemberToAddDTO
    {
        public string UserLogin { get; set; }
        public Guid FamilyId { get; set; }
        public string Role { get; set;}
    }
}
