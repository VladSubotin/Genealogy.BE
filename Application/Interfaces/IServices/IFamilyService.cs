using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IFamilyService
    {
        Guid Create(FamilyToCreateDTO familyToCreate);
        void Update(FamilyToUpdateDTO familyToUpdate);
        void Delete(Guid id);
        IEnumerable<FamilyDTO> getAll(string? name, string? login);
        FamilyProfileDTO get(Guid familyId, string userLogin);
    }
}
