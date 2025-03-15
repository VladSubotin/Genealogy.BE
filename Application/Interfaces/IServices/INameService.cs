using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface INameService
    {
        void Create(NameToCreateDTO nameToCreate);
        void Update(NameToUpdateDTO nameToUpdate);
        void Delete(Guid id);
    }
}
