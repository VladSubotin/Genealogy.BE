using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IVersionService
    {
        void Create(VersionToCreateDTO versionToCreate);
        void Update(VersionToUpdateDTO versionToUpdate);
        void Delete(Guid id);
    }
}
