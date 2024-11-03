using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using scrubsAPI;
using scrubsAPI.Models;
using scrubsAPI.Schemas;

namespace scrubsAPI
{

    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly ScrubsDbContext _context;

        public ReportController(ScrubsDbContext context)
        {
            _context = context;
        }

        [ProducesResponseType<IcdRootsReportModel>(200)]
        [Authorize]
        [HttpGet("icdrootsreport")]
        public async Task<IActionResult> GetIcdRootsReport([FromQuery] List<Guid> icdRoots, [FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            if (icdRoots == null || !icdRoots.Any() || !ModelState.IsValid)
            {
                return BadRequest("Some fields in request are invalid");
            }


            var icd10s = new List<Icd10>();
            var icdIds = new List<Guid>();
            var icd10Roots = _context.Icd10s.Where(i => i.parentId == null).Select(i => i.id).ToList();
            foreach (var icd in icdRoots)
            {
                if (!icd10Roots.Contains(icd))
                {
                    return BadRequest("Entered icd10 id isn't root element");
                }
            }
            foreach (var icd in icdRoots)
            {
                await GetIcdsByRoot(icd, icd10s);
                icdIds.AddRange(icd10s.Select(i => i.id));
                icd10s.Clear();
            }
                


            var inspections = await _context.Diagnoses
                .Include(d => d.inspection)
                .Include(d => d.icdDiagnosis)
                .Include(d => d.inspection.patient)
                .Where(d => icdIds.Contains(d.icdDiagnosis.id) &&
                            d.inspection.date >= start &&
                            d.inspection.date <= end)
                .ToListAsync();

            var records = inspections
                .GroupBy(d => d.inspection.patient.id)
                .Select(g => new IcdRootsReportRecordModel
                {
                    patientName = g.First().inspection.patient.name,
                    patientBirthdate = g.First().inspection.patient.birthDay,
                    gender = g.First().inspection.patient.gender,
                    visitsByRoot = g
                                .GroupBy(d => d.icdDiagnosis.id)
                                .ToDictionary(
                            diag => diag.Key.ToString(),
                            diag => diag.Select(d => d.inspection.id).Distinct().Count())
                }).ToList();

            var summaryByRoot = inspections
                .GroupBy(d => d.icdDiagnosis.id)
                .ToDictionary(
                    g => g.Key.ToString(),
                    g => g.Select(d => d.inspection.id).Distinct().Count()
                 );

            var response = new IcdRootsReportModel
            {
                filters = new IcdRootsReportFiltersModel
                {
                    start = start,
                    end = end,
                    icdRoots = icdRoots
                },
                records = records,
                summaryByRoot = summaryByRoot
            };
            return Ok(response);
        }

        private async Task GetIcdsByRoot(Guid parentId, List<Icd10> results)
        {

            var children = await _context.Icd10s
                .Where(i => i.parentId == parentId)
                .ToListAsync();


            results.AddRange(children);


            foreach (var child in children)
            {
                await GetIcdsByRoot(child.id, results);
            }
        }
    }
}