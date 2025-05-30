using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FerreteriaApp.Models;
using FerreteriaApp.Repositories;

namespace InventarioFerreteria.Forms
{
    public partial class FrmProductos : Form
    {
        private ProductoRepository _productoRepo = new ProductoRepository();
        private int productoSeleccionadoId = -1;

        public FrmProductos()
        {
            InitializeComponent();
            CargarProductos();

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Producto nuevo = new Producto
                {
                    Marca = txtMarca.Text,
                    Modelo = txtModelo.Text,
                    Precio = decimal.Parse(txtPrecio.Text),
                    CantidadInventario = int.Parse(txtCantidad.Text)
                };

                _productoRepo.AgregarProducto(nuevo);
                MessageBox.Show("Producto agregado correctamente.");
                LimpiarCampos();
                CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar: {ex.Message}");
            }
        }

        private void CargarProductos()
        {
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = _productoRepo.ObtenerTodos();
        }

        private void LimpiarCampos()
        {
            txtMarca.Clear();
            txtModelo.Clear();
            txtPrecio.Clear();
            txtCantidad.Clear();
            productoSeleccionadoId = -1;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (productoSeleccionadoId == -1)
            {
                MessageBox.Show("Selecciona un producto para editar.");
                return;
            }

            try
            {
                Producto actualizado = new Producto
                {
                    ProductoID = productoSeleccionadoId,
                    Marca = txtMarca.Text,
                    Modelo = txtModelo.Text,
                    Precio = decimal.Parse(txtPrecio.Text),
                    CantidadInventario = int.Parse(txtCantidad.Text)
                };

                _productoRepo.ActualizarProducto(actualizado);
                MessageBox.Show("Producto actualizado correctamente.");
                LimpiarCampos();
                CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar: {ex.Message}");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (productoSeleccionadoId == -1)
            {
                MessageBox.Show("Selecciona un producto para eliminar.");
                return;
            }

            try
            {
                var confirmar = MessageBox.Show("¿Seguro que deseas eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo);
                if (confirmar == DialogResult.Yes)
                {
                    _productoRepo.EliminarProducto(productoSeleccionadoId);
                    MessageBox.Show("Producto eliminado correctamente.");
                    LimpiarCampos();
                    CargarProductos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}");
            }
        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];
                productoSeleccionadoId = Convert.ToInt32(fila.Cells["ProductoID"].Value);
                txtMarca.Text = fila.Cells["Marca"].Value.ToString();
                txtModelo.Text = fila.Cells["Modelo"].Value.ToString();
                txtPrecio.Text = fila.Cells["Precio"].Value.ToString();
                txtCantidad.Text = fila.Cells["CantidadInventario"].Value.ToString();
            }
        }
    }
}

