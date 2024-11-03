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
using NuGet.Protocol;
using scrubsAPI;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace scrubsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly ScrubsDbContext _context;
        private readonly TokenStorage _tokenStorage;
        public DoctorController(ScrubsDbContext context, TokenStorage tokenStorage)
        {
            _context = context;
            _tokenStorage = tokenStorage;

        }

        [ProducesResponseType<TokenResponceModel>(200)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DoctorRegisterModel doctorDTO)
        {
            var existingEmail = await _context.Doctors.FirstOrDefaultAsync(d => d.email == doctorDTO.email);
            if (existingEmail != null)
            {
                return BadRequest("This email has already used");
            }
            var hasNumber = new Regex(@"\d");


            if (!hasNumber.IsMatch(doctorDTO.password))
            {
                return BadRequest("Password have to had at least 1 number");
            }

            var spec = await _context.Specialities.FirstOrDefaultAsync(d => d.id == doctorDTO.speciality);
            if (spec == null)
            {
                return BadRequest("There are no speciality with such id");
            }

            var doctor = new Doctor { birthday = doctorDTO.birthsday,
                email = doctorDTO.email,
                gender = doctorDTO.gender,
                speciality = spec,
                id = new Guid(), name = doctorDTO.name, phone = doctorDTO.phone,
                createTime = DateTime.Now};

            var passwordHash = HashPassword(doctorDTO.password);
            doctor.password = passwordHash;


            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, doctor.id.ToString()) };

            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(15)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var token = new TokenResponceModel
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwt),
            };

            _tokenStorage.AddToken(token.token, doctor.id.ToString());
            return Ok(token);
        }


        [ProducesResponseType<TokenResponceModel>(200)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentialsModel doctor)
        {

            var user = await _context.Doctors.FirstOrDefaultAsync(d => d.email == doctor.email);
            if (user == null)
            {
                return Unauthorized("wrong email or password");
            }


            if (!VerifyPassword(doctor.password, user.password))
            {
                return Unauthorized("wrong email or password");
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.id.ToString()) };

            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(15)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var token = new TokenResponceModel
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwt),
            };
            _tokenStorage.AddToken(user.id.ToString(), token.token);
            return Ok(token);
        }

        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            var user = Guid.Parse(HttpContext.User.Identity.Name);

            bool removed = _tokenStorage.RemoveToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            if (!removed)
            {
                return BadRequest("User already logged out.");
            }

            return Ok("You're logged out.");
        }


        [ProducesResponseType<DoctorModel>(200)]
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
                return Ok();
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
