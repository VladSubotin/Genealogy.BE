using Application.Interfaces.IServices;
using Application.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly GenealogyDbContext _context;
        private readonly IAuthenticationService _authenticationService;

        public WeatherForecastController(GenealogyDbContext context, IAuthenticationService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        [HttpPost("/login2")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginData)
        {
            var user = await _context.Users
                .Include(p => p.FamilyMembers)
                .FirstOrDefaultAsync(u => u.Login == loginData.Login 
                    && u.HashPassword == loginData.HashPassword);
            if (user == null)
            {
                return Unauthorized();
            }
            else
            {
                var token = _authenticationService.GenrateToken(user);
                return Ok(new { token = token});
            }
        }

        [Authorize]
        [HttpGet("/getUser2")]
        public async Task<IActionResult> GetUser()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader?.Substring("Bearer ".Length).Trim();
            var userLogin = _authenticationService.GetUserDataFromToken(token);

            var user = await _context.Users
                .Include(p => p.FamilyMembers)
                .FirstOrDefaultAsync(u => u.Login == userLogin);
            if (user == null)
            {
                return BadRequest($"User {userLogin} hasn't been found");
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpGet("/getUsers2")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Users.Include(p => p.FamilyMembers).ToListAsync());
        }
    }
}
