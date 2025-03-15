using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FamilyMemberService : IFamilyMemberService
    {
        private readonly IRepository<User, string> userRepository;
        private readonly IRepository<FamilyMember, Guid> familyMemberRepository;
        private readonly IRepository<Family, Guid> familyRepository;

        public FamilyMemberService(IRepository<User, string> userRepository, IRepository<FamilyMember, Guid> familyMemberRepository, IRepository<Family, Guid> familyRepository)
        {
            this.userRepository = userRepository;
            this.familyMemberRepository = familyMemberRepository;
            this.familyRepository = familyRepository;

        }

        public void changeFamilyAdmin(FamilyMemberToChangeAdminDTO familyMemberToChangeAdmin)
        {   
            var familyCurrentAdmin = familyMemberRepository.GetById(familyMemberToChangeAdmin.CurrentAdminId);
            if (familyCurrentAdmin == null)
            {
                throw new Exception($"Family member's id {familyMemberToChangeAdmin.CurrentAdminId} doesn't exist");
            }
            if (familyCurrentAdmin.Role != "Admin")
            {
                throw new Exception($"User {familyCurrentAdmin.UserLogin} isn't an admin of the family with id {familyCurrentAdmin.FamilyId}");
            }

            var familyNewAdmin = familyMemberRepository.GetById(familyMemberToChangeAdmin.NewAdminId);
            if (familyNewAdmin == null)
            {
                throw new Exception($"Family member's id {familyMemberToChangeAdmin.NewAdminId} doesn't exist");
            }

            if (familyCurrentAdmin.FamilyId != familyNewAdmin.FamilyId)
            {
                throw new Exception($"New admin have to be a member from current admin's family");
            }

            var currentAdminHashPassword = userRepository.GetById(familyCurrentAdmin.UserLogin).HashPassword;
            if (currentAdminHashPassword != familyMemberToChangeAdmin.CurrentAdminHashPassword)
            {
                throw new Exception($"Password isn't correct");
            }

            familyCurrentAdmin.Role = "Moderator";
            familyNewAdmin.Role = "Admin";
            familyMemberRepository.Update(familyCurrentAdmin);
            familyMemberRepository.Update(familyNewAdmin);
        }

        public void Create(FamilyMemberToAddDTO familyMemberToAdd)
        {
            var familyMember = familyMemberRepository.GetByField(m => m.UserLogin == familyMemberToAdd.UserLogin 
                && m.FamilyId == familyMemberToAdd.FamilyId).FirstOrDefault();
            if (familyMember != null)
            {
                throw new Exception($"user {familyMemberToAdd.UserLogin} already is a member of the family with id {familyMemberToAdd.FamilyId}");
            }

            familyMemberRepository.Add(new FamilyMember
            {
                Id = Guid.NewGuid(),
                FamilyId = familyMemberToAdd.FamilyId,
                UserLogin = familyMemberToAdd.UserLogin,
                Role = familyMemberToAdd.Role
            });
        }

        public void Delete(Guid id)
        {
            var familyMember = familyMemberRepository.GetById(id);
            if (familyMember?.Role == "Admin")
            {
                throw new Exception($"Family admin can't leave the family");
            }

            familyMemberRepository.Delete(id);
        }

        public IEnumerable<FamilyUsersDTO> getFamilyMembers(Guid familyId)
        {
            var familiyUsers = new List<FamilyUsersDTO>();
            var familyMembers = familyMemberRepository.GetByField(m => m.FamilyId == familyId);
            foreach (var familyMember in familyMembers)
            {
                var user = userRepository.GetById(familyMember.UserLogin);
                familiyUsers.Add(new FamilyUsersDTO
                {
                    Id = familyMember.Id,
                    UserLogin = user.Login,
                    UserName = user.Name,
                    UserRole = familyMember.Role
                });
            }
            return familiyUsers;
        }

        public IEnumerable<UserFamiliesDTO> getUserFamilies(string userLogin)
        {
            var userFamilies = new List<UserFamiliesDTO>();
            var familyMembers = familyMemberRepository.GetByField(m => m.UserLogin == userLogin);
            foreach (var familyMember in familyMembers)
            {
                var family = familyRepository.GetById((Guid)familyMember.FamilyId);
                userFamilies.Add(new UserFamiliesDTO
                {
                    Id = familyMember.Id,
                    FamilyId = family.Id,
                    FamilyName = family.Name,
                    Role = familyMember.Role,
                    FamilyProfileIcon = family.ProfileIcon
                });
            }
            return userFamilies;
        }

        public void Update(FamilyMemberToUpdateDTO familyMemberToUpdate)
        {
            var familyMember = familyMemberRepository.GetById(familyMemberToUpdate.Id);
            if (familyMember == null)
            {
                throw new Exception($"Family member's id {familyMemberToUpdate.Id} doesn't exist");
            }
            if (familyMember?.Role == "Admin")
            {
                throw new Exception($"It's forbidden to change admin's role");
            }

            familyMember.Role = familyMemberToUpdate.Role;
            familyMemberRepository.Update(familyMember);
        }
    }
}
