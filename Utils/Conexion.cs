using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventarioFerreteria.Data
{

    public class ConexionDB
    {
        private static SqlConnection _conexion;

        private ConexionDB() { }

        public static SqlConnection ObtenerConexion()
        {
            if (_conexion == null)
            {
                var conexion = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=FerreteriaDB;Trusted_Connection=True;");
                conexion.Open();
                return conexion;
            }

            if (_conexion.State == System.Data.ConnectionState.Closed)
            {
                _conexion.Open();
            }

            return _conexion;

            
        }

    }

}
