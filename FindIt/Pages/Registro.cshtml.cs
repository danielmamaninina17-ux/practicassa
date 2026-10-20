
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FindIt.Pages
{
    public class RegistroModel : PageModel
    {
        private readonly ServicioUsuarios _servicioUsuarios;

        public RegistroModel(ServicioUsuarios servicioUsuarios)
        {
            _servicioUsuarios = servicioUsuarios;
        }

        [BindProperty]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Introduce un correo electrónico válido.")]
        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        [BindProperty]
        [Phone(ErrorMessage = "Introduce un teléfono válido.")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8,
            ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres.")]
        public string Contrasena { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (_servicioUsuarios.CorreoExiste(Correo))
                {
                    ModelState.AddModelError(
                        nameof(Correo),
                        "Este correo electrónico ya está registrado.");

                    return Page();
                }

                _servicioUsuarios.RegistrarUsuario(
                    Nombre,
                    Correo,
                    Telefono,
                    Contrasena);

                TempData["MensajeExito"] =
                    "¡Registro exitoso! Tu cuenta ha sido creada.";

                return RedirectToPage("/Registro");
            }
            catch (SqlException ex) when (
                ex.Number == 2601 || ex.Number == 2627)
            {
                ModelState.AddModelError(
                    nameof(Correo),
                    "Este correo electrónico ya está registrado.");

                return Page();
            }
            catch (SqlException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "No se pudo completar el registro. Inténtalo nuevamente.");

                return Page();
            }
        }
    }
}
