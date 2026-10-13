using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SkillSwapBackend.Models;
using SkillSwapBackend.Repositories;
using SkillSwapBackend.ViewModels;

namespace SkillSwapBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    // CREATE
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserViewModel model)
    {
       var user = new User
{
    FullName = model.FullName,
    Username = model.Username,
    Email = model.Email,
    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
    MobileNumber = model.MobileNumber
};

        var createdUser = await _userRepository.CreateUser(user);

        return Ok(createdUser);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var user = await _userRepository.GetUserByUsername(model.Username)
            ?? await _userRepository.GetUserByEmail(model.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
        {
            return Unauthorized("Invalid username/email or password");
        }

        var issuer = _configuration["Jwt:Issuer"] ?? "SkillSwapBackend";
        var audience = _configuration["Jwt:Audience"] ?? "SkillSwapClient";
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var expiresAt = DateTime.UtcNow.AddHours(1);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: expiresAt,
            signingCredentials: credentials);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        return Ok(new
        {
            accessToken,
            tokenType = "Bearer",
            expiresAt,
            user = new
            {
                user.Id,
                user.FullName,
                user.Username,
                user.Email,
                user.MobileNumber
            }
        });
    }

    // READ ALL
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();

        return Ok(users);
    }

    // READ ONE
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userRepository.GetUserById(id);

        if (user == null)
        {
            return NotFound("User not found");
        }

        return Ok(user);
    }

    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var user = await _userRepository.GetUserByUsername(username);

        if (user == null)
        {
            return NotFound("User not found");
        }

        return Ok(new
        {
            user.Id,
            user.FullName,
            user.Username,
            user.Email,
            user.MobileNumber
        });
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);

        if (user == null)
        {
            return NotFound("User not found");
        }

        return Ok(new
        {
            user.Id,
            user.FullName,
            user.Username,
            user.Email,
            user.MobileNumber
        });
    }

    // UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(
        int id,
        UserViewModel model)
    {
        var user = new User
        {
            FullName = model.FullName,
            Username = model.Username,
            Email = model.Email,
            Password = model.Password,
            MobileNumber = model.MobileNumber
        };

        var updatedUser =
            await _userRepository.UpdateUser(id, user);

        if (updatedUser == null)
        {
            return NotFound("User not found");
        }

        return Ok(updatedUser);
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var deleted = await _userRepository.DeleteUser(id);

        if (!deleted)
        {
            return NotFound("User not found");
        }

        return Ok("User deleted successfully");
    }
}