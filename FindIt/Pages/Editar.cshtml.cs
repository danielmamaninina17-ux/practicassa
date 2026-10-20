
using FindIt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FindIt.Pages
{
    [Authorize]
    public class EditarModel : PageModel
    {
        private readonly ServicioObjetos _servicio;
        private readonly ServicioUsuarios _servicioUsuarios;

        public EditarModel(
            ServicioObjetos servicio,
            ServicioUsuarios servicioUsuarios)
        {
            _servicio = servicio;
            _servicioUsuarios = servicioUsuarios;
        }

        [BindProperty]
        public Objeto ObjetoEditar { get; set; } = new();

        public List<string> Categorias { get; set; } = new()
        {
            "Documentos",
            "Telefonos",
            "Llaves",
            "Mochilas y bolsos",
            "Ropa",
            "Electronica",
            "Accesorios",
            "Otros"
        };

        public IActionResult OnGet(int id)
        {
            int? usuarioId = ObtenerUsuarioId();

            if (usuarioId == null)
                return Challenge();

            var objeto = _servicio.ObtenerTodos()
                .FirstOrDefault(o => o.IdObjeto == id);

            if (objeto == null)
                return NotFound();

            // Verificar que el reporte pertenezca al usuario.
            if (!_servicio.PerteneceAlUsuario(id, usuarioId.Value))
                return NotFound();

            ObjetoEditar = objeto;

            return Page();
        }

        public IActionResult OnPost()
        {
            int? usuarioId = ObtenerUsuarioId();

            if (usuarioId == null)
                return Challenge();

            if (ObjetoEditar.IdObjeto <= 0)
                return BadRequest();

            // Comprobar la propiedad antes de validar y actualizar.
            if (!_servicio.PerteneceAlUsuario(
                    ObjetoEditar.IdObjeto, usuarioId.Value))
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(ObjetoEditar.Nombre) ||
                string.IsNullOrWhiteSpace(ObjetoEditar.Descripcion) ||
                string.IsNullOrWhiteSpace(ObjetoEditar.Ubicacion) ||
                string.IsNullOrWhiteSpace(ObjetoEditar.Categoria) ||
                string.IsNullOrWhiteSpace(ObjetoEditar.TipoReporte) ||
                string.IsNullOrWhiteSpace(ObjetoEditar.Estado))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Completa todos los campos obligatorios.");

                CargarCategorias();
                return Page();
            }

            if (ObjetoEditar.Nombre.Length > 120 ||
                ObjetoEditar.Descripcion.Length > 1000 ||
                ObjetoEditar.Ubicacion.Length > 200)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Revisa la longitud de los campos.");

                CargarCategorias();
                return Page();
            }

            if (ObjetoEditar.TipoReporte != "Perdido" &&
                ObjetoEditar.TipoReporte != "Encontrado")
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Selecciona un tipo de reporte válido.");

                CargarCategorias();
                return Page();
            }

            string[] estadosPermitidos =
            {
                "Activo",
                "EnProceso",
                "Devuelto",
                "Cerrado"
            };

            if (!estadosPermitidos.Contains(ObjetoEditar.Estado))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Selecciona un estado válido.");

                CargarCategorias();
                return Page();
            }

            try
            {
                bool actualizado = _servicio.ActualizarObjeto(
                    ObjetoEditar, usuarioId.Value);

                if (!actualizado)
                    return NotFound();

                return RedirectToPage("/Objetos");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                CargarCategorias();
                return Page();
            }
        }

        private int? ObtenerUsuarioId()
        {
            string? correo = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(correo))
                return null;

            return _servicioUsuarios
                .ObtenerUsuarioIdPorCorreo(correo);
        }

        private void CargarCategorias()
        {
            Categorias = new List<string>
            {
                "Documentos",
                "Telefonos",
                "Llaves",
                "Mochilas y bolsos",
                "Ropa",
                "Electronica",
                "Accesorios",
                "Otros"
            };
        }
    }
}
