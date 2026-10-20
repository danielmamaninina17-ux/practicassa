
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FindIt.Pages
{
    public class DetalleModel : PageModel
    {
        private readonly ServicioObjetos _servicio;

        public Objeto? ObjetoDetalle { get; set; }

        public DetalleModel(ServicioObjetos servicio)
        {
            _servicio = servicio;
        }

        public IActionResult OnGet(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            ObjetoDetalle = _servicio.ObtenerTodos()
                .FirstOrDefault(o => o.IdObjeto == id);

            if (ObjetoDetalle == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
