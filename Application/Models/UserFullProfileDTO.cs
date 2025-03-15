using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserFullProfileDTO
    {
        public string Login { get; set; } = null!;

        public string? Name { get; set; }

        public string? Email { get; set; }

        public byte[]? ProfileIcon { get; set; }

        public DateOnly? BirthDate { get; set; }
        public string? Description { get; set; }
    }
}
