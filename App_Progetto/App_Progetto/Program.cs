using App_Progetto.Controllers;
using App_Progetto.Data;
using App_Progetto.DatiDb;
using App_Progetto.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Text;
using static System.Formats.Asn1.AsnWriter;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


// Per Entity Framework
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Per Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

//builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();

// Per Authentication esterno
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = configuration["Authentication:Google:ClientId"]!;
        googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
        //googleOptions.CallbackPath = "/User/HomeTerreno"; // Questa è l'URL di callback
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSecretKey"]!))
        };
    });

//builder.Services.AddHttpContextAccessor();
//builder.Services.AddSession();


builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "AppProgetto API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

var app = builder.Build();

//app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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

await AddAdmin();
//await SetData();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

//PER CREARE ROULO ADMIN:

async Task AddAdmin()
{
    // I servizi IUserStore e UserManager richiedono uno scope.
    var services = app!.Services!.CreateScope().ServiceProvider;

    var configuration = new User();
    app.Configuration.Bind("Admin", configuration);

    if (!string.IsNullOrEmpty(configuration.UserName)) // Verifica se UserName è diverso da null o vuoto
    {
        var usersManager = services.GetRequiredService<UserManager<IdentityUser>>();

        var user = new IdentityUser() { UserName = configuration.UserName, Email = configuration.UserName };
        if ((await usersManager.CreateAsync(user, configuration.Password)).Succeeded)
        {
            await usersManager.AddToRoleAsync(user, "Admin");
            await usersManager.AddClaimAsync(user, new("Ruolo", "Admin"));
        }
    }
    else
    {
        // Gestisci il caso in cui configuration.UserName sia nullo o vuoto
        Console.WriteLine("Nome utente non valido nella configurazione 'Admin'.");
    }
}



/*
async Task SetData()
{
    var terrenoDataList = JsonFileReader.DeserializeJsonFile<List<Terreno>>("C:\\Users\\user\\Desktop\\App_Web_progetto\\App_Progetto\\App_Progetto\\DatiDb\\Terrone.json");

    InsertDataIntoTerrenoTable(terrenoDataList);
    var path = args.Length == 0
    ? @"C:\Users\user\Desktop\App_Web_progetto\App_Progetto\App_Progetto\DatiDb\Terrone.json.txt"
    : args[0];

    var stream = File.OpenRead(path);
    //quando uso gli stream è meglio usare ilmetodo async per risparmiare memoria ("traduco" direttamente dalfile, senza salvarlo in memoria)
    var users = await JsonSerializer.DeserializeAsync<UserImport[]>(stream);
}
*/
record User
{
    public string? UserName { get; set; }

    public string? Password { get; set; }
}

/*void MapViewRole()
{
    var group = app.MapGroup("/viewRole").WithTags("Admin");

    group
        .MapGet("/", (AdminController contrAdmin) => contrAdmin.GetAll())
        .RequireAuthorization(PolicyName.IsAdmin);
}*/
