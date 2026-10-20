
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FindIt.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ServicioUsuarios _servicioUsuarios;

        public LoginModel(ServicioUsuarios servicioUsuarios)
        {
            _servicioUsuarios = servicioUsuarios;
        }

        [BindProperty]
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Introduce un correo válido.")]
        public string Correo { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contrasena { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool credencialesValidas =
                _servicioUsuarios.ValidarCredenciales(
                    Correo,
                    Contrasena);

            if (!credencialesValidas)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "El correo o la contraseña son incorrectos.");

                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Correo.Trim()),
                new Claim(ClaimTypes.Email, Correo.Trim())
            };

            var identidad = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identidad);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToPage("/Index");
        }
    }
}
