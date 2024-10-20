using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using scrubsAPI;

namespace scrubsAPI
{
    public class PatientsController : Controller
    {
        private readonly ScrubsDbContext _context;

        public PatientsController(ScrubsDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patients.ToListAsync());
        }

        // GET: Patients/Details/5
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

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientCreateModel patientDTO)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient();
                patient.id = Guid.NewGuid();
                if (patientDTO.birthday != null) 
                {
                    patient.birthDay = patientDTO.birthday;
                }
                patient.gender = patientDTO.gender;
                patient.creationTime = DateTime.Now;
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patientDTO);
        }

        private bool PatientExists(Guid id)
        {
            return _context.Patients.Any(e => e.id == id);
        }
    }
}
