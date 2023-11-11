using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Progetto.Data;
using App_Progetto.Models;
using Humanizer;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace App_Progetto.Controllers
{
    public class SensoresController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Controller> _logger;

        public SensoresController(UserManager<IdentityUser> userManager, ApplicationDbContext context, ILogger<Controller> logger)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        //Creo il task per la visualizzazione dei sensori
        public async Task<IActionResult> SensoriDettaglio(int TerrenoId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var gestione = _context.Gestiones
                .FirstOrDefault(g => g.TerrenoId == TerrenoId && g.UserId == currentUser.Id);

            string ruolo = gestione?.Ruolo ?? "Nessun ruolo associato";
            Console.WriteLine("********* " + ruolo);

            var query = from a in _context.Sensores
                        join t in _context.Terrenos on a.TerrenoId equals t.Id
                        where a.TerrenoId == TerrenoId
                        select new
                        {
                            TerrenoId = a.TerrenoId,
                            Foglio = t.Foglio,
                            Mappale = t.Mappale,
                            Id = a.Id,
                            StatoSensore = a.StatoSensore,
                            TipoSensore = a.TipoSensore, 
                            Ruolo = ruolo
                        };

            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                Console.WriteLine($"TerrenoId: {item.TerrenoId}, Foglio: {item.Foglio}, Mappale: {item.Mappale}, SensoreId: {item.Id}, Stato Sensore: {item.StatoSensore}, Tipo Sensore: {item.TipoSensore}");
            }
            return View(result);
        }



        // GET: Sensores
        /*public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sensores.Include(s => s.Terreno);
            return View(await applicationDbContext.ToListAsync());
        }*/

        public async Task<IActionResult> Index()
        {
            // Ottenere l'ID dell'utente corrente
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var sensoriUtente = _context.Sensores
                .Include(a => a.Terreno)  // Includi il riferimento al Terreno
                .ThenInclude(t => t.Gestiones)  // Assicurati di includere le Gestiones
                .Where(a => a.Terreno.Gestiones.Any(g => g.UserId == userId))
                .ToList();

            return View(sensoriUtente);
        }


        // GET: Sensores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sensores == null)
            {
                return NotFound();
            }

            var sensore = await _context.Sensores
                .Include(s => s.Terreno)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sensore == null)
            {
                return NotFound();
            }

            return View(sensore);
        }

        // GET: Sensores/Create
        public IActionResult Create(int TerrenoId)
        {
            var terreno = _context.Terrenos.Find(TerrenoId);
            
            ViewBag.TerrenoId = TerrenoId;
            ViewData["Mappale"] = terreno?.Mappale;
            ViewData["Foglio"] = terreno?.Foglio;

            return View();
        }

        // POST: Sensores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StatoSensore,TipoSensore,TerrenoId")] Sensore sensore)
        {
            _context.Add(sensore);
            await _context.SaveChangesAsync();
            return RedirectToAction("SensoriDettaglio", new { TerrenoId = sensore.TerrenoId });
        }

        // GET: Sensores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sensores == null)
            {
                return NotFound();
            }

            var sensore = await _context.Sensores.FindAsync(id);
            if (sensore == null)
            {
                return NotFound();
            }

            var terreno = await _context.Terrenos.FindAsync(sensore.TerrenoId);

            // Passa TerrenoId e i dati del Terreno alla vista
            ViewData["TerrenoId"] = sensore.TerrenoId;
            ViewData["Mappale"] = terreno?.Mappale;
            ViewData["Foglio"] = terreno?.Foglio;

            return View(sensore);
        }

        // POST: Sensores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StatoSensore,TipoSensore,TerrenoId")] Sensore sensore)
        {
            if (id != sensore.Id)
            {
                return NotFound();
            }
            _context.Update(sensore);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Sensores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sensores == null)
            {
                return NotFound();
            }

            var sensore = await _context.Sensores
                .Include(s => s.Terreno)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sensore == null)
            {
                return NotFound();
            }

            return View(sensore);
        }

        // POST: Sensores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sensores == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sensores'  is null.");
            }
            var sensore = await _context.Sensores.FindAsync(id);
            if (sensore != null)
            {
                _context.Sensores.Remove(sensore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SensoreExists(int id)
        {
          return (_context.Sensores?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
