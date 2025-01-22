using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.Service
{
    internal sealed class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TokenService> _logger;

        public TokenService(
            IConfiguration configuration,
            ApplicationDBContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<TokenService> logger
        )
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            var signingKey = _configuration["JWT:SigningKey"];
            if (string.IsNullOrEmpty(signingKey))
            {
                throw new ArgumentNullException(
                    "JWT:SigningKey",
                    "Signing key cannot be null or empty."
                );
            }
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }

        private string? GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );
            _logger.LogInformation(userId);
            return !string.IsNullOrEmpty(userId) ? userId : null;
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(
                    JwtRegisteredClaimNames.Email,
                    user.Email ?? throw new ArgumentNullException(nameof(user.Email))
                ),
                new Claim(
                    JwtRegisteredClaimNames.GivenName,
                    user.UserName ?? throw new ArgumentNullException(nameof(user.UserName))
                ),
                // Add NameIdentifier claim
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString() // Assuming user has an Id property which is the unique identifier
                ),
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(
                    _configuration.GetValue<int>("JWT:ExpirationTimeInMinutes")
                ),
                SigningCredentials = creds,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public async Task<bool> RevokeRefreshToken(string AppUserId)
        {
            if (AppUserId != GetCurrentUserId())
            {
                throw new ApplicationException("You cannot do this");
            }
            await _context.RefreshTokens.Where(r => r.AppUserId == AppUserId).ExecuteDeleteAsync();
            return true;
        }
    }
}
