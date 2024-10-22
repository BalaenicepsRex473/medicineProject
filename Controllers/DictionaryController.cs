using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using scrubsAPI;
using scrubsAPI.Models;
using scrubsAPI.Schemas;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        [HttpPost("icd10")]
        public async Task<IActionResult> Create([FromBody] Icd10DTO icd10DTO)
        {
            var icd10 = new Icd10();
            if (ModelState.IsValid)
            {
                icd10.code = icd10DTO.code;

                icd10.id = Guid.NewGuid();

                if (icd10DTO.parentId != null) 
                {
                    if (IcdExists(icd10DTO.parentId))
                    {
                        icd10.parentId = icd10DTO.parentId;
                    }
                    else 
                    {
                        return BadRequest("Icd10 do not exist");
                    }
                }
                icd10.name = icd10DTO.name;

                icd10.createTime = DateTime.Now;
                _context.Add(icd10);
                await _context.SaveChangesAsync();
                return Json(icd10.id);
            }
            return BadRequest();
        }

        [HttpGet("icd10/root")]
        public async Task<IActionResult> Details()
        {
            var rootDiseases = _context.Icd10s
                .Where(d => d.parentId == null)
                .Select(d => new Icd10RecordModel
            {
                code = d.code,
                id = d.id,
                name = d.name,
                createTime = d.createTime
            })
                .ToList();
            return Ok(rootDiseases);
        }

        [HttpGet("icd10")]
        public async Task<IActionResult> Details(string request = "", int pageNumber = 1, int pageSize = 5)
        {
            var totalIcds = _context.Icd10s.Count();
            var Icds = _context.Icd10s
                .OrderBy(p => p.id)
                .Where(d => d.code.Contains(request) || d.name.Contains(request))
                .Select(d => new Icd10RecordModel
                {
                    code = d.code,
                    id = d.id,
                    name = d.name,
                    createTime = d.createTime
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            float totIcds = totalIcds;
            float pgSize = pageSize;
            var response = new Icd10SearchModel
            {
                Icd10s = Icds,
                Pagination = new PageInfoModel
                {
                    size = pageSize,
                    count = (int)Math.Ceiling(totIcds / pgSize),
                    current = pageNumber
                }
            };

            return Ok(response);
        }


        [HttpGet("speciality")]
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

        private bool IcdExists(Guid? id)
        {
            return _context.Icd10s.Any(e => e.id == id);
        }

        [HttpPost("speciality")]
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
