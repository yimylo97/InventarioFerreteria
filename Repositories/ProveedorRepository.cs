using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InventarioFerreteria.Models;
using InventarioFerreteria.Data;

namespace InventarioFerreteria.Repositories
{
    public class ProveedorRepository
    {
        public void Agregar(Proveedor proveedor)
        {
            using (var conn = ConexionDB.ObtenerConexion())
            {
                var query = "INSERT INTO Proveedores (Nombre, Contacto, ProductoID) VALUES (@Nombre, @Contacto, @ProductoID)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                    cmd.Parameters.AddWithValue("@Contacto", proveedor.Contacto);
                    cmd.Parameters.AddWithValue("@ProductoID", proveedor.ProductoID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Proveedor> ObtenerTodos()
        {
            var lista = new List<Proveedor>();

            using (var conn = ConexionDB.ObtenerConexion())
            {
                var query = "SELECT * FROM Proveedores";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Proveedor
                        {
                            ProveedorID = (int)reader["ProveedorID"],
                            Nombre = reader["Nombre"].ToString(),
                            Contacto = reader["Contacto"].ToString(),
                            ProductoID = (int)reader["ProductoID"]
                        });
                    }
                }
            }

            return lista;
        }

        public void Actualizar(Proveedor proveedor)
        {
            using (var conn = ConexionDB.ObtenerConexion())
            {
                var query = "UPDATE Proveedores SET Nombre=@Nombre, Contacto=@Contacto, ProductoID=@ProductoID WHERE ProveedorID=@ProveedorID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                    cmd.Parameters.AddWithValue("@Contacto", proveedor.Contacto);
                    cmd.Parameters.AddWithValue("@ProductoID", proveedor.ProductoID);
                    cmd.Parameters.AddWithValue("@ProveedorID", proveedor.ProveedorID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int id)
        {
            using (var conn = ConexionDB.ObtenerConexion())
            {
                var query = "DELETE FROM Proveedores WHERE ProveedorID=@id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

