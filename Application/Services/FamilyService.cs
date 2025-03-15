using Application.Extentions;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FamilyService : IFamilyService
    {
        private readonly IRepository<User, string> userRepository;
        private readonly IRepository<FamilyMember, Guid> familyMemberRepository;
        private readonly IRepository<Family, Guid> familyRepository;

        public FamilyService(IRepository<User, string> userRepository, IRepository<FamilyMember, Guid> familyMemberRepository, IRepository<Family, Guid> familyRepository)
        {
            this.userRepository = userRepository;
            this.familyMemberRepository = familyMemberRepository;
            this.familyRepository = familyRepository;
        }

        public Guid Create(FamilyToCreateDTO familyToCreate)
        {
            var admin = userRepository.GetById(familyToCreate.AdminLogin);
            if (admin == null)
            {
                throw new Exception($"User {familyToCreate.AdminLogin} doesn't exist");
            }

            Guid id = Guid.NewGuid();
            familyRepository.Add(new Family
            {
                Id = id,
                Name = familyToCreate.Name,
                Description = familyToCreate.Description,
                PrivacyLevel = familyToCreate.PrivacyLevel,
                ProfileIcon = familyToCreate.ProfileIcon,
            });
            familyMemberRepository.Add(new FamilyMember
            {
                Id= Guid.NewGuid(),
                FamilyId = id,
                UserLogin = familyToCreate.AdminLogin,
                Role = "Admin"
            });

            return id;
        }

        public void Delete(Guid id)
        {
            familyRepository.Delete(id);
        }

        public FamilyProfileDTO get(Guid familyId, string userLogin)
        {
            var family = familyRepository.GetById(familyId);
            if (family == null)
            {
                throw new Exception($"Family with id {familyId} doesn't exist");
            }

            var adminMember = familyMemberRepository.GetByField(m => m.FamilyId == familyId && m.Role == "Admin").FirstOrDefault();
            var adminUser = userRepository.GetById(adminMember?.UserLogin);

            var self = familyMemberRepository.GetByField(m => m.FamilyId == familyId && m.UserLogin == userLogin).FirstOrDefault();

            return new FamilyProfileDTO
            {
                Id = family.Id,
                Name = family.Name,
                Description = family.Description,
                PrivacyLevel = family.PrivacyLevel,
                ProfileIcon = family.ProfileIcon,
                Admin = new FamilyUsersDTO
                {
                    Id = adminMember?.Id,
                    UserLogin = adminMember?.UserLogin,
                    UserName = adminUser?.Name,
                    UserRole = adminMember?.Role
                }, 
                MyRole = self?.Role ?? "Guest"
            };
        }

        public IEnumerable<FamilyDTO> getAll(string? name, string? login)
        {
            Expression<Func<Family, bool>> predicate = f => true;
            if (!string.IsNullOrEmpty(name))
            {
                Expression<Func<Family, bool>> namePredicate = f => f.Name
                    .Trim()
                    .IndexOf(name.Trim()) == 0;
                predicate = predicate.MergeAnd(namePredicate);
            }
            if (!string.IsNullOrEmpty(login))
            {
                Expression<Func<Family, bool>> memberPredicate = f => f.FamilyMembers.Any(m => m.UserLogin == login);
                predicate = predicate.MergeAnd(memberPredicate);
            }
            else
            {
                Expression<Func<Family, bool>> memberPredicate = f => f.PrivacyLevel == 0;
                predicate = predicate.MergeAnd(memberPredicate);
            }

            List<FamilyDTO> familiesToShow = new List<FamilyDTO>();
            var families = familyRepository.GetByField(predicate);
            foreach (var f in families)
            {
                int familyMembersCount = familyMemberRepository.GetByField(m => m.FamilyId == f.Id).Count();
                familiesToShow.Add(new FamilyDTO
                {
                    Id = f.Id,
                    Name = f.Name,
                    ProfileIcon = f.ProfileIcon,
                    MembersCount = familyMembersCount
                });
            }
            return familiesToShow;
        }

        public void Update(FamilyToUpdateDTO familyToUpdate)
        {
            var family = familyRepository.GetById(familyToUpdate.Id);
            if (family == null)
            {
                throw new Exception($"Family with id {familyToUpdate.Id} doesn't exist");
            }

            family.Name = familyToUpdate.Name;
            family.Description = familyToUpdate.Description;
            family.PrivacyLevel = familyToUpdate.PrivacyLevel;
            family.ProfileIcon = familyToUpdate.ProfileIcon;
            familyRepository.Update(family);
        }
    }
}
