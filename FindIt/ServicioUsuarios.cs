
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace FindIt
{
    public class ServicioUsuarios
    {
        private readonly string _cadenaConexion;
        private readonly PasswordHasher<string> _hasher = new();

        public ServicioUsuarios(IConfiguration configuracion)
        {
            _cadenaConexion =
                configuracion.GetConnectionString("FindItDB")
                ?? throw new InvalidOperationException(
                    "No se encontró la conexión FindItDB.");
        }

        public bool CorreoExiste(string correo)
        {
            using SqlConnection conexion = new(_cadenaConexion);

            string consulta = @"
                SELECT COUNT(1)
                FROM dbo.Usuarios
                WHERE Correo = @Correo;";

            using SqlCommand comando = new(consulta, conexion);

            comando.Parameters.Add("@Correo", SqlDbType.NVarChar, 150)
                .Value = correo.Trim();

            conexion.Open();

            return Convert.ToInt32(comando.ExecuteScalar()) > 0;
        }

        public bool RegistrarUsuario(
            string nombre,
            string correo,
            string? telefono,
            string contrasena)
        {
            using SqlConnection conexion = new(_cadenaConexion);

            string hash = _hasher.HashPassword(
                correo.Trim(),
                contrasena);

            string consulta = @"
                INSERT INTO dbo.Usuarios
                    (Nombre, Correo, ContrasenaHash, Telefono)
                VALUES
                    (@Nombre, @Correo, @ContrasenaHash, @Telefono);";

            using SqlCommand comando = new(consulta, conexion);

            comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100)
                .Value = nombre.Trim();

            comando.Parameters.Add("@Correo", SqlDbType.NVarChar, 150)
                .Value = correo.Trim();

            comando.Parameters.Add("@ContrasenaHash", SqlDbType.NVarChar, 255)
                .Value = hash;

            comando.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20)
                .Value = string.IsNullOrWhiteSpace(telefono)
                    ? DBNull.Value
                    : telefono.Trim();

            conexion.Open();
            comando.ExecuteNonQuery();

            return true;
        }

        public bool ValidarCredenciales(
            string correo,
            string contrasena)
        {
            using SqlConnection conexion = new(_cadenaConexion);

            string consulta = @"
                SELECT ContrasenaHash
                FROM dbo.Usuarios
                WHERE Correo = @Correo
                  AND Activo = 1;";

            using SqlCommand comando = new(consulta, conexion);

            comando.Parameters.Add("@Correo", SqlDbType.NVarChar, 150)
                .Value = correo.Trim();

            conexion.Open();

            object? resultado = comando.ExecuteScalar();

            if (resultado == null || resultado == DBNull.Value)
            {
                return false;
            }

            string hashGuardado = Convert.ToString(resultado)!;

            PasswordVerificationResult verificacion =
                _hasher.VerifyHashedPassword(
                    correo.Trim(),
                    hashGuardado,
                    contrasena);

            return verificacion != PasswordVerificationResult.Failed;
        }

        public int? ObtenerUsuarioIdPorCorreo(string correo)
        {
            using SqlConnection conexion = new(_cadenaConexion);

            string consulta = @"
                SELECT UsuarioID
                FROM dbo.Usuarios
                WHERE Correo = @Correo
                  AND Activo = 1;";

            using SqlCommand comando = new(consulta, conexion);

            comando.Parameters.Add("@Correo", SqlDbType.NVarChar, 150)
                .Value = correo.Trim();

            conexion.Open();

            object? resultado = comando.ExecuteScalar();

            if (resultado == null || resultado == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(resultado);
        }

        public bool CambiarContrasena(
            string correo,
            string nuevaContrasena)
        {
            if (string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(nuevaContrasena))
            {
                return false;
            }

            string correoLimpio = correo.Trim();

            string hash = _hasher.HashPassword(
                correoLimpio,
                nuevaContrasena);

            using SqlConnection conexion = new(_cadenaConexion);

            string consulta = @"
                UPDATE dbo.Usuarios
                SET ContrasenaHash = @ContrasenaHash
                WHERE Correo = @Correo
                  AND Activo = 1;";

            using SqlCommand comando = new(consulta, conexion);

            comando.Parameters.Add(
                "@ContrasenaHash", SqlDbType.NVarChar, 255)
                .Value = hash;

            comando.Parameters.Add(
                "@Correo", SqlDbType.NVarChar, 150)
                .Value = correoLimpio;

            conexion.Open();

            return comando.ExecuteNonQuery() > 0;
        }
    }
}
