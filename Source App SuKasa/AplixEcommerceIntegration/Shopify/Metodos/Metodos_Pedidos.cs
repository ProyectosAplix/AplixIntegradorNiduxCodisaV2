using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplixEcommerceIntegration.Globales;

namespace AplixEcommerceIntegration.Shopify.Metodos
{
    class Metodos_Pedidos
    {
        //Conexion a base de datos
        Conexion cnn = new Conexion();

        //Métodos que llenan la tabla de los pedidos
        public DataTable pedidos()
        {
            DataTable dt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.MOSTRAR_PEDIDOS";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {

                    da.Fill(dt);
                    cnn.CerrarConexion();
                    return dt;
                }
            }
            catch (Exception)
            {
                dt = null;
                cnn.CerrarConexion();
                return dt;
            }
        }

        //Métodos que llenan la tabla de los pedidos linea
        public DataTable pedidoLinea(string orden)
        {
            DataTable dt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.MOSTRAR_PEDIDOS_LINEA";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ORDEN", Convert.ToInt32(orden));
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {

                    da.Fill(dt);
                    cnn.CerrarConexion();
                    return dt;
                }
            }
            catch (Exception)
            {
                dt = null;
                cnn.CerrarConexion();
                return dt;
            }
        }

    }
}
