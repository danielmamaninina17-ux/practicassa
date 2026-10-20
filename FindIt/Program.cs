
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FindIt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Registrar Razor Pages.
            builder.Services.AddRazorPages();

            // Registrar los servicios de la aplicación.
            builder.Services.AddScoped<ServicioObjetos>();
            builder.Services.AddScoped<ServicioUsuarios>();

            // Configurar la autenticación mediante cookies.
            builder.Services
                .AddAuthentication(
                    CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.AccessDeniedPath = "/Login";

                    options.Cookie.Name = "FindIt.Auth";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Cookie.SecurePolicy =
                        CookieSecurePolicy.SameAsRequest;

                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                    options.SlidingExpiration = true;
                });

            var app = builder.Build();

            // Configurar el entorno de ejecución.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Configurar el procesamiento de solicitudes.
            app.UseHttpsRedirection();
            app.UseRouting();

            // Activar autenticación y autorización.
            app.UseAuthentication();
            app.UseAuthorization();

            // Habilitar archivos estáticos y páginas Razor.
            app.MapStaticAssets();
            app.MapRazorPages().WithStaticAssets();

            // Iniciar la aplicación.
            app.Run();
        }
    }
}
