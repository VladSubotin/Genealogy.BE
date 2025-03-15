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
    public class UserService : IUserService
    {
        private readonly IRepository<User, string> userRepository;
        private readonly IRepository<FamilyMember, Guid> familyMemberRepository;
        private readonly IRepository<Family, Guid> familyRepository;

        public UserService(IRepository<User, string> userRepository, IRepository<FamilyMember, Guid> familyMemberRepository, IRepository<Family, Guid> familyRepository)
        {
            this.userRepository = userRepository;
            this.familyMemberRepository = familyMemberRepository;
            this.familyRepository = familyRepository;

        }

        public void Create(User newUser)
        {
            var user = userRepository.GetById(newUser.Login);
            if (user != null) 
            {
                throw new Exception($"user {user.Login} already exists");
            }
            userRepository.Add(newUser);
        }

        public void Delete(UserToDeleteDTO userToDelete)
        {
            var user = userRepository.GetById(userToDelete.Login);
            if (user != null)
            {
                if (user.HashPassword != userToDelete.HashPassword)
                {
                    throw new Exception($"password isn't correct");
                }

                var adminPermissionsCount = familyMemberRepository
                    .GetByField(m => m.UserLogin == user.Login && m.Role == "Admin", 1).Count();
                if (adminPermissionsCount != 0)
                {
                    throw new Exception("you should transfer your admin permissions to delete own profile");
                }
                userRepository.Delete(userToDelete.Login);
            }
        }

        public User getByLoginAndPassword(LoginDTO loginData)
        {
            var user = userRepository.GetById(loginData.Login);
            if (user == null)
            {
                throw new Exception($"user {loginData.Login} doesn't exist");
            }
            if (user.HashPassword != loginData.HashPassword)
            {
                throw new Exception($"password isn't correct");
            }
            return user;
        }

        public UserFullProfileDTO getFullProfile(string login)
        {
            var user = userRepository.GetById(login);
            if (user == null)
            {
                throw new Exception($"user {login} doesn't exist");
            }

            return new UserFullProfileDTO
            {
                Login = login,
                Name = user.Name,
                Email = user.Email,
                BirthDate = user.BirthDate,
                ProfileIcon = user.ProfileIcon,
                Description = user.Description
            };
        }

        public UserPublicProfileDTO getPublicProfile(string login)
        {
            var user = userRepository.GetById(login);
            if (user == null)
            {
                throw new Exception($"user {login} doesn't exist");
            }

            var userFamilies = new List<UserFamiliesDTO>();
            var familyMembers = familyMemberRepository.GetByField(m => m.UserLogin == login, 5);
            foreach (var familyMember in familyMembers)
            {
                var family = familyRepository.GetById((Guid)familyMember.FamilyId);
                userFamilies.Add( new UserFamiliesDTO
                    { 
                        Id = familyMember.Id,
                        FamilyId = family.Id,
                        FamilyName = family.Name,
                        Role = familyMember.Role,
                        FamilyProfileIcon = family.ProfileIcon
                    });
            }

            int? ageInTens = null;
            if (user.BirthDate != null)
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Today);
                int differenceInDays = today.DayNumber - ((DateOnly)user.BirthDate).DayNumber;
                double differenceInyears = differenceInDays / 365.25;
                ageInTens = (int)Math.Floor(differenceInyears);
            }

            return new UserPublicProfileDTO
            {
                Login = login,
                Name = user.Name,
                Description = user.Description,
                ProfileIcon = user.ProfileIcon,
                AgeInTens = ageInTens,
                Families = userFamilies
            };
        }

        public void Update(User userToUpdate)
        {
            var user = userRepository.GetById(userToUpdate.Login);
            if (user == null)
            {
                throw new Exception($"user {userToUpdate.Login} doesn't exist");
            }
            if (user.HashPassword != userToUpdate.HashPassword)
            {
                throw new Exception($"password isn't correct");
            }
            user.Name = userToUpdate.Name;
            user.Description = userToUpdate.Description;
            user.ProfileIcon = userToUpdate.ProfileIcon;
            user.Email = userToUpdate.Email;
            user.BirthDate = userToUpdate.BirthDate;

            userRepository.Update(user);
        }

        public void UpdatePassword(UserToUpdatePasswordDTO userToUpdatePasswod)
        {
            var user = userRepository.GetById(userToUpdatePasswod.Login);
            if (user == null)
            {
                throw new Exception($"user {userToUpdatePasswod.Login} doesn't exist");
            }
            if (user.HashPassword != userToUpdatePasswod.HashPasswordOld)
            {
                throw new Exception($"password isn't correct");
            }
            user.HashPassword = userToUpdatePasswod.HashPasswordNew;
            userRepository.Update(user);
        }
    }
}
