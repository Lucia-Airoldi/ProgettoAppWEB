using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Progetto.Data;
using App_Progetto.Models;
using Microsoft.AspNetCore.Authorization;

namespace App_Progetto.Controllers
{
    [Authorize(Roles = "Agricoltore,Collaboratore")]
    public class PianoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PianoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pianoes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Pianos.Include(p => p.Attuatores);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Pianoes/Details/5
        public async Task<IActionResult> Details(int? CodAttuatore)
        {
            if (CodAttuatore == null)
            {
                return BadRequest(); // o un altro codice di stato appropriato
            }
            var piano = await _context.Pianos
                .Include(p => p.Attuatores)
                .FirstOrDefaultAsync(p => p.CodAtt == CodAttuatore);

            if(piano == null)
            {
                return RedirectToAction("Create", "Pianoes", new { CodAtt = CodAttuatore});
            }

            var terrenoId = await _context.Attuatores
                .Where(a => a.Id == piano.CodAtt)
                .Select(a => a.TerrenoId)
                .FirstOrDefaultAsync();

            ViewData["CodTerr"] = terrenoId;

            return View(piano);
        }

        // GET: Pianoes/Create
        public IActionResult Create(int CodAtt)
        {
            ViewData["CodAtt"] = CodAtt;

            return View();
        }

        // POST: Pianoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodicePiano,OrarioAttivazione,OrarioDisattivazione,OrarioAttDefault,OrarioDisattDefault,CondAttivazione,CondDisattivazione,CodAtt")] Piano piano)
        {
            _context.Add(piano);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Pianoes", new { CodAttuatore = piano.CodAtt });

        }

        // GET: Pianoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pianos == null)
            {
                return NotFound();
            }

            var piano = await _context.Pianos.FindAsync(id);
            if (piano == null)
            {
                return NotFound();
            }
            
            ViewData["CodAtt"] = piano.CodAtt;

            return View(piano);
        }

        // POST: Pianoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodicePiano,OrarioAttivazione,OrarioDisattivazione,OrarioAttDefault,OrarioDisattDefault,CondAttivazione,CondDisattivazione,CodAtt")] Piano piano)
        {
            Console.WriteLine("Ciao: " + piano.CodAtt);
            _context.Update(piano);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Pianoes", new { CodAttuatore = piano.CodAtt });

        }

        // GET: Pianoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pianos == null)
            {
                return NotFound();
            }

            var piano = await _context.Pianos
                .Include(p => p.Attuatores)
                .FirstOrDefaultAsync(m => m.CodicePiano == id);
            if (piano == null)
            {
                return NotFound();
            }

            return View(piano);
        }

        // POST: Pianoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pianos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Pianos'  is null.");
            }
            var piano = await _context.Pianos.FindAsync(id);
            if (piano != null)
            {
                _context.Pianos.Remove(piano);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PianoExists(int id)
        {
          return (_context.Pianos?.Any(e => e.CodicePiano == id)).GetValueOrDefault();
        }
    }
}
