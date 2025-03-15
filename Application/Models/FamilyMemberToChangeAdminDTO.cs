using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class FamilyMemberToChangeAdminDTO
    {
        public Guid CurrentAdminId { get; set; }
        public Guid NewAdminId { get; set; }
        public string CurrentAdminHashPassword { get; set;}
    }
}
