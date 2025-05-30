using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FerreteriaApp.Models;
using FerreteriaApp.Data;

namespace FerreteriaApp.Repositories
{
    public class ProductoRepository
    {
        public void AgregarProducto(Producto producto)
        {
            string query = "INSERT INTO Productos (Marca, Modelo, Precio, CantidadInventario) VALUES (@Marca, @Modelo, @Precio, @Cantidad)";

            using (SqlCommand cmd = new SqlCommand(query, ConexionDB.ObtenerConexion()))
            {
                cmd.Parameters.AddWithValue("@Marca", producto.Marca);
                cmd.Parameters.AddWithValue("@Modelo", producto.Modelo);
                cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                cmd.Parameters.AddWithValue("@Cantidad", producto.CantidadInventario);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Producto> ObtenerTodos()
        {
            List<Producto> lista = new List<Producto>();
            string query = "SELECT * FROM Productos";

            using (SqlCommand cmd = new SqlCommand(query, ConexionDB.ObtenerConexion()))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new Producto
                    {
                        ProductoID = (int)reader["ProductoID"],
                        Marca = reader["Marca"].ToString(),
                        Modelo = reader["Modelo"].ToString(),
                        Precio = (decimal)reader["Precio"],
                        CantidadInventario = (int)reader["CantidadInventario"]
                    });
                }
            }

            return lista;
        }
    }
}

