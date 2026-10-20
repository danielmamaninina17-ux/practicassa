
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Security.Claims;

namespace FindIt.Pages
{
    [Authorize]
    public class RegistrarModel : PageModel
    {
        private readonly ServicioObjetos _servicio;
        private readonly ServicioUsuarios _servicioUsuarios;
        private readonly IWebHostEnvironment _entorno;

        public RegistrarModel(
            ServicioObjetos servicio,
            ServicioUsuarios servicioUsuarios,
            IWebHostEnvironment entorno)
        {
            _servicio = servicio;
            _servicioUsuarios = servicioUsuarios;
            _entorno = entorno;
        }

        [BindProperty]
        public Objeto NuevoObjeto { get; set; } = new();

        [BindProperty]
        public IFormFile? Foto { get; set; }

        public void OnGet()
        {
            NuevoObjeto.Fecha = DateTime.Today;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (NuevoObjeto.Fecha == default)
            {
                NuevoObjeto.Fecha = DateTime.Today;
            }

            if (string.IsNullOrWhiteSpace(NuevoObjeto.Nombre) ||
                string.IsNullOrWhiteSpace(NuevoObjeto.Descripcion) ||
                string.IsNullOrWhiteSpace(NuevoObjeto.Ubicacion) ||
                string.IsNullOrWhiteSpace(NuevoObjeto.Categoria) ||
                string.IsNullOrWhiteSpace(NuevoObjeto.TipoReporte))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Completa todos los campos obligatorios.");

                return Page();
            }

            if (NuevoObjeto.Nombre.Length > 120 ||
                NuevoObjeto.Descripcion.Length > 1000 ||
                NuevoObjeto.Ubicacion.Length > 200)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Revisa la longitud del nombre, la descripción o la ubicación.");

                return Page();
            }

            if (NuevoObjeto.TipoReporte != "Perdido" &&
                NuevoObjeto.TipoReporte != "Encontrado")
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Selecciona si el objeto está perdido o fue encontrado.");

                return Page();
            }

            string? rutaFisicaFoto = null;

            try
            {
                // Obtener el correo de la persona que inició sesión.
                string? correo = User.FindFirstValue(ClaimTypes.Email);

                if (string.IsNullOrWhiteSpace(correo))
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "No se pudo identificar al usuario. Inicia sesión nuevamente.");

                    return Page();
                }

                // Buscar el ID de ese usuario en la base de datos.
                int? usuarioId =
                    _servicioUsuarios.ObtenerUsuarioIdPorCorreo(correo);

                if (usuarioId == null)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "No se encontró tu cuenta activa. Inicia sesión nuevamente.");

                    return Page();
                }

                if (Foto != null && Foto.Length > 0)
                {
                    const long tamanoMaximo = 5 * 1024 * 1024;

                    if (Foto.Length > tamanoMaximo)
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            "La fotografía no puede superar los 5 MB.");

                        return Page();
                    }

                    string? extension =
                        await ObtenerExtensionImagen(Foto);

                    if (extension == null)
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            "Selecciona una imagen JPG, PNG, GIF o WEBP válida.");

                        return Page();
                    }

                    string carpetaWeb = _entorno.WebRootPath
                        ?? Path.Combine(
                            _entorno.ContentRootPath, "wwwroot");

                    string carpetaUploads = Path.Combine(
                        carpetaWeb, "uploads");

                    Directory.CreateDirectory(carpetaUploads);

                    string nombreArchivo =
                        $"{Guid.NewGuid():N}{extension}";

                    rutaFisicaFoto = Path.Combine(
                        carpetaUploads, nombreArchivo);

                    using (var flujo = new FileStream(
                        rutaFisicaFoto,
                        FileMode.CreateNew,
                        FileAccess.Write,
                        FileShare.None))
                    {
                        await Foto.CopyToAsync(flujo);
                    }

                    NuevoObjeto.FotoURL =
                        "/uploads/" + nombreArchivo;
                }

                // Registrar el objeto con el ID del usuario autenticado.
                _servicio.RegistrarObjeto(NuevoObjeto, usuarioId.Value);

                return RedirectToPage("/Objetos");
            }
            catch (Exception)
            {
                if (rutaFisicaFoto != null &&
                    System.IO.File.Exists(rutaFisicaFoto))
                {
                    System.IO.File.Delete(rutaFisicaFoto);
                }

                ModelState.AddModelError(
                    string.Empty,
                    "No se pudo publicar el objeto. Verifica los datos e inténtalo nuevamente.");

                return Page();
            }
        }

        private static async Task<string?> ObtenerExtensionImagen(
            IFormFile archivo)
        {
            byte[] firma = new byte[12];

            using var flujo = archivo.OpenReadStream();

            int leidos = await flujo.ReadAsync(
                firma, 0, firma.Length);

            if (leidos >= 3 &&
                firma[0] == 0xFF &&
                firma[1] == 0xD8 &&
                firma[2] == 0xFF)
            {
                return ".jpg";
            }

            if (leidos >= 8 &&
                firma[0] == 0x89 &&
                firma[1] == 0x50 &&
                firma[2] == 0x4E &&
                firma[3] == 0x47 &&
                firma[4] == 0x0D &&
                firma[5] == 0x0A &&
                firma[6] == 0x1A &&
                firma[7] == 0x0A)
            {
                return ".png";
            }

            if (leidos >= 6 &&
                firma[0] == 0x47 &&
                firma[1] == 0x49 &&
                firma[2] == 0x46 &&
                firma[3] == 0x38 &&
                (firma[4] == 0x37 || firma[4] == 0x39) &&
                firma[5] == 0x61)
            {
                return ".gif";
            }

            if (leidos >= 12 &&
                firma[0] == 0x52 &&
                firma[1] == 0x49 &&
                firma[2] == 0x46 &&
                firma[3] == 0x46 &&
                firma[8] == 0x57 &&
                firma[9] == 0x45 &&
                firma[10] == 0x42 &&
                firma[11] == 0x50)
            {
                return ".webp";
            }

            return null;
        }
    }
}
