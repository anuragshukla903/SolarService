using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SolarService.Data;
using SolarService.DataContract;
using SolarService.Models;

namespace SolarService.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

       private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // ✅ REGISTER
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("User Registered");
        }

        // ✅ LOGIN
        [HttpPost("login")]
        public IActionResult Login(LoginDTO model)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == model.Email);

            if (user == null)
                return Unauthorized("Invalid Email");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
            if (!isPasswordValid)
                return Unauthorized("Invalid Password");

            var token = GenerateToken(user);
            return Ok(new { token });
        }

        // ✅ TOKEN GENERATOR (NULL SAFE)
        private string GenerateToken(User user)
        {
            var jwtKey = _config["Jwt:Key"];

            if (string.IsNullOrEmpty(jwtKey))
                throw new Exception("JWT Key missing in appsettings.json");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}