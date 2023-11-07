using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Progetto.Data;
using App_Progetto.Models;

namespace App_Progetto.Controllers
{
    public class AttuatoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttuatoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AttDettaglio(int idTerr)
        {
            var applicationDbContext = _context.Attuatores
            .Where(attuatore => attuatore.TerrenoId == idTerr);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Attuatores
        public async Task<IActionResult> Index(int idTerr)
        {
            var applicationDbContext = _context.Attuatores.Include(a => a.Terreno);
            return View(await applicationDbContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["TerrenoId"] = new SelectList(_context.Terrenos, "Id", "Id");
            return View();
        }

        // POST: Attuatores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoAttuatore,Standby,Attivazione,TerrenoId")] Attuatore attuatore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attuatore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TerrenoId"] = new SelectList(_context.Terrenos, "Id", "Id", attuatore.TerrenoId);
            return View(attuatore);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TipoAttuatore,Standby,Attivazione,TerrenoId")] Attuatore attuatore)
        {
            if (id != attuatore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attuatore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttuatoreExists(attuatore.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TerrenoId"] = new SelectList(_context.Terrenos, "Id", "Id", attuatore.TerrenoId);
            return View(attuatore);
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
