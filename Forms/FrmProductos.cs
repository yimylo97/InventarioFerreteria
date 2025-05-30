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
        }
    }
}

