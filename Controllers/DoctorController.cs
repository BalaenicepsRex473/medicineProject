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
using scrubsAPI.Schemas;

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
            var existingEmail = await _context.Doctors.FirstOrDefaultAsync(d => d.email == doctorDTO.email);
            if (existingEmail != null)
            {
                return BadRequest();
            }
            var doctor = new Doctor { birthday = doctorDTO.birthsday,
                email = doctorDTO.email,
                gender = doctorDTO.gender,
                speciality = doctorDTO.speciality,
                id = new Guid(), name = doctorDTO.name, phone = doctorDTO.phone,
                createTime = DateTime.Now};

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

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.id.ToString()) };

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

        [HttpPost("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetProfile()
        {
            var user = Guid.Parse(HttpContext.User.Identity.Name);
            var doctor = _context.Doctors.FirstOrDefault(d => d.id == user);
            var doc = new DoctorModel
            {
                email = doctor.email,
                name = doctor.name,
                creteTime = doctor.createTime,
                birthsday = doctor.birthday,
                id = doctor.id,
                gender = doctor.gender,
                phone = doctor.phone,
            };
            return Ok(doc);
        }

        [HttpPut("edit")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EditProfile([FromBody] DoctorEditModel doctor)
        {
            var user = Guid.Parse(HttpContext.User.Identity.Name);
            if (ModelState.IsValid) {
                var doc = _context.Doctors.FirstOrDefault(d => d.id == user);

                if (doctor.birthday.HasValue)
                {
                    doc.birthday = doctor.birthday.Value;
                }

                if (doctor.phone != null)
                {
                    doc.phone = doctor.phone;
                }

                doc.email = doctor.email;
                doc.gender = doctor.gender;
                doc.email = doctor.name;

                _context.Update(doc);
                await _context.SaveChangesAsync();
                return Ok(doc);
            }
            return BadRequest();

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
