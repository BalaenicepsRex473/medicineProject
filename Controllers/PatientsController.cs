using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Numerics;
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

        [ProducesResponseType<Patient>(200)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p = await _context.Patients
                .FirstOrDefaultAsync(m => m.id == id);
            if (p == null)
            {
                return NotFound();
            }
            var result = new PatientModel
            {
                birthday = p.birthDay,
                createTime = p.creationTime,
                gender = p.gender,
                id = p.id,
                name = p.name,
            };


            return Ok(result);
        }

        [ProducesResponseType<PatientResponceModel>(200)]
        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetPatients([FromQuery] string? name, int pageNumber = 1, int pageSize = 5, bool scheduledVisits = false, bool onlyMine = false, Conclusion? conclusion = null, PatientSorting? patientSorting = null)
        {
            


            var patients = _context.Patients.Select(p => new PatientModel
            {
                birthday = p.birthDay,
                createTime = p.creationTime,
                gender = p.gender,
                id = p.id,
                name = p.name,
            });
            if (scheduledVisits)
            {
                patients = patients
                    .Where(patient => _context.Inspections
                        .Include(p => p.patient)
                        .Any(p => p.nextVisitDate != null && p.patient.id == patient.id));
            }

            if (onlyMine) 
            {
                var user = Guid.Parse(HttpContext.User.Identity.Name);
                var doctor = _context.Doctors.FirstOrDefault(d => d.id == user);
                patients = patients
                    .Where(patient => _context.Inspections
                        .Include(p => p.patient)
                        .Include(p => p.doctor)
                        .Any(p => p.doctor == doctor && p.patient.id == patient.id));

            }

            if (conclusion.HasValue)
            {
                switch (conclusion)
                {
                    case Conclusion.Disease:
                        {
                        patients = patients
                            .Where(patient => _context.Inspections
                                .Include(p => p.patient)
                                .Any(p => p.conclusion == Conclusion.Disease && p.patient.id == patient.id));
                        break;
                        }
                    case Conclusion.Death:
                        {
                        patients = patients
                            .Where(patient => _context.Inspections
                                .Include(p => p.patient)
                                .Any(p => p.conclusion == Conclusion.Death && p.patient.id == patient.id));
                        break;
                        }
                    case Conclusion.Recovery:
                        {
                        patients = patients
                            .Where(patient => _context.Inspections
                            .Include(p => p.patient)
                            .Any(p => p.conclusion == Conclusion.Recovery && p.patient.id == patient.id));
                        break;
                        }
                }
            }

            if (patientSorting.HasValue)
            {
                switch(patientSorting)
                   {
                    case PatientSorting.NameAsc:
                        patients = patients.OrderBy(p => p.name);
                        break;
                    case PatientSorting.NameDesc:
                        patients = patients.OrderBy(p => p.name).Reverse();
                        break;
                    case PatientSorting.CreateAsc:
                        patients = patients.OrderBy(p => p.createTime);
                        break;
                    case PatientSorting.CreateDesc:
                        patients = patients.OrderBy(p => p.createTime).Reverse();
                        break;
                    case PatientSorting.InspectionAsc:
                        patients = patients.Join(_context.Inspections,
                            patient => patient.id,
                            i => i.patient.id,
                            (patient, i) => new { patient = patient, date = i.date })
                                .OrderBy(ex => ex.date)
                                .Select(p => p.patient)
                                .Distinct();
                        break;
                    case PatientSorting.InspectionDesc:
                        patients = patients.Join(_context.Inspections,
                            patient => patient.id,
                            i => i.patient.id,
                            (patient, i) => new { patient = patient, date = i.date })
                                .OrderBy(ex => ex.date).Reverse()
                                .Select(p => p.patient)
                                .Distinct();

                        break;
                }

            }

            if (name != null) 
            {
                patients = patients.Where(p => p.name.ToLower().Contains(name.ToLower()));
            }

            float totPat = patients.Count();
            float pgSize = pageSize;
            var response = new PatientResponceModel
            {
                Patients = await patients
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(),

                Pagination = new PageInfoModel
                {
                    size = pageSize,
                    count = (int)Math.Ceiling(totPat / pgSize),
                    current = pageNumber
                }
            };

            return Ok(response);
        }

        [ProducesResponseType<Guid>(200)]
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
                return Ok(patient.id);
            }
            return BadRequest();
        }


        [ProducesResponseType<Guid>(200)]
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
                
                int Mains = 0;
                await _context.SaveChangesAsync();
                var diagnoses = new List<Diagnosis>(); 
                foreach (var diagnosis in inspectionDTO.diagnoses) {
                    if (Mains < 1)
                    {
                        if (diagnosis.type == DiagnosisType.Main)
                        {
                            Mains++;
                        }

                        var diagnose = new Diagnosis
                        {
                            id = Guid.NewGuid(),
                            icdDiagnosis = await _context.Icd10s.FirstOrDefaultAsync(m => m.id == diagnosis.icdDiagnosisId),
                            inspection = inspection,
                            type = diagnosis.type,
                            description = diagnosis.description,
                            createTime = DateTime.Now
                        };
                        diagnoses.Add(diagnose);
                    }
                    else
                    {
                        return BadRequest("There can be only one main diagnosis");
                    }
                }
                if (Mains == 0)
                {
                    return BadRequest("You haven't entered main diagnosis");
                }
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
                    consultations.Add(consult);
                    comments.Add(comment);
                }
                await _context.AddRangeAsync(consultations);
                await _context.AddRangeAsync(comments);
                await _context.AddRangeAsync(diagnoses);
                _context.Add(inspection);
                await _context.SaveChangesAsync();


                return Ok(inspection.id);
            }
            return BadRequest();
        }

        [Authorize]
        [ProducesResponseType<InspectionPagedListModel>(200)]
        [HttpGet("{id}/inspections")]
        public async Task<IActionResult> GetInspection(Guid id, [FromQuery] List<Guid>? icdRoots = null, bool grouped = false, int page = 1, int size = 5)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.id == id);
            if (patient == null)
            {
                return NotFound();
            }

  
            var diagnoses = _context.Diagnoses
                .Include(p => p.icdDiagnosis)
                .ThenInclude(p => p.parent) 
                .Include(p => p.inspection)
                .ThenInclude(p => p.patient) 
                .Where(p => p.inspection.patient.id == id && p.type == DiagnosisType.Main);


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
                .Include(d => d.previousInspection) 
                .Where(d => d.patient.id == id).ToList();


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

        [ProducesResponseType<List<InspectionShortModel>>(200)]
        [Authorize]
        [HttpGet("{id}/inspections/search")]
        public async Task<IActionResult> SearchInspection(Guid id, string request = "")
        {
            var diagnoses = _context.Diagnoses.Where(p => p.inspection.patient.id == id && (p.icdDiagnosis.code == request | p.icdDiagnosis.name.ToLower().Contains(request.ToLower())))
                    .Select(p => new DiagnosisModel
                    {
                        id = p.id,
                        createTime = p.createTime,
                        code = p.icdDiagnosis.code,
                        name = p.icdDiagnosis.name,
                        description = p.description,
                        type = p.type

                    }).ToList();

            var response = _context.Inspections
                .Where(d => d.patient.id == id)
                .Select(d => new InspectionShortModel
                {
                    id = d.id,
                    createTime = d.createTime,
                    date = d.date,
                    diagnoses = diagnoses
                }).ToList();


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
                name= diagnosis.icdDiagnosis.name,
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
