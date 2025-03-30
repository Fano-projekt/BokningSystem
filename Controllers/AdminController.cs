using Microsoft.AspNetCore.Mvc;
using BokningSystem.Models;
using BokningSystem.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BokningSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly BokningService _bokningService;

        public AdminController(BokningService bokningService)
        {
            _bokningService = bokningService;
        }

       
        public async Task<IActionResult> Index()
        {
            var viewModel = new AdminDashboardViewModel
            {
                LedigaTider = await _bokningService.GetLedigaTiderAsync(),
                Bokningar = await _bokningService.GetAllBokningarAsync()


            };

            return View(viewModel);
        }

        //Lägg till en ny ledig tid
        [HttpPost]
        public async Task<IActionResult> Create(string nyLedigTid)
        {
            if (string.IsNullOrEmpty(nyLedigTid))
            {
                ModelState.AddModelError("", "Datum och tid är obligatoriskt.");
                return RedirectToAction("Index");
            }

            var ledigTid = new LedigaTider
            {
                Tid = DateTime.Parse(nyLedigTid)
            };

            bool success = await _bokningService.AddLedigTidAsync(ledigTid);
            if (!success)
            {
                ModelState.AddModelError("", "Kunde inte lägga till tid.");
            }

            return RedirectToAction("Index");
        }

        //Ta bort en ledig tid
        public async Task<IActionResult> Delete(int id)
        {
            bool success = await _bokningService.DeleteLedigTidAsync(id);
            if (!success)
            {
                ModelState.AddModelError("", "Kunde inte ta bort tiden.");
            }

            return RedirectToAction("Index");
        }

        //Redigera en ledig tid (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var tid = await _bokningService.GetLedigTidByIdAsync(id);
            if (tid == null)
            {
                return NotFound();
            }
            return View(tid);
        }

        //Spara ändrad ledig tid (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(LedigaTider tid)
        {
            if (!ModelState.IsValid)
            {
                return View(tid);
            }

            bool success = await _bokningService.UpdateLedigTidAsync(tid);
            if (!success)
            {
                ModelState.AddModelError("", "Kunde inte uppdatera tiden.");
                return View(tid);
            }

            return RedirectToAction("Index");
        }
    }
}
