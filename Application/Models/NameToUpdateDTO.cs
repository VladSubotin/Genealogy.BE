﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class NameToUpdateDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public bool? IsMain { get; set; }
    }
}
