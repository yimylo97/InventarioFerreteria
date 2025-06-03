using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventarioFerreteria.Models;
using InventarioFerreteria.Data;
using System.Windows.Forms;

namespace InventarioFerreteria.Repositories
{
    public class UsuarioRepository
    {
        public Usuario ValidarLogin(string nombreUsuario, string contrasena)
        {
            using (var conn = ConexionDB.ObtenerConexion())
            {
                string query = "SELECT * FROM Usuarios WHERE NombreUsuario = @nombre";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombreUsuario);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string passDB = reader["Contrasena"].ToString();

                            if (passDB == contrasena)
                            {
                                return new Usuario
                                {
                                    UsuarioID = (int)reader["UsuarioID"],
                                    NombreUsuario = reader["NombreUsuario"].ToString(),
                                    Rol = reader["Rol"].ToString()
                                };
                            }
                            else
                            {
                                MessageBox.Show("Contraseña incorrecta");
                                return null;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Usuario no encontrado");
                            return null;
                        }
                    }
                }
            }
        }

    }
}

