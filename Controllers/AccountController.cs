using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Account;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        private readonly SignInManager<AppUser> _signinManager;

        private readonly IConfiguration _configuration;
        private readonly ApplicationDBContext _context;

        public sealed record TokenResponse(string AccessToken, string RefreshToken);

        public AccountController(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            SignInManager<AppUser> signinManager,
            IConfiguration configuration,
            ApplicationDBContext context
        )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signinManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            // Validate the request payload
            if (!ModelState.IsValid)
                return BadRequest("Invalid request data");

            // Normalize the username for case-insensitive matching
            var normalizedUsername = loginDto.Username?.Trim().ToLower();
            if (string.IsNullOrEmpty(normalizedUsername))
                return BadRequest("Username is required");

            // Fetch the user
            var user = await _userManager.Users.FirstOrDefaultAsync(u =>
                u.UserName == normalizedUsername
            );
            if (user == null)
                return Unauthorized("Invalid username or password");

            // Verify the password
            var signInResult = await _signinManager.CheckPasswordSignInAsync(
                user,
                loginDto.Password,
                lockoutOnFailure: false
            );
            if (!signInResult.Succeeded)
                return Unauthorized("Invalid username or password");

            // Generate and return the JWT token
            var token = _tokenService.CreateToken(user);
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                AppUserId = user.Id,
                Token = _tokenService.GenerateRefreshToken(),
                ExpiresOnUtc = DateTime.Now.AddDays(7),
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            var userDto = new NewUserDto
            {
                UserName = user.UserName!,
                Email = user.Email!,
                AccessToken = token,
                RefreshToken = refreshToken.Token,
            };

            return Ok(userDto);
        }

        [HttpPost("users/refresh-token")]
        public async Task<IActionResult> LoginUserWithRefreshToken(
            [FromBody] RefreshTokenDto request
        )
        {
            RefreshToken? refreshToken = await _context
                .RefreshTokens.Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == request.RefreshToken);
            if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
            {
                throw new ApplicationException("The Refresh Token Has Expired");
            }
            string accessToken = _tokenService.CreateToken(refreshToken.User);
            // Generate new refresh token
            string newRefreshToken = _tokenService.GenerateRefreshToken();

            // Mark the old token for deletion by setting its expiration to now
            refreshToken.ExpiresOnUtc = DateTime.UtcNow;

            var newToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                AppUserId = refreshToken.User.Id,
                Token = newRefreshToken,
                ExpiresOnUtc = DateTime.Now.AddDays(7),
            };
            _context.RefreshTokens.Add(newToken);
            await _context.SaveChangesAsync();
            var UserToken = new TokenResponse(accessToken, newRefreshToken);
            return Ok(UserToken);
        }

        [HttpDelete("users/{id}/refresh-tokens")]
        public async Task<IActionResult> RevokeRefreshTokens(string id)
        {
            bool success = await _tokenService.RevokeRefreshToken(id);
            return success ? NoContent() : BadRequest();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        var refreshToken = new RefreshToken
                        {
                            Id = Guid.NewGuid(),
                            AppUserId = appUser.Id,
                            Token = _tokenService.GenerateRefreshToken(),
                            ExpiresOnUtc = DateTime.Now.AddDays(7),
                        };
                        _context.RefreshTokens.Add(refreshToken);
                        await _context.SaveChangesAsync();
                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                AccessToken = _tokenService.CreateToken(appUser),
                                RefreshToken = refreshToken.Token,
                            }
                        );
                    }
                    else
                    {
                        return BadRequest(roleResult.Errors);
                    }
                }
                else
                {
                    return BadRequest(createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
