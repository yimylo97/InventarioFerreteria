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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            var repo = new UsuarioRepository();
            var usuario = repo.ValidarLogin(txtUsuario.Text.Trim(), txtContrasena.Text.Trim());

            if (usuario != null)
            {
                MessageBox.Show($"Bienvenido, {usuario.NombreUsuario} ({usuario.Rol})");

                this.Hide();

                if (usuario.Rol == "Administrador")
                {
                    var frm = new FrmProductos();
                    frm.Show();
                }
                else
                {
                    var frm = new FrmVentas();
                    frm.Show();
                }
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
