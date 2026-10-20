
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace FindIt
{
    public class ServicioObjetos
    {
        private readonly string cadenaConexion;

        public ServicioObjetos(IConfiguration configuracion)
        {
            cadenaConexion = configuracion.GetConnectionString("FindItDB")
                ?? throw new InvalidOperationException(
                    "No se encontró la conexión FindItDB.");
        }

        // Registrar un objeto asociado al usuario que lo publica.
        public void RegistrarObjeto(Objeto objeto, int usuarioId)
        {
            using var conexion = new SqlConnection(cadenaConexion);
            conexion.Open();

            const string sqlCategoria = @"
                SELECT CategoriaID
                FROM dbo.Categorias
                WHERE Nombre = @Categoria;";

            int categoriaId;

            using (var comandoCategoria =
                new SqlCommand(sqlCategoria, conexion))
            {
                comandoCategoria.Parameters.Add(
                    "@Categoria", SqlDbType.NVarChar, 60)
                    .Value = objeto.Categoria;

                object? resultado = comandoCategoria.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                {
                    throw new InvalidOperationException(
                        "La categoría seleccionada no existe.");
                }

                categoriaId = Convert.ToInt32(resultado);
            }

            const string sql = @"
                INSERT INTO dbo.Reportes
                (
                    UsuarioID,
                    CategoriaID,
                    Tipo,
                    Titulo,
                    Descripcion,
                    Ubicacion,
                    FechaSuceso,
                    Estado,
                    FotoURL
                )
                VALUES
                (
                    @UsuarioID,
                    @CategoriaID,
                    @Tipo,
                    @Titulo,
                    @Descripcion,
                    @Ubicacion,
                    @FechaSuceso,
                    'Activo',
                    @FotoURL
                );";

            using var comando = new SqlCommand(sql, conexion);

            comando.Parameters.Add("@UsuarioID", SqlDbType.Int)
                .Value = usuarioId;

            comando.Parameters.Add("@CategoriaID", SqlDbType.Int)
                .Value = categoriaId;

            comando.Parameters.Add("@Tipo", SqlDbType.NVarChar, 15)
                .Value = objeto.TipoReporte;

            comando.Parameters.Add("@Titulo", SqlDbType.NVarChar, 120)
                .Value = objeto.Nombre;

            comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 1000)
                .Value = objeto.Descripcion;

            comando.Parameters.Add("@Ubicacion", SqlDbType.NVarChar, 200)
                .Value = objeto.Ubicacion;

            comando.Parameters.Add("@FechaSuceso", SqlDbType.Date)
                .Value = objeto.Fecha == default
                    ? DBNull.Value
                    : objeto.Fecha.Date;

            comando.Parameters.Add("@FotoURL", SqlDbType.NVarChar, 500)
                .Value = string.IsNullOrWhiteSpace(objeto.FotoURL)
                    ? DBNull.Value
                    : objeto.FotoURL;

            comando.ExecuteNonQuery();
        }

        // Obtener todos los objetos publicados, incluido su propietario.
        public List<Objeto> ObtenerTodos()
        {
            var objetos = new List<Objeto>();

            const string sql = @"
                SELECT
                    r.ReporteID,
                    r.UsuarioID,
                    r.Titulo,
                    r.Descripcion,
                    r.Ubicacion,
                    r.Tipo,
                    r.FechaSuceso,
                    r.Estado,
                    c.Nombre AS Categoria,
                    r.FotoURL
                FROM dbo.Reportes r
                INNER JOIN dbo.Categorias c
                    ON r.CategoriaID = c.CategoriaID
                ORDER BY r.FechaPublicacion DESC;";

            using var conexion = new SqlConnection(cadenaConexion);
            using var comando = new SqlCommand(sql, conexion);

            conexion.Open();

            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                objetos.Add(new Objeto
                {
                    IdObjeto = lector.GetInt32(0),
                    UsuarioID = lector.GetInt32(1),
                    Nombre = lector.GetString(2),
                    Descripcion = lector.GetString(3),
                    Ubicacion = lector.GetString(4),
                    TipoReporte = lector.GetString(5),
                    Fecha = lector.IsDBNull(6)
                        ? default
                        : lector.GetDateTime(6),
                    Estado = lector.GetString(7),
                    Categoria = lector.GetString(8),
                    FotoURL = lector.IsDBNull(9)
                        ? null
                        : lector.GetString(9)
                });
            }

            return objetos;
        }

        // Buscar objetos por texto.
        public List<Objeto> BuscarObjetos(string texto)
        {
            texto ??= "";
            texto = texto.Trim();

            return ObtenerTodos().FindAll(o =>
                (o.Nombre ?? "").Contains(
                    texto, StringComparison.OrdinalIgnoreCase) ||
                (o.Descripcion ?? "").Contains(
                    texto, StringComparison.OrdinalIgnoreCase) ||
                (o.Categoria ?? "").Contains(
                    texto, StringComparison.OrdinalIgnoreCase) ||
                (o.Color ?? "").Contains(
                    texto, StringComparison.OrdinalIgnoreCase) ||
                (o.Ubicacion ?? "").Contains(
                    texto, StringComparison.OrdinalIgnoreCase));
        }

        // Actualizar un objeto únicamente si pertenece al usuario.
        public bool ActualizarObjeto(Objeto objeto, int usuarioId)
        {
            using var conexion = new SqlConnection(cadenaConexion);
            conexion.Open();

            const string sqlCategoria = @"
                SELECT CategoriaID
                FROM dbo.Categorias
                WHERE Nombre = @Categoria;";

            int categoriaId;

            using (var comandoCategoria =
                new SqlCommand(sqlCategoria, conexion))
            {
                comandoCategoria.Parameters.Add(
                    "@Categoria", SqlDbType.NVarChar, 60)
                    .Value = objeto.Categoria;

                object? resultado = comandoCategoria.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                {
                    throw new InvalidOperationException(
                        "La categoría seleccionada no existe.");
                }

                categoriaId = Convert.ToInt32(resultado);
            }

            const string sql = @"
                UPDATE dbo.Reportes
                SET
                    CategoriaID = @CategoriaID,
                    Tipo = @Tipo,
                    Titulo = @Titulo,
                    Descripcion = @Descripcion,
                    Ubicacion = @Ubicacion,
                    FechaSuceso = @FechaSuceso,
                    Estado = @Estado
                WHERE ReporteID = @ReporteID
                  AND UsuarioID = @UsuarioID;";

            using var comando = new SqlCommand(sql, conexion);

            comando.Parameters.Add("@CategoriaID", SqlDbType.Int)
                .Value = categoriaId;

            comando.Parameters.Add("@Tipo", SqlDbType.NVarChar, 15)
                .Value = objeto.TipoReporte;

            comando.Parameters.Add("@Titulo", SqlDbType.NVarChar, 120)
                .Value = objeto.Nombre;

            comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 1000)
                .Value = objeto.Descripcion;

            comando.Parameters.Add("@Ubicacion", SqlDbType.NVarChar, 200)
                .Value = objeto.Ubicacion;

            comando.Parameters.Add("@FechaSuceso", SqlDbType.Date)
                .Value = objeto.Fecha == default
                    ? DBNull.Value
                    : objeto.Fecha.Date;

            comando.Parameters.Add("@Estado", SqlDbType.NVarChar, 20)
                .Value = objeto.Estado;

            comando.Parameters.Add("@ReporteID", SqlDbType.Int)
                .Value = objeto.IdObjeto;

            comando.Parameters.Add("@UsuarioID", SqlDbType.Int)
                .Value = usuarioId;

            return comando.ExecuteNonQuery() > 0;
        }

        // Eliminar un objeto únicamente si pertenece al usuario.
        public bool EliminarObjeto(int id, int usuarioId)
        {
            using var conexion = new SqlConnection(cadenaConexion);
            conexion.Open();

            const string sql = @"
                DELETE FROM dbo.Reportes
                WHERE ReporteID = @ReporteID
                  AND UsuarioID = @UsuarioID;";

            using var comando = new SqlCommand(sql, conexion);

            comando.Parameters.Add("@ReporteID", SqlDbType.Int)
                .Value = id;

            comando.Parameters.Add("@UsuarioID", SqlDbType.Int)
                .Value = usuarioId;

            return comando.ExecuteNonQuery() > 0;
        }

        // Comprobar si el objeto pertenece al usuario.
        public bool PerteneceAlUsuario(int id, int usuarioId)
        {
            using var conexion = new SqlConnection(cadenaConexion);

            const string sql = @"
                SELECT COUNT(1)
                FROM dbo.Reportes
                WHERE ReporteID = @ReporteID
                  AND UsuarioID = @UsuarioID;";

            using var comando = new SqlCommand(sql, conexion);

            comando.Parameters.Add("@ReporteID", SqlDbType.Int)
                .Value = id;

            comando.Parameters.Add("@UsuarioID", SqlDbType.Int)
                .Value = usuarioId;

            conexion.Open();

            return Convert.ToInt32(comando.ExecuteScalar()) > 0;
        }
    }
}
