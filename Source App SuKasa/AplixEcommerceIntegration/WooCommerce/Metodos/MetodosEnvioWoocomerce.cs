using AplixEcommerceIntegration.Globales;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.WooCommerce.Metodos
{
    public  class MetodosEnvioWoocomerce
    {

        public string ip = ConfigurationSettings.AppSettings["Ip"].ToString();

        //Establecer la instancia con la clase Conexion
        private Conexion v_conexion = new Globales.Conexion();
        SqlDataReader v_leer;
        SqlCommand v_comando = new SqlCommand();
        DataTable v_tabla = new DataTable();


        //Obtener las zonas de envio 
        public List<Clases.ZonasWoocoemrce> obtener_zonas_de_woocomerce()
        {

            List<Clases.ZonasWoocoemrce> lista_zonas = new List<Clases.ZonasWoocoemrce>();

            try
            {
                Metodos.MetodosConfiguracionWoocomerce met_configuracion = new Metodos.MetodosConfiguracionWoocomerce();
                string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos

                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/shipping/zones");
                RestRequest restRequest = new RestRequest(Method.GET);
                restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                IRestResponse restResponse = restClient.Execute(restRequest);

                if ((int)restResponse.StatusCode == 200)
                {

                    lista_zonas = JsonConvert.DeserializeObject<List<Clases.ZonasWoocoemrce>>(restResponse.Content);

                }

                return lista_zonas;
            }
            catch (Exception)
            {
                throw;
            }

        }

        //Obtener los metodos de envio 
        public List<Clases.MetodosEnvioWoocoemrce> obtener_metodos_envio_de_woocomerce( string id_zona)
        {

            List<Clases.MetodosEnvioWoocoemrce> lista_zonas = new List<Clases.MetodosEnvioWoocoemrce>();

            try
            {
                Metodos.MetodosConfiguracionWoocomerce met_configuracion = new Metodos.MetodosConfiguracionWoocomerce();
                string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos

                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/shipping/zones/" + id_zona+ "/methods");
                RestRequest restRequest = new RestRequest(Method.GET);
                restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                IRestResponse restResponse = restClient.Execute(restRequest);

                if ((int)restResponse.StatusCode == 200)
                {

                    lista_zonas = JsonConvert.DeserializeObject<List<Clases.MetodosEnvioWoocoemrce>>(restResponse.Content);

                }

                return lista_zonas;
            }
            catch (Exception)
            {
                throw;
            }

        }

        //Insertar un nuevo metodo de envio      
        public void registrar_metodo_envio (Clases.EnvioWoocomerce obj_envio)
        {
            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.AGREGAR_METODO_PAGO";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@ID_METODO_PAGO", obj_envio.id_metodo_pago);
                v_comando.Parameters.AddWithValue("@DESCRIPCION_METODO", obj_envio.metodo_pago);
                v_comando.Parameters.AddWithValue("@ARTICULO", obj_envio.sku);
                v_comando.Parameters.AddWithValue("@PRECIO", obj_envio.precio);
                v_comando.Parameters.AddWithValue("@ID_ZONA", obj_envio.zona);
                v_comando.Parameters.AddWithValue("@DESCRIPCION_ZONA", obj_envio.nombre_zona);

                v_comando.ExecuteNonQuery();

              
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Mostrar los metodos pago
        public DataTable mostrar_metodos_envio()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_METODOS_PAGO";
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

        //Mostrar articulos de envio
        public DataTable mostrar_articulos_envio()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_ARTICULOS_PARA_ENVIO";
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

        //Mostrar articulos busqueda de envio
        public DataTable mostrar_articulos_envio_busqueda( Clases.EnvioBusqueda obj_envio)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.BUSCAR_ARTICULOS_ENVIO";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@DATO", ((object)obj_envio.dato.ToString()) ?? DBNull.Value);
                v_comando.Parameters.AddWithValue("@PARAMETRO", ((object)obj_envio.parametro.ToString()) ?? DBNull.Value);
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

        //Actualizar el metodo de envio       
        public void actualizar_metodo_envio(Clases.EnvioWoocomerce obj_envio)
        {
            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE_ACTUALIZAR_ESTADO_METODO_ENVIO";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@ID_METODO_PAGO", obj_envio.id_metodo_pago);               
                v_comando.Parameters.AddWithValue("@ARTICULO", obj_envio.sku);             
                v_comando.Parameters.AddWithValue("@ID_ZONA", obj_envio.zona);
                v_comando.Parameters.AddWithValue("@ESTADO", obj_envio.estado);
                v_comando.ExecuteNonQuery();


            }
            catch (Exception)
            {
                throw;
            }
        }

        //Eliminar metodos de envio     
        public void eliminar_metodo_envio()
        {
            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE_ELIMINAR_METODO_ENVIO";
                v_comando.CommandType = CommandType.StoredProcedure;              
                v_comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }


        //***************************************************** API *******************************************************

        //Obtener los metodos de envio editados recientemente 
        public string obtener_metodos_envio_editados_recientemete()
        {

            string json = "";

            try
            {

                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_obtener_metodos_envio_editados");
                RestRequest restRequest = new RestRequest(Method.GET);
                IRestResponse restResponse = restClient.Execute(restRequest);
                if ((int)restResponse.StatusCode == 200)
                {
                    json = restResponse.Content;
                }

                return json;
            }
            catch (Exception)
            {
                throw;
            }

        }

        //Actualizar el precio de un metodo de envio
        public string actualizar_precio_envios_woocomerce()
        {

            string mensaje = "";

            int i = 0;

            try
            {

                    MetodosConfiguracionWoocomerce met_configuracion = new MetodosConfiguracionWoocomerce();
                    string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                    string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                    string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos

                    List<Clases.EnvioWoocomerce> lista_envios = new List<Clases.EnvioWoocomerce>();

                    lista_envios = JsonConvert.DeserializeObject<List<Clases.EnvioWoocomerce>>(obtener_metodos_envio_editados_recientemete());

                    if (lista_envios.Count > 0) {

                        while (i < lista_envios.Count) {


                            Clases.MetodosEnvioActWoocoemrce obj_se = new Clases.MetodosEnvioActWoocoemrce();

                            Clases.SettingsWoocomerce obj = new Clases.SettingsWoocomerce();
                            obj_se.settings = obj;
                            obj.cost = lista_envios[i].precio.ToString();

                            //Convertimos el atributo en un nuevo Json para agregar el atributo
                            string json = JsonConvert.SerializeObject(obj_se);

                            RestClient restClient2 = new RestClient();
                            restClient2.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/shipping/zones/" + lista_envios[i].zona.ToString() + "/methods/" + lista_envios[i].id_metodo_pago.ToString());
                            RestRequest restRequest2 = new RestRequest(Method.PUT);
                            restClient2.Authenticator = new HttpBasicAuthenticator(clave, password);
                            //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                            restRequest2.AddHeader("Content-Type", "application/json");
                            restRequest2.AddParameter("application/json", json, ParameterType.RequestBody);
                            IRestResponse restResponse2 = restClient2.Execute(restRequest2);

                            if ((int)restResponse2.StatusCode == 200)
                            {
                                 mensaje = "Costos de envío actualizados con éxito";
                            }
                            else {

                            }

                            i++;
                        }

                    }

              
                return mensaje;

            }
            catch (Exception ex)
            {
                return mensaje + " " + ex.Message.ToString();
            }

        }

        //***************************************************** API *******************************************************


    }

}
