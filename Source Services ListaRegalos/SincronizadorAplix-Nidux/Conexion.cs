using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorAplix_Nidux
{
    public class Conexion
    {
        private static SqlConnection sqlConexion = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString );
        public OracleConnection oracleConexion = new OracleConnection(ConfigurationManager.ConnectionStrings["conexionOracle"].ConnectionString);


        //Metodo para abrir la conexion con base de datos SQL
        public SqlConnection AbrirConexion()
        {
            try
            {
                if (sqlConexion.State == ConnectionState.Closed) sqlConexion.Open();

                return sqlConexion;
            }
            catch (Exception)
            {

                return sqlConexion;
            }

        }

        //Metodo para cerrar la conexion con base de datos SQL
        public SqlConnection CerrarConexion()
        {
            try
            {
                if (sqlConexion.State == ConnectionState.Open) sqlConexion.Close();

                return sqlConexion;
            }
            catch (Exception)
            {

                return sqlConexion;
            }

        }

        //Funcion para abrir la conexion con Oracle
        public OracleConnection open()
        {
            try
            {
                if (oracleConexion.State == ConnectionState.Closed) oracleConexion.Open();
                return oracleConexion;
            }
            catch (Exception)
            {
                return oracleConexion;
            }
        }

        //Funcion para abrir la conexion con Oracle
        public OracleConnection close()
        {
            try
            {
                if (oracleConexion.State == ConnectionState.Open) oracleConexion.Close();
                return oracleConexion;
            }
            catch (Exception)
            {
                return oracleConexion;
            }
        }
    }
}
