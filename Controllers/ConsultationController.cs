using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using scrubsAPI;
using scrubsAPI.Models;
using scrubsAPI.Schemas;

namespace scrubsAPI
{

    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationController : Controller
    {
        private readonly ScrubsDbContext _context;

        public ConsultationController(ScrubsDbContext context)
        {
            _context = context;
        }

        [ProducesResponseType<ConsultationModel>(200)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConsultation(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultations
                .Include(p => p.inspection)
                .Include(p => p.speciality)
                .FirstOrDefaultAsync(m => m.id == id);
            if (consultation == null)
            {
                return NotFound();
            }
            var comments = await _context.Comments
                .Include(i => i.author)
                .Include(i => i.parentComment)
                .Include(i => i.consultation)
                .Where(p => p.consultation.id == id)
                .Select(p => new CommentModel
                {
                    id = p.id,
                    createTime = p.createTime,
                    modifiedTime = p.modifiedTime,
                    content = p.content,
                    author = p.author.name,
                    authorId = p.author.id,
                    parentId = p.parentComment != null ? p.parentComment.id : null,
                })
                .ToListAsync();

            var result = new ConsultationModel
            {
                id = consultation.id,
                createTime = consultation.createTime,
                inspectionId = consultation.inspection.id,
                speciality = new SpecialityModel 
                {
                    id = consultation.speciality.id,
                    createTime = consultation.speciality.creationTime,
                    name = consultation.speciality.name,
                },
                comments = comments,
            };

            return Ok(result);
        }

        [ProducesResponseType<Guid>(200)]
        [Authorize]
        [HttpPost("{id}/comment")]
        public async Task<IActionResult> AddComment(Guid id, [FromBody] CommentCreateModel commentCreation)
        {
            if (ModelState.IsValid)
            {
                var user = Guid.Parse(HttpContext.User.Identity.Name);
                var doctor = _context.Doctors
                    .Include(p => p.speciality)
                    .FirstOrDefault(d => d.id == user);

                var consultation = await _context.Consultations
                    .Include(p => p.inspection)
                    .Include(p => p.speciality)
                    .Include(p => p.inspection.doctor)
                    .FirstOrDefaultAsync(m => m.id == id);
                if (consultation == null)
                {
                    return NotFound();
                }

                if (consultation.speciality != doctor.speciality && consultation.inspection.doctor != doctor)
                {
                    return BadRequest("User doesn't have add comment to consultation (unsuitable specialty or not the inspection author)");
                }
                var comment = new Comment
                {
                    id = Guid.NewGuid(),
                    consultation = consultation,
                    author = doctor,
                    content = commentCreation.content,
                    createTime = DateTime.Now,
                    modifiedTime = null,
                    parentComment = null,
                };
                if (commentCreation.parentId != null)
                {
                    comment.parentComment = await _context.Comments
                        .Include(i => i.author)
                        .Include(i => i.parentComment)
                        .Include(i => i.consultation)
                        .FirstOrDefaultAsync(m => m.id == commentCreation.parentId);
                }
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                return Ok(comment.id);
            }
            else 
            {
                return BadRequest();
            }
        }


        [Authorize]
        [HttpPut("comment/{id}")]
        public async Task<IActionResult> EditComment(Guid id, [FromBody] CommentEditModel commentEditing)
        {

            if (ModelState.IsValid)
            {
                var comment = await _context.Comments
                    .FirstOrDefaultAsync(m => m.id == id);
                if (comment == null)
                {
                    return NotFound();
                }

                var user = Guid.Parse(HttpContext.User.Identity.Name);
                var doctor = _context.Doctors.FirstOrDefault(d => d.id == user);

                if (doctor != comment.author)
                {
                    return Forbid("User is not the author of the comment");
                }

                comment.content = commentEditing.content;
                _context.Update(comment);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else 
            {
                return BadRequest();
            }
        }

        [Authorize]
        [ProducesResponseType<InspectionPagedListModel>(200)]
        [HttpGet("")]
        public async Task<IActionResult> GetInspections([FromQuery] List<Guid>? icdRoots = null, bool grouped = false, int page = 1, int size = 5)
        {

            var user = Guid.Parse(HttpContext.User.Identity.Name);
            var doctor = _context.Doctors
                .Include(p => p.speciality)
                .FirstOrDefault(d => d.id == user);

            var diagnoses = _context.Diagnoses
                .Include(p => p.icdDiagnosis)
                .ThenInclude(p => p.parent)
                .Include(p => p.inspection)
                .ThenInclude(p => p.patient)
                .Where(p => p.type == DiagnosisType.Main);


            if (icdRoots != null)
            {
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
                diagnoses = diagnoses.Where(p => icdIds.Contains(p.icdDiagnosis.id));
            }

            var diagnosesList = await diagnoses.ToListAsync();


            var inspectionsQuery = _context.Inspections
                .Include(d => d.patient)
                .Include(d => d.doctor)
                .Include(d => d.doctor.speciality)
                .Include(d => d.previousInspection)
                .Where(i => i.doctor.speciality == doctor.speciality)
                .ToList();


            if (grouped)
            {
                inspectionsQuery = inspectionsQuery.Where(p => p.previousInspection == null).ToList();
            }


            var inspectionsWithDiagnosis = inspectionsQuery
                .Select(d => new InspectionPreviewModel
                {
                    id = d.id,
                    createTime = d.createTime,
                    previousId = d.previousInspection != null ? d.previousInspection.id : null,
                    date = d.date,
                    conclusion = d.conclusion,
                    doctorId = d.doctor.id,
                    doctor = d.doctor.name,
                    patientId = d.patient.id,
                    patient = d.patient.name,
                    hasChain = d.nextVisitDate != null,
                    hasNested = d.previousInspection != null,
                    diagnosis = diagnosesList.Where(p => p.inspection.id == d.id).Select(p => toDiagnosisModel(p)).FirstOrDefault(),
                });


            var filteredInspections = inspectionsWithDiagnosis.Where(p => p.diagnosis != null);

            float totalInspections = filteredInspections.Count();
            float pageSize = size;
            var response = new InspectionPagedListModel
            {
                inspections = filteredInspections
                    .Skip((page - 1) * size)
                    .Take(size).ToList(),
                pagination = new PageInfoModel
                {
                    size = size,
                    count = (int)Math.Ceiling(totalInspections / pageSize),
                    current = page
                }
            };

            return Ok(response);
        }

        private DiagnosisModel toDiagnosisModel(Diagnosis? diagnosis)
        {
            return new DiagnosisModel
            {
                id = diagnosis.id,
                createTime = diagnosis.createTime,
                description = diagnosis.description,
                code = diagnosis.icdDiagnosis.code,
                name = diagnosis.icdDiagnosis.name,
                type = diagnosis.type
            };
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
