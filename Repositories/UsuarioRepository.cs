using FerreteriaApp.Data;
using InventarioFerreteria.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerreteriaApp.Repositories
{
    public class UsuarioRepository
    {
        public Usuario ValidarLogin(string nombreUsuario, string contrasena)
        {
            using (var conn = ConexionDB.ObtenerConexion())
            {
                string query = "SELECT * FROM Usuarios WHERE NombreUsuario = @nombre AND Contrasena = @contrasena";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                  cmd.Parameters.AddWithValue("@nombre", nombreUsuario);
                  cmd.Parameters.AddWithValue("@contrasena", contrasena);
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                       if (reader.Read())
                       {
                         return new Usuario
                         { 
                             UsuarioID = (int)reader["UsuarioID"],
                             NombreUsuario = reader["NombreUsuario"].ToString(),
                             Rol = reader["Rol"].ToString()
                         };
                       }
                     }
                }
            }

           return null;
        }
    }
}

