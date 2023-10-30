using App_Progetto.ModelView;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics.Metrics;

namespace App_Progetto.Controllers;


//[ApiController]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    
    public async Task<IActionResult> ViewRole()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var users = await _userManager.Users.ToListAsync();

        /*
         * var usersWithRoles = _userManager.Users
            .Select(user => new
            {
                UserId = user.Id,
                Username = user.UserName,
                Roles = _userManager.GetRolesAsync(user).Result
            })
            .ToList();
         */

        List<UserRole> result = new ();
        int idCounter = 1;
        foreach (var user in users) 
        {
            if (user.Id != currentUser.Id)
            {
                result.Add(new UserRole()
                {
                    Id = idCounter++,
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                }); ;
            }
        }

        return View(result);
    }

    [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageRoles(string userId)
        {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound();

        var roles = await _roleManager.Roles.ToListAsync();

        var viewModel = new Role
        {
            UserId = user.Id,
            UserName = user.UserName,
            Roles = roles.Select(role => new RoleViewUser
            {
                RoleId = role.Id,
                RoleName = role.Name,
                IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ManageRoles(Role model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);

        if (user == null)
            return NotFound();

        var userRoles = await _userManager.GetRolesAsync(user);

        foreach (var role in model.Roles)
        {
            if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                await _userManager.RemoveFromRoleAsync(user, role.RoleName);

            if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                await _userManager.AddToRoleAsync(user, role.RoleName);
        }

        return RedirectToAction(nameof(Index));
    }

}
