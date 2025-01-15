using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        //чисто парсинг icd с сайта миндздрава 
        //[httppost("parseall")]
        //public async task<iactionresult> posticd10data([frombody] list<icd10jsondto> icd10data)
        //{
        //    var icd10codes = icd10data.select(code => new icd10
        //    {
        //        id = guid.newguid(),
        //        code = code.mkb_code,
        //        name = code.mkb_name,
        //        createtime = datetime.utcnow,
        //        idfromjson = code.id,
        //        parentidfromjson = code.id_parent.hasvalue ? code.id_parent.value : null
        //    }).tolist();


        //    await populatedatabase(_context, icd10codes);

        //    return ok();
        //}


        //private static async task populatedatabase(scrubsdbcontext context, list<icd10> icd10data)
        //{

        //    await context.icd10s.addrangeasync(icd10data);
        //    await context.savechangesasync();


        //    foreach (var code in icd10data)
        //    {
        //        if (code.parentidfromjson.hasvalue)
        //        {

        //            var parent = await context.icd10s.firstordefaultasync(p => p.idfromjson == code.parentidfromjson);
        //            if (parent != null)
        //            {
        //                code.parentid = parent.id;
        //                code.parent = parent;
        //            }
        //        }
        //    }
        //    await context.savechangesasync();
        //}


        [ProducesResponseType<Icd10RecordModel>(200)]
        [HttpGet("icd10/root")]
        public async Task<IActionResult> GetIcdRoots()
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

        [ProducesResponseType<Icd10SearchModel>(200)]
        [HttpGet("icd10")]
        public async Task<IActionResult> GetConcreteIcds(string request = "", int pageNumber = 1, int pageSize = 5)
        {
            var totalIcds = _context.Icd10s.Where(d => d.code == request || d.name.ToLower().Contains(request.ToLower())).Count();
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

  
        [ProducesResponseType<SpecialitiesPagedListModel>(200)]
        [HttpGet("speciality")]
        public async Task<IActionResult> GetSpecialities(string name = "", int pageNumber = 1, int pageSize = 5)
        {
            var totalSpecialities = _context.Specialities.Where(p => p.name.ToLower().Contains(name)).Count();
            var specialities = _context.Specialities
                .Where(p => p.name.ToLower().Contains(name))
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

        [Authorize]
        [ProducesResponseType<Guid>(200)]
        [HttpPost("speciality")]
        public async Task<IActionResult> Create([FromBody] SpecialityCreateModel specialityDTO)
        {
            var speciality = new Speciality();
            if (ModelState.IsValid)
            {
                speciality.id = Guid.NewGuid();

                speciality.name = specialityDTO.name;

                speciality.creationTime = DateTime.Now;
                _context.Add(speciality);
                await _context.SaveChangesAsync();
                return Ok(speciality.id);
            }
            return BadRequest();
        }
    }
}
