using FerreteriaApp.Repositories;
using InventarioFerreteria.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventarioFerreteria.Forms
{
    public partial class FrmProveedores : Form
    {
        private readonly ProveedorRepository proveedorRepo = new ProveedorRepository();
        public FrmProveedores()
        {
            InitializeComponent();
            CargarProveedores();
        }

        private void CargarProveedores()
        {
            dgvProveedores.DataSource = null;
            dgvProveedores.DataSource = proveedorRepo.ObtenerTodos();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                var proveedor = new Proveedor
                {
                    Nombre = txtNombre.Text,
                    Contacto = txtContacto.Text,
                    ProductoID = int.Parse(txtProductoID.Text)
                };

                proveedorRepo.Agregar(proveedor);
                MessageBox.Show("Proveedor agregado exitosamente.");
                Limpiar();
                CargarProveedores();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar: {ex.Message}");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProveedores.CurrentRow != null)
            {
                try
                {
                    var proveedor = new Proveedor
                    {
                        ProveedorID = (int)dgvProveedores.CurrentRow.Cells["ProveedorID"].Value,
                        Nombre = txtNombre.Text,
                        Contacto = txtContacto.Text,
                        ProductoID = int.Parse(txtProductoID.Text)
                    };

                    proveedorRepo.Actualizar(proveedor);
                    MessageBox.Show("Proveedor actualizado.");
                    Limpiar();
                    CargarProveedores();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar: {ex.Message}");
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProveedores.CurrentRow != null)
            {
                try
                {
                    int id = (int)dgvProveedores.CurrentRow.Cells["ProveedorID"].Value;
                    proveedorRepo.Eliminar(id);
                    MessageBox.Show("Proveedor eliminado.");
                    Limpiar();
                    CargarProveedores();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar: {ex.Message}");
                }
            }
        }

        private void dgvProveedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProveedores.CurrentRow != null)
            {
                txtNombre.Text = dgvProveedores.CurrentRow.Cells["Nombre"].Value.ToString();
                txtContacto.Text = dgvProveedores.CurrentRow.Cells["Contacto"].Value.ToString();
                txtProductoID.Text = dgvProveedores.CurrentRow.Cells["ProductoID"].Value.ToString();
            }
        }

        private void Limpiar()
        {
            txtNombre.Clear();
            txtContacto.Clear();
            txtProductoID.Clear();
            dgvProveedores.ClearSelection();
        }
    }
}
