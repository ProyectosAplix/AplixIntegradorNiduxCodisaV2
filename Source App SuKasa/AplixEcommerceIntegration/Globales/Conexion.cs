using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.OracleClient;
using System.Windows.Forms;

namespace AplixEcommerceIntegration.Globales
{
    public class Conexion
    {
        //String de Conexion
        private SqlConnection v_Conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        public OracleConnection o_Conexion = new OracleConnection(ConfigurationManager.ConnectionStrings["conexionOracle"].ConnectionString);

        //Metodo para abrir la conexion con base de datos
        public SqlConnection AbrirConexion()
        {
            //MessageBox.Show($"SQL {ConfigurationManager.ConnectionStrings["conexion"].ConnectionString} \n ORACLE {ConfigurationManager.ConnectionStrings["conexionOracle"].ConnectionString} ");

            try
            {
                if (v_Conexion.State == ConnectionState.Closed) v_Conexion.Open();
                return v_Conexion;
            }
            catch (Exception)
            {
                return v_Conexion;
            }
        }
        //Metodo para cerrar la conexion con base de datos
        public SqlConnection CerrarConexion()
        {
            try
            {
                if (v_Conexion.State == ConnectionState.Open) v_Conexion.Close();
                return v_Conexion;
            }
            catch (Exception)
            {
                return v_Conexion;
            }
        }

        //Funcion para abrir la conexion con Oracle
        public OracleConnection open()
        {
            try
            {
                if (o_Conexion.State == ConnectionState.Closed) o_Conexion.Open();
                return o_Conexion;
            }
            catch (Exception)
            {
                return o_Conexion;
            }
        }

        //Funcion para abrir la conexion con Oracle
        public OracleConnection close()
        {
            try
            {
                if (o_Conexion.State == ConnectionState.Open) o_Conexion.Close();
                return o_Conexion;
            }
            catch (Exception)
            {
                return o_Conexion;
            }
        }
    }
}
