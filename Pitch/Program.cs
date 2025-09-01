using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http; // SameSiteMode, CookieSecurePolicy
using Microsoft.AspNetCore.Rewrite; // Redirect "/" -> "/Landing"
using Microsoft.AspNetCore.DataProtection; // Persistência de chaves
using Pitch.Data;
using Pitch.Models;
using Pitch.Repositories;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages (com política de autorização padrão)
builder.Services.AddRazorPages(options =>
{
    // Protege tudo por padrão
    options.Conventions.AuthorizeFolder("/");

    // Libera anonimamente o que precisa ser público
    options.Conventions.AllowAnonymousToFolder("/Account");
    options.Conventions.AllowAnonymousToPage("/Privacy");
    options.Conventions.AllowAnonymousToPage("/Landing"); // nova home pública
});

// DbContext (MySQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 34)),
        mysqlOptions =>
        {
            mysqlOptions.SchemaBehavior(MySqlSchemaBehavior.Translate,
                (schema, table) => $"{schema}_{table}");
        }
    )
);

// Identity
builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Cookie de autenticação (HTTP-only em dev)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "PitchAuth";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;

    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // permite HTTP em dev
});

// Persistência de chaves de DataProtection (evita invalidar cookie entre requests/execuções)
var keysPath = Path.Combine(builder.Environment.ContentRootPath, "PitchKeys");
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
    .SetApplicationName("Pitch");

// Controllers
builder.Services.AddControllers();

// DI Repositories
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// IMPORTANTE: NÃO redirecionar para HTTPS em HTTP-only local
// app.UseHttpsRedirection();

// Redireciona "/" -> "/Landing" (nova Home)
var rewrite = new RewriteOptions()
    .AddRedirect("^$", "Landing");
app.UseRewriter(rewrite);

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // <- antes de Authorization
app.UseAuthorization();  // <- depois

app.MapControllers();
app.MapRazorPages();

// Seed de Roles e Admin
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<Users>>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        var roles = new[] { "Colaborador", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminEmail = "admin@pitch.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new Users
            {
                UserName = adminEmail,
                Email = adminEmail,
                Nome = "Administrador",
                EmailConfirmed = true
            };

            var adminPassword = "Admin@123";
            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                logger.LogInformation("Usuário admin criado: {Email}", adminEmail);
            }
            else
            {
                logger.LogError("Falha ao criar admin: {Erros}", string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro durante a criação de roles/usuário admin");
    }
}

app.Run();
