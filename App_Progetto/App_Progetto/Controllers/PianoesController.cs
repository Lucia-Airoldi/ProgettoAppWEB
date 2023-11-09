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
        public async Task<IActionResult> Details(int? id)
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

        // GET: Pianoes/Create
        public IActionResult Create()
        {
            ViewData["CodAtt"] = new SelectList(_context.Attuatores, "Id", "Id");
            return View();
        }

        // POST: Pianoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodicePiano,OrarioAttivazione,OrarioDisattivazione,OrarioAttDefault,OrarioDisattDefault,CondAttivazione,CondDisattivazione,CodAtt")] Piano piano)
        {
            if (ModelState.IsValid)
            {
                _context.Add(piano);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodAtt"] = new SelectList(_context.Attuatores, "Id", "Id", piano.CodAtt);
            return View(piano);
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
            ViewData["CodAtt"] = new SelectList(_context.Attuatores, "Id", "Id", piano.CodAtt);
            return View(piano);
        }

        // POST: Pianoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodicePiano,OrarioAttivazione,OrarioDisattivazione,OrarioAttDefault,OrarioDisattDefault,CondAttivazione,CondDisattivazione,CodAtt")] Piano piano)
        {
            if (id != piano.CodicePiano)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(piano);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PianoExists(piano.CodicePiano))
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
            ViewData["CodAtt"] = new SelectList(_context.Attuatores, "Id", "Id", piano.CodAtt);
            return View(piano);
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
