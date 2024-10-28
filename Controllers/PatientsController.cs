using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class PatientController : Controller
    {
        private readonly ScrubsDbContext _context;

        public PatientController(ScrubsDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(Guid? id)
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
        public async Task<IActionResult> GetPatients(int pageNumber = 1, int pageSize = 5)
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
                Pagination = new PageInfoModel
                {
                    size = pageSize,
                    count = (int)Math.Ceiling(totPat / pgSize),
                    current = pageNumber
                }
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> CreatePatient([FromBody] PatientCreateModel patientDTO)
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

        [Authorize]
        [HttpPost("{id}/inspections")]
        public async Task<IActionResult> CreateInspection(Guid id, [FromBody] InspectionCreateModel inspectionDTO)
        {
            var user = Guid.Parse(HttpContext.User.Identity.Name);
            var doctor = _context.Doctors.FirstOrDefault(d => d.id == user);
            var inspection = new Inspection();
            if (ModelState.IsValid)
            {
                inspection.doctor = doctor;
                inspection.patient = _context.Patients.FirstOrDefault(d => d.id == id);
                inspection.id = Guid.NewGuid();
                inspection.createTime = DateTime.Now;
                inspection.date = inspectionDTO.date;
                inspection.anamesis = inspectionDTO.anamesis;
                inspection.complaints = inspectionDTO.complaints;
                inspection.treatment = inspectionDTO.treatment;
                inspection.conclusion = inspectionDTO.conclusion;
                if (inspection.conclusion.ToString() == "Recovery")
                {
                    if (inspection.nextVisitDate.HasValue || inspection.deathTime.HasValue)
                    {
                        return BadRequest("Patient has recoveried, he has no death time or next visit");
                    }
                    inspection.nextVisitDate = null;
                    inspection.deathTime = null;
                }
                else if (inspectionDTO.conclusion.ToString() == "Death")
                {
                    if (inspectionDTO.nextVisitDate.HasValue || !inspectionDTO.deathTime.HasValue)
                    {
                        return BadRequest("Patient has dead, cant have next visit");
                    }
                    inspection.deathTime = inspectionDTO.deathTime;
                }
                else
                {
                    if (!inspectionDTO.nextVisitDate.HasValue || inspectionDTO.deathTime.HasValue)
                    {
                        return BadRequest("Patient is ill, he cant be dead");
                    }
                    inspection.nextVisitDate = inspectionDTO.nextVisitDate;
                }
                if (inspectionDTO.previousInspectionId != null)
                {
                    inspection.previousInspection = await _context.Inspections.FirstOrDefaultAsync(m => m.id == inspectionDTO.previousInspectionId);
                }
                _context.Add(inspection);
                await _context.SaveChangesAsync();
                foreach (var diagnosis in inspectionDTO.diagnoses) {
                    var diagnose = new Diagnosis
                    {
                        id = Guid.NewGuid(),
                        icdDiagnosis = await _context.Icd10s.FirstOrDefaultAsync(m => m.id == diagnosis.icdDiagnosisId),
                        inspection = inspection,
                        type = diagnosis.type,
                        description = diagnosis.description,
                        createTime = DateTime.Now
                    };
                    _context.Add(diagnose);
                }
                await _context.SaveChangesAsync();
                var consultations = new List<Consultation>();
                var comments = new List<Comment>();
                foreach (var consultation in inspectionDTO.consultations)
                {
                    var consult = new Consultation
                    {
                        id = Guid.NewGuid(),
                        createTime = DateTime.Now,
                        inspection = inspection,
                        speciality = await _context.Specialities.FirstOrDefaultAsync(m => m.id == consultation.specialityId),
                    };
                    var comment = new Comment
                    {
                        id = Guid.NewGuid(),
                        content = consultation.comment.content,
                        author = doctor,
                        consultation = consult,
                        createTime = DateTime.Now,
                        modifiedTime = null,
                        parentComment = null
                    };
                    _context.Add(comment);
                    _context.Add(consult);
                }
                await _context.SaveChangesAsync();


                return Ok(Json(inspection.id));
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("{id}/inspections")]
        public async Task<IActionResult> GetInspection(Guid id, int page = 1, int size = 5)
        {
            var diagnoses = _context.Diagnoses.Where(p => p.inspection.patient.id == id)
                    .Select(p => new DiagnosisModel
                    {
                        id = p.id,
                        createTime = p.createTime,
                        code = p.icdDiagnosis.code,
                        name = p.icdDiagnosis.name,
                        description = p.description,
                        type = p.type

                    }).ToList();

            var totalIns = _context.Inspections.Where(d => d.patient.id == id).Count();
            var ins = _context.Inspections
                .OrderBy(d => d.id)
                .Where(d => d.patient.id == id)
                .Select(d => new InspectionPreviewModel
                {
                    id = d.id,
                    createTime = d.createTime,
                    previousId = d.previousInspection.id,
                    date = d.date,
                    conclusion = d.conclusion,
                    doctorId = d.doctor.id,
                    doctor = d.doctor.name,
                    patientId = d.patient.id,
                    patient = d.patient.name,
                    hasChain = d.nextVisitDate != null ? true : false,
                    hasNested = d.previousInspection != null ? true : false,
                    diagnosis = diagnoses
                })
                .Skip((page - 1) * size)
                .Take(page)
            .ToList();


            float totIns = totalIns;
            float pgSize = page;
            var response = new InspectionPagedListModel
            {
                inspections = ins,
                pagination = new PageInfoModel
                {
                    size = size,
                    count = (int)Math.Ceiling(totIns / pgSize),
                    current = page
                }
            };

            return Ok(response);
        }
    }
}
