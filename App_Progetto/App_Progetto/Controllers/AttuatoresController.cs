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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace App_Progetto.Controllers
{
    public class AttuatoresController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Controller> _logger;


        public AttuatoresController(UserManager<IdentityUser> userManager, ApplicationDbContext context, ILogger<Controller> logger)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        [Authorize(Roles = "Agricoltore,Collaboratore")]
        public async Task<IActionResult> AttDettaglio(int TerrenoId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var gestione = _context.Gestiones
                .FirstOrDefault(g => g.TerrenoId == TerrenoId && g.UserId == currentUser.Id);

            string ruolo = gestione?.Ruolo ?? "Nessun ruolo associato";
            
            var query = from a in _context.Attuatores
                        join t in _context.Terrenos on a.TerrenoId equals t.Id
                        where a.TerrenoId == TerrenoId
                        select new
                        {
                            TerrenoId = a.TerrenoId,
                            Mappale = t.Mappale,
                            Foglio = t.Foglio,
                            Id = a.Id,
                            TipoAttuatore = a.TipoAttuatore,
                            Standby = a.Standby,
                            Attivazione = a.Attivazione,
                            Ruolo = ruolo
                        };
            ViewData["IdTerreno"] = TerrenoId;
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                Console.WriteLine($"TerrenoId: {item.TerrenoId}, Foglio: {item.Foglio}, Mappale: {item.Mappale}, TipoAttuatore: {item.TipoAttuatore}, Standby: {item.Standby}, Attivazione: {item.Attivazione}");
            }
            return View(result);
        }

        // GET: Attuatores
        
        public async Task<IActionResult> Index()
        {
            // Ottenere l'ID dell'utente corrente
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var attuatoriUtente = _context.Attuatores
                .Include(a => a.Terreno)  // Includi il riferimento al Terreno
                .ThenInclude(t => t.Gestiones)  // Assicurati di includere le Gestiones
                .Where(a => a.Terreno.Gestiones.Any(g => g.UserId == userId))
                .ToList();

            return View(attuatoriUtente);
        }

        // GET: Attuatores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Attuatores == null)
            {
                return NotFound();
            }

            var attuatore = await _context.Attuatores
                .Include(a => a.Terreno)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attuatore == null)
            {
                return NotFound();
            }

            return View(attuatore);
        }

        // GET: Attuatores/Create
        public IActionResult Create(int TerrenoId)
        {
            // Imposta il TerrenoId nella ViewBag (o ovunque tu preferisca passare dati alla vista)
            var terreno = _context.Terrenos.Find(TerrenoId);

            ViewBag.TerrenoId = TerrenoId;
            ViewData["Mappale"] = terreno?.Mappale;
            ViewData["Foglio"] = terreno?.Foglio;

            return View();
        }

        // POST: Attuatores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoAttuatore,Standby,Attivazione,TerrenoId")] Attuatore attuatore)
        {
            _context.Add(attuatore);
            await _context.SaveChangesAsync();
            /*return RedirectToAction(nameof(Index));*/
            //return RedirectToAction("AttDettaglio", new { TerrenoId = attuatore.TerrenoId });
            return RedirectToAction("Create", "Pianoes", new { TerrenoId = attuatore.TerrenoId });
        }

        // GET: Attuatores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Attuatores == null)
            {
                return NotFound();
            }

            var attuatore = await _context.Attuatores.FindAsync(id);
            if (attuatore == null)
            {
                return NotFound();
            }

            var terreno = await _context.Terrenos.FindAsync(attuatore.TerrenoId);

            // Passa TerrenoId e i dati del Terreno alla vista
            ViewData["TerrenoId"] = attuatore.TerrenoId;
            ViewData["Mappale"] = terreno?.Mappale;
            ViewData["Foglio"] = terreno?.Foglio; 
            
            return View(attuatore);
        }

        // POST: Attuatores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Attuatores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TipoAttuatore,Standby,Attivazione,TerrenoId")] Attuatore attuatore)
        {
            if (id != attuatore.Id)
            {
                return NotFound();
            }
            _context.Update(attuatore);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Attuatores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Attuatores == null)
            {
                return NotFound();
            }

            var attuatore = await _context.Attuatores
                .Include(a => a.Terreno)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attuatore == null)
            {
                return NotFound();
            }

            return View(attuatore);
        }

        // POST: Attuatores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Attuatores == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Attuatores'  is null.");
            }
            var attuatore = await _context.Attuatores.FindAsync(id);
            if (attuatore != null)
            {
                _context.Attuatores.Remove(attuatore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttuatoreExists(int id)
        {
          return (_context.Attuatores?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
