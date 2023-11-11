using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Progetto.Data;
using App_Progetto.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace App_Progetto.Controllers
{
    public class TerrenoesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UserController> _logger;


        public TerrenoesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext,
        ILogger<UserController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        // GET: Terrenoes
        public async Task<IActionResult> HomeTerreno()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var query = from terreno in _dbContext.Terrenos
                        join gestione in _dbContext.Gestiones on terreno.Id equals gestione.TerrenoId
                        where gestione.UserId == currentUser.Id
                        select new
                        {
                            TerrenoId = terreno.Id,
                            Foglio = terreno.Foglio,
                            Mappale = terreno.Mappale,
                            Ettari = terreno.Ettari,
                            Citta = terreno.CittaTerreno,
                            TipoColtura = terreno.TipoColtura,
                            TipoTerreno = terreno.TipoTerreno,
                            Ruolo = gestione.Ruolo
                            // Aggiungi altre proprietà se necessario
                        };

            var result = await query.ToListAsync();


            return View(result);
        }

        public async Task<IActionResult> VisualizzaTerreno(int TerrenoId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var terr = _dbContext.Terrenos.FirstOrDefault(t => t.Id == TerrenoId);
            List<String> NameCollab = new List<string>();

            if (terr == null)
                return NotFound();

            var Collaboratori = _dbContext.Gestiones
                .Where(g => g.TerrenoId == TerrenoId && g.Ruolo == "Collaboratore")
                .ToList();

            foreach (var collab in Collaboratori)
            {
                var user = await _userManager.FindByIdAsync(collab.UserId);
                NameCollab.Add(user.UserName);
            }
            //Console.WriteLine("ifuiru " + string.Join(", ", NameCollab));

            var gestione = _dbContext.Gestiones
                .FirstOrDefault(g => g.TerrenoId == terr.Id && g.UserId == currentUser.Id);

            string ruolo = gestione?.Ruolo ?? "Nessun ruolo associato";
            Console.WriteLine("********* " + ruolo);

            var viewModel = new
            {
                TerrenoId = terr.Id,
                Mappale = terr.Mappale,
                Foglio = terr.Foglio,
                Ettari = terr.Ettari,
                Citta = terr.CittaTerreno,
                TipoColtura = terr.TipoColtura,
                TipoTerreno = terr.TipoTerreno,
                Collaboratore = NameCollab,
                Ruolo = ruolo
            };

            return View(viewModel);
        }


        // GET: Terrenoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Terrenoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Mappale,Foglio,Ettari,CittaTerreno,TipoColtura,TipoTerreno")] Terreno terreno)
        {

                Console.WriteLine("Sto aggiungendo NUOVO Terreno");
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                Console.WriteLine($"UserId: {userId}");

                _dbContext.Add(terreno);
                await _dbContext.SaveChangesAsync();

                //CreateGestione();
            
                var datiGestione = new Gestione
                {
                    UserId = userId,
                    Ruolo = "Agricoltore",
                    TerrenoId = terreno.Id
                };
                
                _dbContext.Add(datiGestione);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(HomeTerreno));
               
        }

        //POST: per aggiungere nella Tabella Gestione, una nuova tupla,  
        public async Task<IActionResult> AggiungiCollaboratore(int terrenoId, string username)
        {
            Console.WriteLine("****Codice del Terreno ***** " + terrenoId);

            // Esempio di verifica (considera di implementare una logica più robusta):
            var user = _userManager.FindByNameAsync(username).Result;
            if (user != null)
            {
                var datiGestione = new Gestione
                {
                    UserId = user.Id,
                    Ruolo = "Collaboratore",
                    TerrenoId = terrenoId
                };

                _dbContext.Add(datiGestione);
                await _dbContext.SaveChangesAsync();
                TempData["Messaggio"] = "Collaboratore aggiunto con successo!";
                TempData["MessaggioTipo"] = "success";
            }
            else
            {
                TempData["Messaggio"] = "Username non trovato.";
                TempData["MessaggioTipo"] = "danger";
            }

            // Redirect alla vista originale
            return RedirectToAction("VisualizzaTerreno", new { TerrenoId = terrenoId });
        }



        //_________________________________________________________________


        // GET: Terrenoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _dbContext.Terrenos == null)
            {
                return NotFound();
            }

            var terreno = await _dbContext.Terrenos.FindAsync(id);
            if (terreno == null)
            {
                return NotFound();
            }
            return View(terreno);
        }

        // POST: Terrenoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Mappale,Foglio,Ettari,CittaTerreno,TipoColtura,TipoTerreno")] Terreno terreno)
        {
            if (id != terreno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(terreno);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerrenoExists(terreno.Id))
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
            return View(terreno);
        }

        // GET: Terrenoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _dbContext.Terrenos == null)
            {
                return NotFound();
            }

            var terreno = await _dbContext.Terrenos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (terreno == null)
            {
                return NotFound();
            }

            return View(terreno);
        }

        // POST: Terrenoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_dbContext.Terrenos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Terrenos'  is null.");
            }
            var terreno = await _dbContext.Terrenos.FindAsync(id);
            if (terreno != null)
            {
                _dbContext.Terrenos.Remove(terreno);
            }
            
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(HomeTerreno));
        }

        private bool TerrenoExists(int id)
        {
          return (_dbContext.Terrenos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
