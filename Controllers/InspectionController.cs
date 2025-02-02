﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using scrubsAPI;
using scrubsAPI.Models;
using scrubsAPI.Schemas;

namespace scrubsAPI
{

    [ApiController]
    [Route("api/[controller]")]
    public class InspectionController : Controller
    {
        private readonly ScrubsDbContext _context;

        public InspectionController(ScrubsDbContext context)
        {
            _context = context;
        }

        [ProducesResponseType<InspectionModel>(200)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInspection(Guid id)
        {
            var inspec = await _context.Inspections
                .Include(i => i.doctor)
                .Include(i => i.patient)
                .Include(i => i.previousInspection)
                .FirstOrDefaultAsync(p => p.id == id);
            if (inspec == null)
            {
                return NotFound();
            }
            var doc = new DoctorModel
            {
                id = inspec.doctor.id,
                name = inspec.doctor.name,
                creteTime = inspec.doctor.createTime,
                birthsday = inspec.doctor.birthday,
                email = inspec.doctor.email,
                phone = inspec.doctor.phone,
                gender = inspec.doctor.gender

            };
            var patient = new PatientModel
            {
                id = inspec.patient.id,
                createTime = inspec.patient.creationTime,
                name = inspec.patient.name,
                birthday = inspec.patient.birthDay,
                gender = inspec.patient.gender

            };
            var diagnoses = _context.Diagnoses
                .Include(i => i.icdDiagnosis)
                .Include(i => i.inspection)
                .Where(p => p.inspection.id == inspec.id)
                    .Select(p => new DiagnosisModel
                    {
                        id = p.id,
                        createTime = p.createTime,
                        code = p.icdDiagnosis.code,
                        name = p.icdDiagnosis.name,
                        description = p.description,
                        type = p.type

                    }).ToList();
            var baseInspec = await _context.Inspections
                .Include(i => i.doctor)
                .Include(i => i.patient)
                .Include(i => i.previousInspection)
                .FirstOrDefaultAsync(p => p.patient.id == inspec.patient.id && p.previousInspection == null && p.id != id);

            var consultations = _context.Consultations
                .Include(i => i.speciality)
                .Include(i => i.inspection)
                .Where(p => p.inspection.id == id)
                .Select(p => new InspectionConsultationModel
                {
                    id = p.id,
                    createTime = p.createTime,
                    inspectionId = p.inspection.id,
                    speciality = new SpecialityModel
                    {
                        id = p.speciality.id,
                        createTime = p.speciality.creationTime,
                        name = p.speciality.name,
                    },
                    rootComment =  _context.Comments
                        .Include(i => i.author)
                        .Include(i => i.parentComment)
                        .Include(i => i.author.speciality)
                        .Include(i => i.consultation)
                        .Where(d => d.consultation.id == p.id && d.parentComment == null).Select(p => new InspectionCommentModel
                        {
                            id = p.id,
                            author = new DoctorModel
                            {
                                id = p.author.id,
                                name = p.author.name,
                                creteTime = p.author.createTime,
                                birthsday = p.author.birthday,
                                email = p.author.email,
                                phone = p.author.phone,
                                gender = p.author.gender

                            },
                            modifyTime = p.modifiedTime,
                            content = p.content,
                            createTime = p.createTime,
                            parentId = null,

                        }).FirstOrDefault(g => true),
                    commentNumber = _context.Comments
                    .Include(i => i.author)
                    .Include(i => i.parentComment)
                    .Include(i => i.author.speciality)
                    .Include(i => i.consultation)
                    .Include(i => i.consultation.inspection)
                    .Where(k => k.consultation.id == p.id).Count(),
                }).ToList();

            var ins = new InspectionModel
            {
                id = inspec.id,
                createTime = inspec.createTime,
                previousInspectionId = inspec.previousInspection?.id,
                date = inspec.date,
                conclusion = inspec.conclusion,
                nextVisitDate = inspec.nextVisitDate,
                deathTime = inspec.deathTime,
                baseInspectionId = baseInspec?.id,
                doctor = doc,
                patient = patient,
                diagnoses = diagnoses,
                consultations = consultations,
                anamesis = inspec.anamesis,
                complaints = inspec.complaints,
                treatment = inspec.treatment
            };


            return Ok(ins);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditInspection(Guid id, [FromBody] InspectionEditModel inspectionEdition)
        {
            if (ModelState.IsValid)
            {
                var inspection = await _context.Inspections
                    .FirstOrDefaultAsync(i => i.id == id);

                if (inspection == null)
                {
                    return BadRequest("there are no such inpection");
                }

                if (inspectionEdition.conclusion.ToString() == "Recovery")
                {
                    if (inspectionEdition.deathTime.HasValue)
                    {
                        return BadRequest("Patient has recoveried, he is not dead");
                    }
                    inspection.nextVisitDate = null;
                    inspection.deathTime = null;
                }
                else if (inspectionEdition.conclusion.ToString() == "Death")
                {
                    if (inspectionEdition.nextVisitDate.HasValue || !inspectionEdition.deathTime.HasValue)
                    {
                        return BadRequest("Patient is dead, he cant have next visit or dont have death time");
                    }
                    if (_context.Inspections.Include(p => p.previousInspection).Any(p => p.previousInspection.id == id))
                    {
                        return BadRequest("Patient with existing visits after that cant be dead");
                    }
                    if (inspectionEdition.deathTime > DateTime.Now)
                    {
                        return BadRequest("Patient cant be dead in future");
                    }
                    inspection.deathTime = inspectionEdition.deathTime;
                }
                else
                {
                    if (!inspectionEdition.nextVisitDate.HasValue || inspectionEdition.deathTime.HasValue)
                    {
                        return BadRequest("Patient is ill, he cant be dead or dont have next visit date");
                    }
                    inspection.nextVisitDate = inspectionEdition.nextVisitDate;
                }

                inspection.anamesis = inspectionEdition.anamesis;
                inspection.complaints = inspectionEdition.complaints;
                inspection.conclusion = inspectionEdition.conclusion;
                inspection.treatment = inspectionEdition.treatment;

                var diagnoses = await _context.Diagnoses
                   .Where(p => p.inspection.id == inspection.id)
                   .ToListAsync();


                var diagnoses2 = new List<Diagnosis>();
                var Mains = 0;
                foreach (var diagnosis in inspectionEdition.diagnoses)
                {
                    if (Mains <= 1)
                    {
                        if (diagnosis.type == DiagnosisType.Main)
                        {
                            Mains++;
                            if (Mains == 2)
                            {
                                return BadRequest("There can be only one main diagnosis");
                            }
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
                        diagnoses2.Add(diagnose);
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

                foreach (var diagnosis in diagnoses)
                {
                    _context.Diagnoses.Remove(diagnosis);
                    await _context.SaveChangesAsync();

                }
                await _context.AddRangeAsync(diagnoses2);
                await _context.SaveChangesAsync();
                _context.Update(inspection);

                return Ok();
            }
            else 
            {
                return BadRequest();
            }
        }

        [ProducesResponseType<List<InspectionPreviewModel>>(200)]
        [Authorize]
        [HttpGet("{id}/chain")]
        public async Task<IActionResult> GetInspectionChain(Guid id)
        {
            var inspection = await _context.Inspections
                            .FirstOrDefaultAsync(i => i.id == id);

            if (inspection == null)
            {
                return NotFound();
            }

            var answer = await GetInspectionChainRec(id);
            return Ok(answer);
        }


        private async Task<List<InspectionPreviewModel>> GetInspectionChainRec(Guid id)
        {
            var result = new List<InspectionPreviewModel>();


            var ins = await _context.Inspections
                            .Include(i => i.doctor)
                            .Include(i => i.patient)
                            .Include(i => i.previousInspection)
                            .FirstOrDefaultAsync(p => p.id == id);


            if (ins == null)
            {
                return result;
            }


            var insp = new InspectionPreviewModel
            {
                id = ins.id,
                createTime = ins.createTime,
                previousId = ins.previousInspection?.id,
                date = ins.date,
                conclusion = ins.conclusion,
                doctorId = ins.doctor.id,
                doctor = ins.doctor.name,
                patientId = ins.patient.id,
                patient = ins.patient.name,
                hasChain = ins.nextVisitDate != null,
                hasNested = ins.previousInspection != null,
                diagnosis = toDiagnosisModel(_context.Diagnoses
                .Include(p => p.icdDiagnosis)
                .FirstOrDefault(p => p.inspection.id == ins.id && p.type == DiagnosisType.Main)),
            };


            result.Add(insp);


            if (ins.nextVisitDate != null)
            {
                var nextInsp = await _context.Inspections
                    .Include(i => i.doctor)
                    .Include(i => i.patient)
                    .Include(i => i.previousInspection)
                    .FirstOrDefaultAsync(p => p.previousInspection.id == ins.id);


                if (nextInsp != null)
                {
                    var subsequentResults = await GetInspectionChainRec(nextInsp.id);
                    result.AddRange(subsequentResults);
                }
            }

            return result;

        }

        private DiagnosisModel toDiagnosisModel(Diagnosis diagnosis)
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

    }

}