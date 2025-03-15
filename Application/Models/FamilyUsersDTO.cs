using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class FamilyUsersDTO
    {
        public Guid? Id { get; set; }
        public string? UserLogin { get; set; }
        public string? UserName { get; set; }
        public string? UserRole { get; set; }
    }
}
