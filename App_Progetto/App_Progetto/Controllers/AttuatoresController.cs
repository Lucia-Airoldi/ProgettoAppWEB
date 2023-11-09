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

namespace App_Progetto.Controllers
{
    public class AttuatoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserController> _logger;


        public AttuatoresController(ApplicationDbContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> AttDettaglio(int TerrenoId)
        {
            var query = from a in _context.Attuatores
                        join t in _context.Terrenos on a.TerrenoId equals t.Id
                        where a.TerrenoId == TerrenoId
                        select new
                        {
                            TerrenoId = a.TerrenoId,
                            Foglio = t.Foglio,
                            Mappale = t.Mappale,
                            TipoAttuatore = a.TipoAttuatore,
                            Standby = a.Standby,
                            Attivazione = a.Attivazione
                        };

            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                Console.WriteLine($"TerrenoId: {item.TerrenoId}, Foglio: {item.Foglio}, Mappale: {item.Mappale}, TipoAttuatore: {item.TipoAttuatore}, Standby: {item.Standby}, Attivazione: {item.Attivazione}");
            }
            return View(result);
        }

        // GET: Attuatores
        /*public async Task<IActionResult> Index(int? id)
        {
            var applicationDbContext = _context.Attuatores.Include(a => a.Terreno);
            return View(await applicationDbContext.ToListAsync());
        }*/

        public async Task<IActionResult> Index()
        {
            // Ottenere l'ID dell'utente corrente
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



            var attuatoriUtente = _context.Attuatores
                .Include(a => a.Terreno)  // Includi il riferimento al Terreno
                .Where(a => _context.Gestiones.Any(g => g.UserId == userId && g.TerrenoId == a.TerrenoId))
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
            ViewBag.TerrenoId = TerrenoId;

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
            return RedirectToAction("AttDettaglio", new { TerrenoId = attuatore.TerrenoId });
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
            ViewData["TerrenoId"] = new SelectList(_context.Terrenos, "Id", "Id", attuatore.TerrenoId);
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
