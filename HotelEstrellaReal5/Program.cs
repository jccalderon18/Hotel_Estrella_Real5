using HotelEstrellaReal5.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


// Agregar servicios a la aplicaci�n
builder.Services.AddControllersWithViews();

// Configurar la cultura predeterminada
var supportedCultures = new[] { "en-US", "es-CO", "es-ES" }; // Define las culturas soportadas por la aplicaci�n
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es-CO"), // Establece la cultura por defecto a espa�ol de Colombia
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToArray(), // Convierte las culturas soportadas a objetos CultureInfo
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToArray() // Establece las culturas de la interfaz de usuario
};

// Configura los servicios de localizaci�n en la aplicaci�n
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es-CO"); // Establece la cultura por defecto
    options.SupportedCultures = new[] { new CultureInfo("es-CO"), new CultureInfo("en-US") }; // Culturas soportadas
    options.SupportedUICultures = new[] { new CultureInfo("es-CO"), new CultureInfo("en-US") }; // Culturas de la interfaz de usuario
});


// Configuraci�n del DbContext.
builder.Services.AddDbContext<HotelEstrellaReal5Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));



// Configuraci�n del servicio de sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiraci�n de la sesi�n
    options.Cookie.HttpOnly = true; // Cookie solo accesible a trav�s de HTTP
    options.Cookie.IsEssential = true; // La cookie es esencial para la aplicaci�n
});

// Configuraci�n de autenticaci�n con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Login";
        options.LogoutPath = "/Acceso/Logout";
        options.AccessDeniedPath = "/Acceso/Denegado";
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.Name = "HOTEL_EstrellaReal";
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddControllersWithViews(optins => {
    optins.Filters.Add(new ResponseCacheAttribute() { NoStore = true, Location = ResponseCacheLocation.None });
});

var app = builder.Build();

// Configuraci�n del pipeline de solicitudes
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // El valor predeterminado de HSTS es 30 d�as. Puede que desees cambiar esto para escenarios de producci�n, consulta https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRequestLocalization(localizationOptions);

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
