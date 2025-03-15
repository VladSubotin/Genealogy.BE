using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IUserService
    {
        UserFullProfileDTO getFullProfile(string login);
        UserPublicProfileDTO getPublicProfile(string login);
        void Create(User user);
        void Update(User user);
        void UpdatePassword(UserToUpdatePasswordDTO userToUpdatePasswod);
        void Delete(UserToDeleteDTO userToDelete);
        User getByLoginAndPassword(LoginDTO loginData);
    }
}
