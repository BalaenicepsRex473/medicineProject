using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using scrubsAPI;

namespace scrubsAPI
{

    [ApiController]
    [Route("api/[controller]")]
    public class patientsController : Controller
    {
        private readonly ScrubsDbContext _context;

        public patientsController(ScrubsDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return Json(patient);
        }
        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> Details(int pageNumber = 1, int pageSize = 5)
        {
            var totalPatients = _context.Patients.Count();
            var patients = _context.Patients
                .OrderBy(p => p.id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            float totPat = totalPatients;
            float pgSize = pageSize;
            var response = new PatientResponceModel
            {
                Patients = patients,
                Pagination = new PageInfoModel{
                    size = pageSize,
                    count = (int)Math.Ceiling(totPat / pgSize),
                    current = pageNumber}
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody]PatientCreateModel patientDTO)
        {
            var patient = new Patient();
            if (ModelState.IsValid)
            {
                patient.id = Guid.NewGuid();
                if (patientDTO.birthday != null) 
                {
                    patient.birthDay = patientDTO.birthday;
                }
                patient.name = patientDTO.name;
                patient.gender = patientDTO.gender;
                patient.creationTime = DateTime.Now;
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return Json(patient.id);
            }
            return BadRequest();
        }

        private bool PatientExists(Guid id)
        {
            return _context.Patients.Any(e => e.id == id);
        }
    }
}
