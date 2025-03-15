using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IPersonService
    {
        Guid Create(PersonToCreateDTO personToCreate);
        void Update(PersonProfileDTO personToUpdate);
        void Delete(Guid id);
        PersonProfileDTO get(Guid id, string userLogin);
        IEnumerable<PersonMainInfoDTO> getAll(PersonOptionsDTO? personOptions);
        //IEnumerable<ConflictVersionDTO> verifyFactDates(Guid id);
        HierarchicTreeNodeDTO getHierarchicTreeNode(Guid id, sbyte direction);
    }
}
