using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InventarioFerreteria.Data;
using InventarioFerreteria.Models;

namespace InventarioFerreteria.Repositories
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

        public void ActualizarProducto(Producto producto)
        {
            using (SqlConnection conexion = ConexionDB.ObtenerConexion())
            {
                string query = "UPDATE Productos SET Marca = @Marca, Modelo = @Modelo, Precio = @Precio, CantidadInventario = @Cantidad WHERE ProductoID = @ProductoID";
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Marca", producto.Marca);
                    comando.Parameters.AddWithValue("@Modelo", producto.Modelo);
                    comando.Parameters.AddWithValue("@Precio", producto.Precio);
                    comando.Parameters.AddWithValue("@Cantidad", producto.CantidadInventario);
                    comando.Parameters.AddWithValue("@ProductoID", producto.ProductoID);

                    comando.ExecuteNonQuery();
                }
            }
        }

        public void EliminarProducto(int productoId)
        {
            using (SqlConnection conexion = ConexionDB.ObtenerConexion())
            {
                string query = "DELETE FROM Productos WHERE ProductoID = @ProductoID";
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@ProductoID", productoId);
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}

