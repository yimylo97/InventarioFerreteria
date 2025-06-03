using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using InventarioFerreteria.Models;
using InventarioFerreteria.Data;
using System.Data;

namespace InventarioFerreteria.UI
{
    public partial class FormVentas : Form
    {
        private List<DetalleVenta> carrito = new List<DetalleVenta>();

        public FormVentas()
        {
            InitializeComponent();
            CargarProductosComboBox();
            CargarProductosDisponibles();
        }

        private void CargarProductosComboBox()
        {
            try
            {
                using (SqlConnection conexion = ConexionDB.ObtenerConexion())
                {
                    string query = "SELECT ProductoID, Marca + ' - ' + Modelo AS Nombre FROM Productos";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, string> productos = new Dictionary<int, string>();
                    while (reader.Read())
                    {
                        productos.Add(reader.GetInt32(0), reader.GetString(1));
                    }

                    cboProductos.DataSource = new BindingSource(productos, null);
                    cboProductos.DisplayMember = "Value";
                    cboProductos.ValueMember = "Key";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboProductos.SelectedItem == null)
                {
                    MessageBox.Show("Selecciona un producto.");
                    return;
                }

                int productoId = ((KeyValuePair<int, string>)cboProductos.SelectedItem).Key;
                int cantidad = (int)nudCantidad.Value;

                if (cantidad <= 0)
                {
                    MessageBox.Show("La cantidad debe ser mayor a 0.");
                    return;
                }

                using (SqlConnection conexion = ConexionDB.ObtenerConexion())
                {
                    string query = "SELECT Marca, Modelo, Precio, CantidadInventario FROM Productos WHERE ProductoID = @id";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@id", productoId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string marca = reader.GetString(0);
                        string modelo = reader.GetString(1);
                        decimal precio = reader.GetDecimal(2);
                        int stockDisponible = reader.GetInt32(3);

                        if (cantidad > stockDisponible)
                        {
                            MessageBox.Show("No hay suficiente stock disponible.");
                            return;
                        }

                        var existente = carrito.FirstOrDefault(p => p.ProductoId == productoId);
                        if (existente != null)
                        {
                            existente.Cantidad += cantidad;
                        }
                        else
                        {
                            carrito.Add(new DetalleVenta
                            {
                                ProductoId = productoId,
                                Marca = marca,
                                Modelo = modelo,
                                Precio = precio,
                                Cantidad = cantidad
                            });
                        }

                        ActualizarVistaCarrito();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar producto al carrito: " + ex.Message);
            }
        }

        private void btnEliminarDelCarrito_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.SelectedRows.Count > 0)
            {
                var fila = dgvCarrito.SelectedRows[0];
                int productoId = (int)fila.Cells["ProductoId"].Value;

                var item = carrito.FirstOrDefault(p => p.ProductoId == productoId);
                if (item != null)
                {
                    var confirm = MessageBox.Show($"¿Eliminar {item.Marca} - {item.Modelo} del carrito?", "Confirmar eliminación", MessageBoxButtons.YesNo);
                    if (confirm == DialogResult.Yes)
                    {
                        carrito.Remove(item);
                        ActualizarVistaCarrito();
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un producto del carrito para eliminar.");
            }
        }

        private void btnFinalizarVenta_Click(object sender, EventArgs e)
        {
            if (carrito.Count == 0)
            {
                MessageBox.Show("El carrito está vacío.");
                return;
            }

            var confirmar = MessageBox.Show("¿Deseas finalizar esta venta?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conexion = ConexionDB.ObtenerConexion())
                {
                    SqlTransaction transaccion = conexion.BeginTransaction();

                    try
                    {
                        string queryVenta = "INSERT INTO Ventas (Fecha) OUTPUT INSERTED.VentaID VALUES (@fecha)";
                        SqlCommand cmdVenta = new SqlCommand(queryVenta, conexion, transaccion);
                        cmdVenta.Parameters.AddWithValue("@fecha", DateTime.Now);
                        int ventaId = (int)cmdVenta.ExecuteScalar();

                        decimal totalVenta = 0;

                        foreach (var item in carrito)
                        {
                            // Insertar detalle
                            string queryDetalle = @"INSERT INTO DetalleVentas (VentaID, ProductoID, Cantidad, PrecioUnitario)
                                            VALUES (@ventaId, @productoId, @cantidad, @precio)";
                            SqlCommand cmdDetalle = new SqlCommand(queryDetalle, conexion, transaccion);
                            cmdDetalle.Parameters.AddWithValue("@ventaId", ventaId);
                            cmdDetalle.Parameters.AddWithValue("@productoId", item.ProductoId);
                            cmdDetalle.Parameters.AddWithValue("@cantidad", item.Cantidad);
                            cmdDetalle.Parameters.AddWithValue("@precio", item.Precio);
                            cmdDetalle.ExecuteNonQuery();

                            // Actualizar stock
                            string queryStock = "UPDATE Productos SET CantidadInventario = CantidadInventario - @cantidad WHERE ProductoID = @productoId";
                            SqlCommand cmdStock = new SqlCommand(queryStock, conexion, transaccion);
                            cmdStock.Parameters.AddWithValue("@cantidad", item.Cantidad);
                            cmdStock.Parameters.AddWithValue("@productoId", item.ProductoId);
                            cmdStock.ExecuteNonQuery();

                            totalVenta += item.Cantidad * item.Precio;
                        }

                        transaccion.Commit();
                        MessageBox.Show($"¡Venta registrada exitosamente!\nTotal vendido: ${totalVenta:0.00}");

                        carrito.Clear();
                        ActualizarVistaCarrito();
                        CargarProductosComboBox();
                        nudCantidad.Value = 1;
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback();
                        MessageBox.Show("Error al guardar la venta: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error general al finalizar la venta: " + ex.Message);
            }
        }

        private void CargarProductosDisponibles()
        {
            try
            {
                using (SqlConnection conexion = ConexionDB.ObtenerConexion())
                {
                    string query = "SELECT ProductoID, Marca, Modelo, Precio, CantidadInventario FROM Productos WHERE CantidadInventario > 0";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);
                    dgvProductosDisponibles.DataSource = tabla;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos disponibles: " + ex.Message);
            }
        }


        private void ActualizarVistaCarrito()
        {
            dgvCarrito.DataSource = null;
            dgvCarrito.DataSource = carrito.Select(c => new
            {
                c.ProductoId,
                c.Marca,
                c.Modelo,
                Precio = c.Precio.ToString("C2"),
                c.Cantidad,
                Subtotal = (c.Precio * c.Cantidad).ToString("C2")
            }).ToList();

            if (dgvCarrito.Columns["ProductoId"] != null)
                dgvCarrito.Columns["ProductoId"].HeaderText = "ID";

            if (dgvCarrito.Columns["Marca"] != null)
                dgvCarrito.Columns["Marca"].HeaderText = "Marca";

            if (dgvCarrito.Columns["Modelo"] != null)
                dgvCarrito.Columns["Modelo"].HeaderText = "Modelo";

            if (dgvCarrito.Columns["Precio"] != null)
                dgvCarrito.Columns["Precio"].HeaderText = "Precio Unitario";

            if (dgvCarrito.Columns["Cantidad"] != null)
                dgvCarrito.Columns["Cantidad"].HeaderText = "Cantidad";

            if (dgvCarrito.Columns["Subtotal"] != null)
                dgvCarrito.Columns["Subtotal"].HeaderText = "Subtotal";
        }

        private void btnAgregarAlCarrito_Click(object sender, EventArgs e)
        {

        }
    }
}