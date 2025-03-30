using BokningSystem.Models;
using BokningSystem.Services;
using BokningSystem.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BokningSystem.Controllers
{
    public class BokningController : Controller
    {
        private readonly BokningService _bokningService;
        private readonly AppDbContext _context;

        public BokningController(BokningService bokningService, AppDbContext context)
        {
            _bokningService = bokningService;
            _context = context;
        }

        public IActionResult Login(string roll)
        {
            ViewBag.Roll = roll;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Enkel hårdkodad kontroll – byt ut med databas senare
            if (username == "patient" && password == "1234")
            {
              
                return RedirectToAction("Create", "Bokning");
            }
            
            if (username == "Admin" && password == "1234")
            {
              
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.Error = "Fel användarnamn eller lösenord.";
            return View();
        }


        // 🔹 Visar alla bokningar i tabell
        public async Task<IActionResult> Index()
        {
            var bokningar = await _bokningService.GetAllBokningarAsync();

            // 🔹 Rensa bokningar vars tid har passerat
            var gamlaBokningar = bokningar
                .Where(b => b.Tid < DateTime.Now) // jämför med nuvarande tid
                .ToList();

            foreach (var bokning in gamlaBokningar)
            {
                await _bokningService.DeleteBokningAsync(bokning.Id);
            }

            var aktuellaBokningar = await _bokningService.GetAllBokningarAsync();
            return View(aktuellaBokningar);
        }


        // 🔹 Visar bokningsformuläret
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.StaffCategories = await _bokningService.GetStaffCategoriesAsync();
            ViewBag.LedigaTider = await _bokningService.GetLedigaTiderAsync();

            // 🔹 Hämta bokade tider från API:t
            var bokadeTider = await _bokningService.GetAllBokningarAsync();
            ViewBag.BokadeTider = bokadeTider;

            return View(new Bokning());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Bokning bokning)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StaffCategories = await _bokningService.GetStaffCategoriesAsync();
                ViewBag.LedigaTider = await _bokningService.GetLedigaTiderAsync();
                ViewBag.BokadeTider = await _bokningService.GetAllBokningarAsync();

                return View(bokning);
            }

            var success = await _bokningService.AddBokningAsync(bokning);
            if (success)
            {
                // Lägg till bekräftelsemeddelande 
                ViewBag.BokningsStatus = "Din bokning har registrerats!";

                ViewBag.StaffCategories = await _bokningService.GetStaffCategoriesAsync();
                ViewBag.LedigaTider = await _bokningService.GetLedigaTiderAsync();
                ViewBag.BokadeTider = await _bokningService.GetAllBokningarAsync();

                return View(new Bokning()); 
            }

            ModelState.AddModelError("", "Kunde inte skapa bokning. Försök igen.");

            ViewBag.StaffCategories = await _bokningService.GetStaffCategoriesAsync();
            ViewBag.LedigaTider = await _bokningService.GetLedigaTiderAsync();
            ViewBag.BokadeTider = await _bokningService.GetAllBokningarAsync();

            return View(bokning);
        }

        
        [HttpGet]
        public async Task<JsonResult> GetPatientInfo(string personnummer)
        {
            var patient = await _bokningService.GetPatientInfoAsync(personnummer);
            if (patient == null)
                return Json(null);

            return Json(new { name = $"{patient.Fornamn} {patient.Efternamn}", id = patient.Id });
        }


        // Hämtar personal 
        [HttpGet]
        public async Task<JsonResult> GetStaffMembers(string category)
        {
            var staff = await _bokningService.GetStaffMembersAsync(category);
            return Json(staff);
        }

        
        public async Task<IActionResult> CreateByPersonnummer(string personnummer)
        {
            var patient = await _bokningService.GetPatientInfoAsync(personnummer);

            if (patient == null)
            {
                ModelState.AddModelError("", "Patienten kunde inte hittas.");
                ViewBag.StaffCategories = await _bokningService.GetStaffCategoriesAsync();
                ViewBag.LedigaTider = _context.LedigaTider.ToList();
                return View("Create", new Bokning());
            }

            var bokning = new Bokning
            {
                PatientId = patient.Personnummer,
                PatientNamn = $"{patient.Fornamn} {patient.Efternamn}"
            };

            ViewBag.StaffCategories = await _bokningService.GetStaffCategoriesAsync();
            ViewBag.LedigaTider = _context.LedigaTider.ToList();

            return View("Create", bokning);
        }

    }
}
