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

  
    public class MetodosPedidosWoocomerce
    {

        public string ip = ConfigurationSettings.AppSettings["Ip"].ToString();

        //Establecer la instancia con la clase Conexion
        private Conexion v_conexion = new Globales.Conexion();
        SqlDataReader v_leer;
        SqlCommand v_comando = new SqlCommand();
        DataTable v_tabla = new DataTable();

        //Mostramos los encabezados de pedidos
        public DataTable mostrar_pedidos()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_PEDIDOS";
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

        //Mostramos las lineas del pedidos
        public DataTable mostrar_lineas_pedidos(Clases.PedidosWoocomerce obj_peiddo)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_LINEAS_PEDIDOS";
                v_comando.Parameters.AddWithValue("@PEDIDOS", ((object)obj_peiddo.id.ToString()) ?? DBNull.Value);
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

        //Mostramos las envios del pedidos
        public DataTable mostrar_envios_pedidos(Clases.PedidosWoocomerce obj_peiddo)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_LINEAS_METODOS";
                v_comando.Parameters.AddWithValue("@PEDIDOS", ((object)obj_peiddo.id.ToString()) ?? DBNull.Value);
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

        //Bucar pedidos
        public DataTable buscar_pedidos(WooCommerce.Clases.PedidosBusquedaWoocomerce obj_pedido)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.BUSCAR_PEDIDOS";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@DATO", ((object)obj_pedido.dato.ToString()) ?? DBNull.Value);
                v_comando.Parameters.AddWithValue("@PARAMETRO", ((object)obj_pedido.parametro.ToString()) ?? DBNull.Value);
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

        //Bucar pedidos por fecha
        public DataTable buscar_pedidos_fecha(WooCommerce.Clases.PedidosBusquedaFechaWoocomerce obj_pedido)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.BUSCAR_POR_FECHAS";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@FECHA_INI", ((object)obj_pedido.fecha_ini.ToString()) ?? DBNull.Value);
                v_comando.Parameters.AddWithValue("@FECHA_FIN", ((object)obj_pedido.fecha_fin.ToString()) ?? DBNull.Value);
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

        //Enviar pedidos al ERP
        public void sincronizador_pedidos_al_ERP()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.AGREGAR_PEDIOS_SISTEMA";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_leer = v_comando.ExecuteReader();
                v_tabla.Load(v_leer);
                v_conexion.CerrarConexion();

            }
            catch (Exception)
            {
                throw;
            }
        }


        //******************************************************* PEDIDOS  *********************************************************** //

        //obtener los pedidos de woocomerce en estado:     
        public string obtener_pedidos_woocoemrce()
        {

            string json = "";

            try
            {
                Metodos.MetodosConfiguracionWoocomerce met_configuracion = new Metodos.MetodosConfiguracionWoocomerce();
                string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos                                                                                         

                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/orders?status=processing");
                RestRequest restRequest = new RestRequest(Method.GET);
                restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                IRestResponse restResponse = restClient.Execute(restRequest);


                //Si Woocomerce responde entoces que continue el proceso
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
        public string insertar_pedidos_woocomerce()
        {

            string mensaje = "";

            int i = 0;

            try
            {

                string json = obtener_pedidos_woocoemrce();

                MetodosConfiguracionWoocomerce met_configuracion = new MetodosConfiguracionWoocomerce();
                string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos

                //Enviamos la clase creada a nuestro api para que los inserte o actualice en nuestra tablas propias de woocomerce
                RestClient restClient3 = new RestClient();
                restClient3.BaseUrl = new Uri("http://" + ip + "/api/insertar_pedidos");
                RestRequest restRequest3 = new RestRequest(Method.POST);
                restRequest3.AddHeader("Content-Type", "application/json");
                restRequest3.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse restResponse3 = restClient3.Execute(restRequest3);

                if ((int)restResponse3.StatusCode == 200)
                {
                   mensaje = "Pedidos ingresados con éxito";
                }
                else
                {

                }

                return mensaje;

            }
            catch (Exception ex)
            {
                return mensaje + " " + ex.Message.ToString();
            }

        }

        //Obtener los pedidos editados recientemente 
        public string obtener_pedidos_editados_recientemete()
        {

            string json = "";

            try
            {

                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_obtener_pedidos_editados");
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

        //Actualizar el estado de los pedidos   
        public string actualizar_pedidos_woocoemrce()
        {

            string json = "";

            try
            {

                json = obtener_pedidos_editados_recientemete();

                //Convierte a clase los pedios para actualizar esos pedidos
                List<Clases.PedidosEstado> lista_json = JsonConvert.DeserializeObject<List<Clases.PedidosEstado>>(json);

                if (lista_json.Count > 0)
                {

                    int i = 0;

                    while (i < lista_json.Count) {

                        Metodos.MetodosConfiguracionWoocomerce met_configuracion = new Metodos.MetodosConfiguracionWoocomerce();
                        string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                        string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                        string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos                                                                                         
                        string json_status =  JsonConvert.SerializeObject(lista_json[i]);
                        RestClient restClient = new RestClient();
                        restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/orders/" + lista_json[i].id.ToString());
                        RestRequest restRequest = new RestRequest(Method.PUT);
                        restRequest.AddHeader("Content-Type", "application/json");
                        restRequest.AddParameter("application/json", json_status, ParameterType.RequestBody);
                        restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                        //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                        IRestResponse restResponse = restClient.Execute(restRequest);

                        //Si Woocomerce responde entoces que continue el proceso
                        if ((int)restResponse.StatusCode == 200)
                        {
                            json = restResponse.Content;
                        }

                        i++;
                    }

                }

              
                return json = "Estado de pedidos actualizados con éxito";
            }
            catch (Exception)
            {

                throw;
            }

        }


        //******************************************************* PEDIDOS *********************************************************** //



    }


    }
