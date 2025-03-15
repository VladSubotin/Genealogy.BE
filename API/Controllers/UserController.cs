using Application.Interfaces.IServices;
using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthenticationService authenticationService;

        public UserController(IUserService userService, IAuthenticationService authenticationService)
        {
            this.userService = userService;
            this.authenticationService = authenticationService;
        }

        [HttpPost("/login")]
        public IActionResult Login([FromBody] LoginDTO loginData)
        {
            User user;
            try
            {
                user = userService.getByLoginAndPassword(loginData);
                var token = authenticationService.GenrateToken(user);
                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Exception = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("/getSelf")]
        public IActionResult GetFullProfile()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader?.Substring("Bearer ".Length).Trim();
            var login = authenticationService.GetUserDataFromToken(token);

            UserFullProfileDTO user;
            try
            {
                user = userService.getFullProfile(login);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        [HttpGet("/getUser")]
        public IActionResult Get(string login)
        {
            UserPublicProfileDTO user;
            try
            {
                user = userService.getPublicProfile(login);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        [HttpPost("/register")]
        public IActionResult Post([FromBody] User user)
        {
            try
            {
                userService.Create(user);
                var token = authenticationService.GenrateToken(user);
                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpPut("/updateUser")]
        public IActionResult Put([FromBody] User user)
        {
            try
            {
                userService.Update(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpPut("/updatePassword")]
        public IActionResult PutPassword([FromBody] UserToUpdatePasswordDTO user)
        {
            try
            {
                userService.UpdatePassword(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpDelete("/deleteUser")]
        public IActionResult Delete([FromBody] UserToDeleteDTO user)
        {
            try
            {
                userService.Delete(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }
    }
}
