﻿using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IBiographicalFactService
    {
        void Create(FactToCreateDTO factToCreate);
        void Delete(Guid id);
    }
}
