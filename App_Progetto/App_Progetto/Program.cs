using App_Progetto.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;
using static System.Formats.Asn1.AsnWriter;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    var configuration = builder.Configuration;
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"]!;
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

/*
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "Admin", "Agricoltore", "Collaboratore" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                Console.WriteLine($"Ruolo '{role}' creato con successo.");
            }
            else
            {
                Console.WriteLine($"Il ruolo '{role}' esiste già.");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Errore durante l'inizializzazione dei ruoli");
        // Puoi gestire ulteriormente l'eccezione qui, se necessario
    }
}
*/
await AddAdmin();

//PER CREARE ROULO ADMIN:

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();


async Task AddAdmin()
{
    // I servizi IUserStore e UserManager richiedono uno scope.
    var services = app!.Services!.CreateScope().ServiceProvider;

    var configuration = new User();
    app.Configuration.Bind("Admin", configuration);

    var usersManager = services.GetRequiredService<UserManager<IdentityUser>>();

    var user = new IdentityUser() { UserName = configuration.UserName!, Email = configuration.UserName!};
    if ((await usersManager.CreateAsync(user, configuration.Password!)).Succeeded)
    {
        await usersManager.AddToRoleAsync(user, "Admin");
        await usersManager.AddClaimAsync(user, new("Ruolo", "Admin"));
    }
}


record User
{
    public string? UserName { get; set; }

    public string? Password { get; set; }
}