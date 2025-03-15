using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    internal class PermissionService
    {
        //public bool HaveAccess(string userLogin, Guid familyId, int methodId)
        //{
        //    var privacyType = privacyTypeRepository.Get(
        //        p => p.Families.Where(f => f.Id == familyId))
        //        .FirstOrDefault();
        //    var role = roleRepository.Get(
        //        r => r.FamilyMembers.Where(
        //            m => m.familyId == familyId && m.userLogin == userLogin))
        //        .FirstOrDefault();
        //    var guestRole = roleService.GetDefault();

        //    var permission = permissionRepository.Get(
        //        p => p.Method.Id == methodId
        //        && p.PrivacyType.Id == privacyType.Id
        //        && p.Role.Id == (role?.Id ?? guestRoleId))
        //        .FirstOrDefault();
        //    return permission?.haveAccess ?? false;
        //}
    }
}
