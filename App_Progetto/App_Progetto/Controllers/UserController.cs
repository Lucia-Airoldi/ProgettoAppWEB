using App_Progetto.ModelView;
using App_Progetto.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App_Progetto.Data;

namespace App_Progetto.Controllers;

public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UserController> _logger;

    public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext,
        ILogger<UserController> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IActionResult> HomeTerreno()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        
        var query = from terreno in _dbContext.Terrenos
                    join gestione in _dbContext.Gestiones on terreno.Id equals gestione.TerrenoId
                            where gestione.UserId == currentUser.Id
                            select new {
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
        var terr = _dbContext.Terrenos.FirstOrDefault(t => t.Id == TerrenoId);
        List<String> NameCollab = new List<string>();

        if (terr == null)
            return NotFound();

        var Collaboratori = _dbContext.Gestiones
            .Where(g => g.TerrenoId == TerrenoId && g.Ruolo == "Collaboratore")
            .ToList();
        foreach (var collaboratore in Collaboratori)
        {
            Console.WriteLine($"Collaboratore: UserId={collaboratore.UserId}, Ruolo={collaboratore.Ruolo}");
        }

        foreach (var collab in Collaboratori)
        {

            var user = await _userManager.FindByIdAsync(collab.UserId);
            NameCollab.Add(user.UserName);
        }
        Console.WriteLine("ifuiru " + string.Join(", ", NameCollab));
        _logger.LogInformation($"HOLAAAAAAAAAAA - NomeCollab: {string.Join(", ", NameCollab)}");

        var viewModel = new
        {
            TerrenoId = terr.Id,
            Foglio = terr.Foglio,
            Mappale = terr.Mappale,
            Ettari = terr.Ettari,
            Citta = terr.CittaTerreno,
            TipoColtura = terr.TipoColtura,
            TipoTerreno = terr.TipoTerreno,
            Collaboratore = NameCollab
        };

        return View(viewModel);
    }
}