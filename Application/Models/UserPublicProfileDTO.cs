using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserPublicProfileDTO
    {
        public string Login { get; set; } = null!;

        public string? Name { get; set; }

        public byte[]? ProfileIcon { get; set; }

        public int? AgeInTens { get; set; }
        public string? Description { get; set; }

        public IEnumerable<UserFamiliesDTO> Families { get; set; } = new List<UserFamiliesDTO>();
    }
}
