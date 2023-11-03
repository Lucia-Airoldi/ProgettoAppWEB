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

    public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
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
}