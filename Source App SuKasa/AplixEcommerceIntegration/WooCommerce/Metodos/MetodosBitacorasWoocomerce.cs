using AplixEcommerceIntegration.Globales;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.WooCommerce.Metodos
{
    public class MetodosBitacorasWoocomerce
    {
        public string ip = ConfigurationSettings.AppSettings["Ip"].ToString();

        //Establecer la instancia con la clase Conexion
        private Conexion v_conexion = new Globales.Conexion();
        SqlDataReader v_leer;
        SqlCommand v_comando = new SqlCommand();
        DataTable v_tabla = new DataTable();

        //Mostramos los encabezados de pedidos
        public DataTable mostrar_bitacoras()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_BITACORA";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_leer = v_comando.ExecuteReader();
                v_tabla.Load(v_leer);
                v_conexion.CerrarConexion();
                return v_tabla;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Mostrar los modulos de bitacora
        public DataTable mostrar_modulos_bitacora()
        {
            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_BITACORA_MODULOS";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_leer = v_comando.ExecuteReader();
                v_tabla.Load(v_leer);
                v_conexion.CerrarConexion();
                return v_tabla;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Mostrar error por modulo
        public DataTable buscar_bitacora_modulo(WooCommerce.Clases.BitacorasWoocomerce obj_bitacora)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_BITACORA_POR_MODULO";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@MODULO", ((object)obj_bitacora.modulo.ToString()) ?? DBNull.Value);
                v_leer = v_comando.ExecuteReader();
                v_tabla.Load(v_leer);
                v_conexion.CerrarConexion();
                return v_tabla;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Mostrar errores por fechas
        public DataTable buscar_bitacora_por_fechas(WooCommerce.Clases.BitacorasFechasWoocomerce obj_bitacora)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_BITACORA_POR_FECHAS";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@FECHA_INI", ((object)obj_bitacora.fecha_ini.ToString()) ?? DBNull.Value);
                v_comando.Parameters.AddWithValue("@FECHA_FIN", ((object)obj_bitacora.fecha_fin.ToString()) ?? DBNull.Value);
                v_leer = v_comando.ExecuteReader();
                v_tabla.Load(v_leer);
                v_conexion.CerrarConexion();
                return v_tabla;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Eliminar metodos de envio     
        public void eliminar_bitacora_errores()
        {
            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.LIMPIAR_BITACORA_ERRORES";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //***************************************************  API ****************************************************************




        //***************************************************  API ****************************************************************

    }


}
