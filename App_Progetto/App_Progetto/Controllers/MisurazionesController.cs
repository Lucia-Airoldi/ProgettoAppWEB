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
    public class MisurazionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MisurazionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Misuraziones
        public async Task<IActionResult> Index(int CodSensore)
        {
            var misurazioniSensore = _context.Misuraziones
                .Include(m => m.Sensores)
                .Where(m => m.Sensores.Id == CodSensore)
                .ToList();
            ViewData["codSensore"] = CodSensore;

            return View(misurazioniSensore);
        }

        // GET: Misuraziones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Misuraziones == null)
            {
                return NotFound();
            }

            var misurazione = await _context.Misuraziones
                .Include(m => m.Sensores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (misurazione == null)
            {
                return NotFound();
            }

            return View(misurazione);
        }

        // GET: Misuraziones/Create
        public IActionResult Create(int CodSensore)
        {
            var tipoSensore = _context.Sensores
            .Where(s => s.Id == CodSensore)
            .Select(s => s.TipoSensore)
            .FirstOrDefault();

            //Console.WriteLine($"TipoSensore: {tipoSensore}");
            Console.WriteLine($"CodiceSensore nel metodo Create: {CodSensore}");

            ViewBag.CodiceSensore = CodSensore;
            ViewData["CodSensore"] = CodSensore;
            ViewData["TipoSensore"] = tipoSensore; 

            return View();
        }

        // POST: Misuraziones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataOra,CodiceSensore,Valore,TipoMisurazione")] Misurazione misurazione)
        {
            Console.WriteLine("DataOra: " + misurazione.DataOra);
            Console.WriteLine("CodiceSensore: " + misurazione.CodiceSensore);
            Console.WriteLine("Valore: " + misurazione.Valore);
            Console.WriteLine("TipoMisurazione: " + misurazione.TipoMisurazione);
 
                ViewData["CodiceSensore"] = new SelectList(_context.Sensores, "Id", "Id", misurazione.CodiceSensore);
                Console.WriteLine("**********-->");
                _context.Add(misurazione);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                

            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
            }
            return RedirectToAction("Index", new { CodSensore = misurazione.CodiceSensore });
            //return View(misurazione);
        }

        // GET: Misuraziones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Misuraziones == null)
            {
                return NotFound();
            }

            var misurazione = await _context.Misuraziones
                .Include(m => m.Sensores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (misurazione == null)
            {
                return NotFound();
            }

            return View(misurazione);
        }

        // POST: Misuraziones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Misuraziones == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Misuraziones'  is null.");
            }
            var misurazione = await _context.Misuraziones.FindAsync(id);
            if (misurazione != null)
            {
                _context.Misuraziones.Remove(misurazione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { CodSensore = misurazione.CodiceSensore });
        }


        // GET: Misuraziones/Edit/5
        public async Task<IActionResult> Edit(DateTime? id)
        {
            if (id == null || _context.Misuraziones == null)
            {
                return NotFound();
            }

            var misurazione = await _context.Misuraziones.FindAsync(id);
            if (misurazione == null)
            {
                return NotFound();
            }
            ViewData["CodiceSensore"] = new SelectList(_context.Sensores, "Id", "Id", misurazione.CodiceSensore);
            return View(misurazione);
        }

        // POST: Misuraziones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime id, [Bind("DataOra,CodiceSensore,Valore,TipoMisurazione")] Misurazione misurazione)
        {
            if (id != misurazione.DataOra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(misurazione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MisurazioneExists(misurazione.DataOra))
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
            ViewData["CodiceSensore"] = new SelectList(_context.Sensores, "Id", "Id", misurazione.CodiceSensore);
            return View(misurazione);
        }

        private bool MisurazioneExists(DateTime id)
        {
          return (_context.Misuraziones?.Any(e => e.DataOra == id)).GetValueOrDefault();
        }
    }
}
