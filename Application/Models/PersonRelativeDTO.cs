﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PersonRelativeDTO
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }

        public string? RelationType { get; set; }
    }
}
