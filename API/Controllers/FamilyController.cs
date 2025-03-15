using Application.Interfaces.IServices;
using Application.Models;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyController : ControllerBase
    {
        private readonly IFamilyService familyService;
        private readonly Application.Interfaces.IServices.IAuthenticationService authenticationService;

        public FamilyController(IFamilyService familyService, Application.Interfaces.IServices.IAuthenticationService authenticationService)
        {
            this.familyService = familyService;
            this.authenticationService = authenticationService;
        }

        [Authorize]
        [HttpPost("/addFamily")]
        public IActionResult Post([FromBody] FamilyToCreateDTO family)
        {
            string userLogin = String.Empty;
            var authHeader = HttpContext?.Request?.Headers?["Authorization"].FirstOrDefault();
            var token = authHeader?.Length > 7 == true ? authHeader.Substring("Bearer ".Length).Trim() : null;
            userLogin = token == null ? String.Empty : authenticationService.GetUserDataFromToken(token);
            family.AdminLogin = userLogin;

            try
            {
                return Ok(familyService.Create(family));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpPut("/updateFamily")]
        public IActionResult Put([FromBody] FamilyToUpdateDTO family)
        {
            try
            {
                familyService.Update(family);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpDelete("/removeFamily")]
        public IActionResult Delete(string id)
        {
            try
            {
                familyService.Delete(Guid.Parse(id));
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

        [HttpGet("/getFamilies")]
        public IActionResult GetFamilies(string? name)
        {
            string userLogin = String.Empty;
            var authHeader = HttpContext?.Request?.Headers?["Authorization"].FirstOrDefault();
            var token = authHeader?.Length > 7 == true ? authHeader.Substring("Bearer ".Length).Trim() : null;
            userLogin = token == null ? String.Empty : authenticationService.GetUserDataFromToken(token);

            try
            {
                var families = familyService.getAll(name, userLogin);
                return Ok(families);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpGet("/getFamily")]
        public IActionResult GetFamily(string id)
        {
            string userLogin = String.Empty;
            var authHeader = HttpContext?.Request?.Headers?["Authorization"].FirstOrDefault();
            var token = authHeader?.Length > 7 == true ? authHeader.Substring("Bearer ".Length).Trim() : null;
            userLogin = token == null ? String.Empty : authenticationService.GetUserDataFromToken(token);
            
            try
            {
                var family = familyService.get(Guid.Parse(id), userLogin);
                return Ok(family);
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
