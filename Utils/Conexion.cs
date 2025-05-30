using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace FerreteriaApp.Data
{
    public class ConexionDB
    {
        private static SqlConnection _conexion;

        private ConexionDB() { }

        public static SqlConnection ObtenerConexion()
        {
            if (_conexion == null)
            {
                _conexion = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=FerreteriaDB;Trusted_Connection=True;");
            }

            if (_conexion.State == System.Data.ConnectionState.Closed)
                _conexion.Open();

            return _conexion;
        }
    }
}
