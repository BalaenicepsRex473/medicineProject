using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        //Чисто парсинг icd с сайта миндздрава 
        //[HttpPost("parseAll")]
        //public async Task<IActionResult> PostICD10Data([FromBody] List<Icd10JsonDTO> icd10Data)
        //{
        //    var icd10Codes = icd10Data.Select(code => new Icd10
        //    {
        //        id = Guid.NewGuid(),
        //        code = code.MKB_CODE,
        //        name = code.MKB_NAME,
        //        createTime = DateTime.UtcNow,
        //        idFromJson = code.ID,
        //        parentIdFromJson = code.ID_PARENT.HasValue ? code.ID_PARENT.Value : null
        //    }).ToList();


        //    await PopulateDatabase(_context, icd10Codes);

        //    return Ok();
        //}


        //private static async Task PopulateDatabase(ScrubsDbContext context, List<Icd10> icd10Data)
        //{

        //    await context.Icd10s.AddRangeAsync(icd10Data);
        //    await context.SaveChangesAsync();


        //    foreach (var code in icd10Data)
        //    {
        //        if (code.parentIdFromJson.HasValue)
        //        {

        //            var parent = await context.Icd10s.FirstOrDefaultAsync(p => p.idFromJson == code.parentIdFromJson);
        //            if (parent != null)
        //            {
        //                code.parentId = parent.id;
        //                code.parent = parent;
        //            }
        //        }
        //    }
        //    await context.SaveChangesAsync();
        //}


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
                .Where(d => d.code == request || d.name.ToLower().Contains(request.ToLower()))
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
