using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using scrubsAPI;
using scrubsAPI.Models;

namespace scrubsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DictionaryController : Controller
    {
        private readonly ScrubsDbContext _context;
        public DictionaryController(ScrubsDbContext context)
        {
            _context = context;
        }

        [HttpGet("icd10")]
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

        [HttpGet()]
        public async Task<IActionResult> Details(int pageNumber = 1, int pageSize = 5)
        {
            var totalSpecialities = _context.Specialities.Count();
            var specialities = _context.Specialities
                .OrderBy(p => p.id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            float totSpec = totalSpecialities;
            float pgSize = pageSize;
            var response = new SpecialitiesPagedListModel
            {
                Specialities = specialities,
                Pagination = new PageInfoModel
                {
                    size = pageSize,
                    count = (int)Math.Ceiling(totSpec / pgSize),
                    current = pageNumber
                }
            };

            return Ok(response);
        }


        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] SpecialityDTO specialityDTO)
        {
            var speciality = new Speciality();
            if (ModelState.IsValid)
            {
                speciality.id = Guid.NewGuid();

                speciality.name = specialityDTO.name;

                speciality.creationTime = DateTime.Now;
                _context.Add(speciality);
                await _context.SaveChangesAsync();
                return Json(speciality.id);
            }
            return BadRequest();
        }
    }
}
