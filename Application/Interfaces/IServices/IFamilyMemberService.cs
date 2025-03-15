using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IFamilyMemberService
    {
        void Create(FamilyMemberToAddDTO familyMemberToAdd);
        void Update(FamilyMemberToUpdateDTO familyMemberToUpdate);
        void Delete(Guid id);
        IEnumerable<UserFamiliesDTO> getUserFamilies(string userLogin);
        IEnumerable<FamilyUsersDTO> getFamilyMembers(Guid familyId);
        void changeFamilyAdmin(FamilyMemberToChangeAdminDTO familyMemberToChangeAdmin);
    }
}
