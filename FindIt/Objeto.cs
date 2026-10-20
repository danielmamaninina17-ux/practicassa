
    using System;

    namespace FindIt
    {
        public class Objeto
        {
            public int IdObjeto { get; set; }
             public int UsuarioID { get; set; }

            public string Nombre { get; set; } = "";

            public string Descripcion { get; set; } = "";

            public string Categoria { get; set; } = "";

            public string Color { get; set; } = "";

            public string Ubicacion { get; set; } = "";

            public string TipoReporte { get; set; } = "";

            public DateTime Fecha { get; set; }

            public string Estado { get; set; } = "Pendiente";

            public string? FotoURL { get; set; }
        }
    }
