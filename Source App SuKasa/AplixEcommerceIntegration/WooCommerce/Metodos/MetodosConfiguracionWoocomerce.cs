using AplixEcommerceIntegration.Globales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.WooCommerce.Metodos
{
    public class MetodosConfiguracionWoocomerce
    {


        //Establecer la instancia con la clase Conexion
        private Conexion v_conexion = new Conexion();
        SqlDataReader v_leer;
        SqlCommand v_comando = new SqlCommand();

        //Obtener los datos de configuracion 
        public WooCommerce.Clases.ConfiguracionWoocomerce obtener_datos_configuracion()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.OBTENER_DATOS_CONFIGURACION";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_leer = v_comando.ExecuteReader();

                WooCommerce.Clases.ConfiguracionWoocomerce obj_configuracion = new WooCommerce.Clases.ConfiguracionWoocomerce();

                while (v_leer.Read())
                {

                   
                    obj_configuracion.COMPANY = v_leer.GetString(1).ToString();
                    obj_configuracion.BODEGA = v_leer.GetString(2).ToString();
                    obj_configuracion.NIVEL_PRECIO = v_leer.GetString(3).ToString();
                    obj_configuracion.VERSION = v_leer.GetInt32(4);
                    obj_configuracion.IMPUESTO = v_leer.GetString(5).ToString();
                    obj_configuracion.CLIENTE_CONTADO = v_leer.GetString(6).ToString();
                    obj_configuracion.CATEGORIA_CLIENTE = v_leer.GetString(7).ToString();
                    obj_configuracion.ACTIVIDAD_COMERCIAL = v_leer.GetString(8).ToString();
                    obj_configuracion.SINCRONIZA_DESCUENTOS = v_leer.GetString(9).ToString();
                    obj_configuracion.ARTICULO_FLETE = v_leer.GetString(10).ToString();
                    obj_configuracion.CLAVE_API = v_leer.GetString(11).ToString();
                    obj_configuracion.PASS_API = v_leer.GetString(12).ToString();
                    obj_configuracion.TIENDA = v_leer.GetString(13).ToString();

                }

                v_conexion.CerrarConexion();

                return obj_configuracion;
            }
            catch (Exception)
            {
                throw;
            }
        }





    }

}
