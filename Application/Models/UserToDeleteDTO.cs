using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserToDeleteDTO
    {
        public string Login { get; set; } = null!;
        public string? HashPassword { get; set; }
    }
}
