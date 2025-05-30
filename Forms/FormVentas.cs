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
    public partial class FormVentas : Form
    {
        private List<DetalleVenta> carrito = new List<DetalleVenta>();
        private decimal totalVenta = 0;


        public FormVentas()
        {
            InitializeComponent();
        }

        private void lblCarrito_Click(object sender, EventArgs e)
        {

        }
    }
}
