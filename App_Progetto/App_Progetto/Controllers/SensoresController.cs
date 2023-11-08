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
    public class SensoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SensoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sensores
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sensores.Include(s => s.Terreno);
            return View(await applicationDbContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["TerrenoId"] = new SelectList(_context.Terrenos, "Id", "Id");
            return View();
        }

        // POST: Sensores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StatoSensore,TipoSensore,TerrenoId")] Sensore sensore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sensore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TerrenoId"] = new SelectList(_context.Terrenos, "Id", "Id", sensore.TerrenoId);
            return View(sensore);
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
            ViewData["TerrenoId"] = new SelectList(_context.Terrenos, "Id", "Id", sensore.TerrenoId);
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sensore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SensoreExists(sensore.Id))
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
            ViewData["TerrenoId"] = new SelectList(_context.Terrenos, "Id", "Id", sensore.TerrenoId);
            return View(sensore);
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
