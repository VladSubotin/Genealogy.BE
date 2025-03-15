using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserToUpdatePasswordDTO
    {

        public string? Login { get; set; } = null!; 
        public string? HashPasswordOld { get; set; }
        public string? HashPasswordNew { get; set; }
    }
}
