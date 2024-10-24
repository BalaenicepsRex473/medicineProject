using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace scrubsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly ScrubsDbContext _context;

        public DoctorController(ScrubsDbContext context)
        {
            _context = context;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DoctorRegisterModel doctorDTO)
        {
            var existingDoctor = await _context.Doctors.FirstOrDefaultAsync(d => d.name == doctorDTO.name);
            if (existingDoctor != null)
            {
                return BadRequest();
            }
            var doctor = new Doctor { birthday = doctorDTO.birthsday,
                email = doctorDTO.email,
                gender = doctorDTO.gender,
                speciality = doctorDTO.speciality,
                id = new Guid(), name = doctorDTO.name, phone = doctorDTO.phone };

            var passwordHash = HashPassword(doctorDTO.password);
            doctor.password = passwordHash;


            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentialsModel doctor)
        {

            var user = await _context.Doctors.FirstOrDefaultAsync(d => d.email == doctor.email);
            if (user == null)
            {
                return Unauthorized();
            }


            if (!VerifyPassword(doctor.password, user.password))
            {
                return Unauthorized();
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.name) };

            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(15)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
        }

        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Logout()
        {
            return Ok();
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedPassword);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var providedHash = HashPassword(password);
            return providedHash == hashedPassword;
        }
    }
}
