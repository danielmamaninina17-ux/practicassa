
using FindIt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace FindIt.Pages
{
    [Authorize]
    public class ObjetosModel : PageModel
    {
        private readonly ServicioObjetos _servicio;
        private readonly ServicioUsuarios _servicioUsuarios;

        public List<Objeto> Objetos { get; set; } = new();

        public int? UsuarioActualId { get; set; }

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

        [BindProperty(SupportsGet = true)]
        public string Busqueda { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public string Tipo { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public string Categoria { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public string EstadoFiltro { get; set; } = "";

        public ObjetosModel(
            ServicioObjetos servicio,
            ServicioUsuarios servicioUsuarios)
        {
            _servicio = servicio;
            _servicioUsuarios = servicioUsuarios;
        }

        public IActionResult OnGet()
        {
            string? correo = User.FindFirstValue(
                ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(correo))
            {
                Console.WriteLine(
                    "ERROR: No se encontró el correo en la sesión.");

                return Challenge();
            }

            UsuarioActualId =
                _servicioUsuarios.ObtenerUsuarioIdPorCorreo(correo);

            // Diagnóstico temporal para comprobar la sesión.
            Console.WriteLine(
                $"CORREO DE SESIÓN: {correo}");

            Console.WriteLine(
                $"ID DE USUARIO: {UsuarioActualId}");

            if (!UsuarioActualId.HasValue)
            {
                Console.WriteLine(
                    "ERROR: No se encontró un usuario activo para este correo.");

                return Challenge();
            }

            Objetos = _servicio.ObtenerTodos();

            if (!string.IsNullOrWhiteSpace(Busqueda))
            {
                Objetos = Objetos.FindAll(o =>
                    (o.Nombre ?? "").Contains(
                        Busqueda,
                        StringComparison.OrdinalIgnoreCase) ||
                    (o.Descripcion ?? "").Contains(
                        Busqueda,
                        StringComparison.OrdinalIgnoreCase) ||
                    (o.Categoria ?? "").Contains(
                        Busqueda,
                        StringComparison.OrdinalIgnoreCase) ||
                    (o.Ubicacion ?? "").Contains(
                        Busqueda,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(Tipo))
            {
                Objetos = Objetos.FindAll(o =>
                    (o.TipoReporte ?? "").Equals(
                        Tipo,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(Categoria))
            {
                Objetos = Objetos.FindAll(o =>
                    (o.Categoria ?? "").Equals(
                        Categoria,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(EstadoFiltro))
            {
                Objetos = Objetos.FindAll(o =>
                    (o.Estado ?? "").Equals(
                        EstadoFiltro,
                        StringComparison.OrdinalIgnoreCase));
            }

            return Page();
        }

        public IActionResult OnPostEliminar(int id)
        {
            if (id <= 0)
                return BadRequest();

            string? correo = User.FindFirstValue(
                ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(correo))
                return Challenge();

            int? usuarioId =
                _servicioUsuarios.ObtenerUsuarioIdPorCorreo(correo);

            if (!usuarioId.HasValue)
                return Challenge();

            bool eliminado =
                _servicio.EliminarObjeto(id, usuarioId.Value);

            if (!eliminado)
                return NotFound();

            return RedirectToPage("/Objetos");
        }
    }
}
