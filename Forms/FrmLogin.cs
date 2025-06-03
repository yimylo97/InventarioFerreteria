using InventarioFerreteria.Repositories;
using InventarioFerreteria.UI;
using System.Windows.Forms;
using System;
using InventarioFerreteria.Models;


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
            string nombre = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(contrasena))
            {
                MessageBox.Show("Por favor, ingresa el nombre de usuario y la contraseña.",
                                "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var repo = new UsuarioRepository();
                var usuario = repo.ValidarLogin(nombre, contrasena);

                if (usuario != null)
                {
                    MessageBox.Show($"Bienvenido, {usuario.NombreUsuario} ({usuario.Rol})",
                                    "Acceso concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();

                    if (usuario.Rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                    {
                        new FrmProductos().Show();
                    }
                    else
                    {
                        new FormVentas().Show();
                    }
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.",
                                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtContrasena.Clear();
                    txtUsuario.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error durante el inicio de sesión: " + ex.Message,
                                "Error del sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

