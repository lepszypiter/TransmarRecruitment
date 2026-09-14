using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TransmarRecruitment.Data;
using TransmarRecruitment.Models;

namespace TransmarRecruitment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserCreate model)
        {
            if (_db.Users.Any(u => u.Email == model.Email)) return BadRequest(new { message = "User exists" });
            var user = new User { Email = model.Email, Name = model.Name, PasswordHash = HashPassword(model.Password) };
            _db.Users.Add(user);
            _db.SaveChanges();
            return CreatedAtAction(null, new { id = user.Id });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null) return Unauthorized(new { message = "Invalid credentials" });
            if (!VerifyPassword(model.Password, user.PasswordHash)) return Unauthorized(new { message = "Invalid credentials" });

            var token = GenerateToken(user);
            return Ok(new { token, user = new { user.Id, user.Email, user.Name } });
        }

        private string GenerateToken(User user)
        {
            var key = _config["Jwt:Key"] ?? "dev-secret-please-change";
            var issuer = _config["Jwt:Issuer"] ?? "transmar";

            var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), new Claim(JwtRegisteredClaimNames.Email, user.Email) };
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer, issuer, claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }

        public record UserCreate(string Email, string Password, string? Name);
        public record LoginModel(string Email, string Password);
    }
}
