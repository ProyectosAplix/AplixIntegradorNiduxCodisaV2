using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplixEcommerceIntegration.Globales;
using System.Data;
using System.Data.SqlClient;
using AplixEcommerceIntegration.Shopify.Clases;


namespace AplixEcommerceIntegration.Shopify.Metodos
{
    class Metodo_Credenciales
    {
        //Conexion a base de datos
        Conexion cnn = new Conexion();
        public List<Credenciales> obtener_credenciales()
        {
            var lista_credenciales = new List<Credenciales>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT TOP(1) URL_TIENDA AS TIENDA, CLAVE_API AS CLAVE, PASSWORD_API AS PASS FROM SHOPIFY.CREDENCIALES";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataReader da = cmd.ExecuteReader();
                while (da.Read())
                {
                    Credenciales cre = new Credenciales();
                    cre.url_tienda = da["TIENDA"].ToString();
                    cre.calve_api = da["CLAVE"].ToString();
                    cre.password_api = da["PASS"].ToString();
                    lista_credenciales.Add(cre);

                }
                cnn.CerrarConexion();
                return lista_credenciales;
            }
            catch (Exception)
            {
                lista_credenciales = null;
                cnn.CerrarConexion();
                return lista_credenciales;
            }
        }

    }
}
