using Application.Interfaces.IServices;
using Application.Models;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyMemberController : ControllerBase
    {
        private readonly IFamilyMemberService familyMemberService;
        private readonly Application.Interfaces.IServices.IAuthenticationService authenticationService;

        public FamilyMemberController(IFamilyMemberService familyMemberService, Application.Interfaces.IServices.IAuthenticationService authenticationService)
        {
            this.familyMemberService = familyMemberService;
            this.authenticationService = authenticationService;
        }

        //[Authorize]
        [HttpPost("/addMember")]
        public IActionResult Post([FromBody] FamilyMemberToAddDTO familyMember)
        {
            try
            {
                familyMemberService.Create(familyMember);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpPut("/updateMember")]
        public IActionResult Put([FromBody] FamilyMemberToUpdateDTO familyMember)
        {
            try
            {
                familyMemberService.Update(familyMember);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpDelete("/removeMember")]
        public IActionResult Delete(string id)
        {
            try
            {
                familyMemberService.Delete(Guid.Parse(id));
                return Ok();
            }
            catch (FormatException)
            {
                return BadRequest($"Invalid GUID format: {id}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpPut("/changeAdmin")]
        public IActionResult ChangeAdmin(FamilyMemberToChangeAdminDTO familyMemberToChangeAdmin)
        {
            try
            {
                familyMemberService.changeFamilyAdmin(familyMemberToChangeAdmin);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        [HttpGet("/getMemberFamilies")]
        public IActionResult GetUserFamilies()
        {
            string userLogin = String.Empty;
            var authHeader = HttpContext?.Request?.Headers?["Authorization"].FirstOrDefault();
            var token = authHeader?.Length > 7 == true ? authHeader.Substring("Bearer ".Length).Trim() : null;
            userLogin = token == null ? String.Empty : authenticationService.GetUserDataFromToken(token);

            try
            {
                var userFamilies = familyMemberService.getUserFamilies(userLogin);
                return Ok(userFamilies);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        [HttpGet("/getFamilyMembers")]
        public IActionResult GetFamilyUsers(string id)
        {
            try
            {
                var familyUsers = familyMemberService.getFamilyMembers(Guid.Parse(id));
                return Ok(familyUsers);
            }
            catch (FormatException)
            {
                return BadRequest($"Invalid GUID format: {id}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }
    }
}
