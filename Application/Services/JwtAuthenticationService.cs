using Application.Interfaces.IServices;
using Application.Models;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class JwtAuthenticationService : IAuthenticationService
    {
        private readonly JwtSettings jwtSettings;

        public JwtAuthenticationService(IConfiguration configuration) 
        {
            jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
        }

        public string GenrateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Login)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.TokenExpiryInMinutes),
                SigningCredentials = credentials,
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        public string GetUserDataFromToken(string securityToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            var claimsPrincipal = tokenHandler.ValidateToken(securityToken, tokenValidationParameters, out validatedToken);
            var claim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            return claim.Value;
        }
    }
}
