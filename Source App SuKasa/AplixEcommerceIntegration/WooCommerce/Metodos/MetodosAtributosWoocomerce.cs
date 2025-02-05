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

    public class MetodosAtributosWoocomerce
    {

        public string ip = ConfigurationSettings.AppSettings["Ip"].ToString();

        //Establecer la instancia con la clase Conexion
        private Conexion v_conexion = new Globales.Conexion();
        SqlDataReader v_leer;
        SqlCommand v_comando = new SqlCommand();
        DataTable v_tabla = new DataTable();

        //Mostrar los atributos
        public DataTable mostrar_atributos()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_ATRIBUTOS";
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

        //metodo para validar si un  atributo ya existe o no
        public string validar_nombre_atributo(Clases.AtributosWoocomerce obj_atributo)
        {
            string mensaje = "";

            try
            {


                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.VALIDAR_NOMBRE_ATRIBUTO";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@NOMBRE", obj_atributo.name);
                v_leer = v_comando.ExecuteReader();
                v_leer.Read();
                mensaje = v_leer.GetString(v_leer.GetOrdinal("MENSAJE"));
                v_conexion.CerrarConexion();

                return mensaje;
            }
            catch (Exception)
            {
                throw;
            }

        }

        //Limpiar lista de atributos
        public void limpiar_atributos() {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.BORRAR_ATRIBUTOS";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.ExecuteNonQuery();
                v_conexion.CerrarConexion();

            }
            catch (Exception)
            {
                throw;
            }

        }

        //Bucar atributos
        public DataTable buscar_atributos(WooCommerce.Clases.BusquedaAtributos obj_atributos)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.BUSCAR_ATRIBUTOS";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@DATO", ((object)obj_atributos.dato.ToString()) ?? DBNull.Value);
                v_comando.Parameters.AddWithValue("@PARAMETRO", ((object)obj_atributos.parametro.ToString()) ?? DBNull.Value);
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





        //*********** WOOCOMERCE ***************************************

        //Agregar nuevo atributo
        public string agregar_nuevo_atributo(Clases.AtributosWoocomerce obj_atributos)
        {

            string mensaje = "";

            try
            {
                Metodos.MetodosConfiguracionWoocomerce met_configuracion = new Metodos.MetodosConfiguracionWoocomerce();
                string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos
               
                Clases.AtributosWoocomerce res_obj_atributos = new Clases.AtributosWoocomerce();

                //Convertimos el atributo en un nuevo Json para agregar el atributo
                string json_obj_atributos = JsonConvert.SerializeObject(obj_atributos);

                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/attributes");
                RestRequest restRequest = new RestRequest(Method.POST);
                restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                restRequest.AddHeader("Content-Type", "application/json");
                restRequest.AddParameter("application/json", json_obj_atributos, ParameterType.RequestBody);
                IRestResponse restResponse = restClient.Execute(restRequest);

                if ((int)restResponse.StatusCode == 201)
                {

                    res_obj_atributos = JsonConvert.DeserializeObject<Clases.AtributosWoocomerce>(restResponse.Content);
                    List<Clases.AtributosWoocomerce> lista_obj_atributos = new List<Clases.AtributosWoocomerce>();
                    lista_obj_atributos.Add(res_obj_atributos);
                    string lista_json = JsonConvert.SerializeObject(lista_obj_atributos);

                    RestClient restClient3 = new RestClient();
                    restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomcerce_insertar_atributos");
                    RestRequest restRequest3 = new RestRequest(Method.POST);
                    restRequest3.AddHeader("Content-Type", "application/json");
                    restRequest3.AddParameter("application/json", lista_json, ParameterType.RequestBody);
                    IRestResponse restResponse3 = restClient3.Execute(restRequest3);


                    mensaje = "Atributo " + obj_atributos.name + " agregado exitosamente con el código: " + res_obj_atributos.id.ToString();

                }
                else
                {
                    mensaje = "Error al agregar el atributo: " + obj_atributos.name + " error: " + restResponse.Content;
                }

                return mensaje;
            }
            catch (Exception ex)
            {
                return  mensaje + ex.Message.ToString();
            }

           

        }

        //Obtener los atributos de woocomerce
        public string obtener_atributos_de_woocomerce()
        {

            string json = "";

            try
            {
                Metodos.MetodosConfiguracionWoocomerce met_configuracion = new Metodos.MetodosConfiguracionWoocomerce();
                string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos
                                                                                            
                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/attributes");
                RestRequest restRequest = new RestRequest(Method.GET);
                restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
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

        //Insertar atributos de woocomerce a nuestra tienda
        public string insertar_atributos(){

            string mensaje = "";

            try
            {
                //Lista con todos los atributps de la tienda
                List<Clases.AtributosWoocomerce> lista_json = JsonConvert.DeserializeObject<List<Clases.AtributosWoocomerce>>(obtener_atributos_de_woocomerce());

                //Validamos que esta lista venga llena
                if (lista_json.Count > 0)
                {
                    //Convertimos el atributo en un nuevo Json para agregar el atributo
                    string json_obj_atributos = JsonConvert.SerializeObject(lista_json);

                    RestClient restClient3 = new RestClient();
                    restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomcerce_insertar_atributos");
                    RestRequest restRequest3 = new RestRequest(Method.POST);
                    restRequest3.AddHeader("Content-Type", "application/json");
                    restRequest3.AddParameter("application/json", json_obj_atributos, ParameterType.RequestBody);
                    IRestResponse restResponse3 = restClient3.Execute(restRequest3);

                    if ((int)restResponse3.StatusCode == 200)
                    {
                        mensaje = restResponse3.Content;
                    }
                    else {

                        mensaje = restResponse3.Content;
                    }

                }
                else {

                    mensaje = "No hay atributos por ingresar";

                }

                return mensaje;

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
           
        }

        //Actualizar todos los atributos modificados en nuestras tabalas
        public string actualizar_atributos( string json) {

            string mensaje = "";

            RestClient restClient3 = new RestClient();
            restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomcerce_insertar_atributos");
            RestRequest restRequest3 = new RestRequest(Method.POST);
            restRequest3.AddHeader("Content-Type", "application/json");
            restRequest3.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse restResponse3 = restClient3.Execute(restRequest3);

            if ((int)restResponse3.StatusCode == 200)
            {
                mensaje = restResponse3.Content;
            }
            else
            {

                mensaje = restResponse3.Content;
            }

            return mensaje;
           
        }

        //Obtener los atributos editados recientemente 
        public string obtener_atributos_de_editados_recientemete()
        {

            string json = "";

            try
            {
               
                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_obtener_atributos_actualizados");
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

        //Actualizar los atributos editados recientemente
        public string actualizar_un_atributo_woocomerce()
        {

            string mensaje = "";

            try
            {

                string json = obtener_atributos_de_editados_recientemete();

                List<Clases.AtributosWoocomerce>lista_atributos = JsonConvert.DeserializeObject<List<Clases.AtributosWoocomerce>>(json);

                if (lista_atributos.Count > 0) {

                    MetodosConfiguracionWoocomerce met_configuracion = new MetodosConfiguracionWoocomerce();
                    string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                    string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                    string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos

                    int i = 0;

                    while (i < lista_atributos.Count) {


                        string jso_atributo = JsonConvert.SerializeObject(lista_atributos[i]);

                        RestClient restClient = new RestClient();
                        restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/attributes/"+ lista_atributos[i].id.ToString());
                        RestRequest restRequest = new RestRequest(Method.PUT);
                        restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                        //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                        restRequest.AddHeader("Content-Type", "application/json");
                        restRequest.AddParameter("application/json", jso_atributo, ParameterType.RequestBody);
                        IRestResponse restResponse = restClient.Execute(restRequest);

                        //Si  Woocomerce responde creado
                        if ((int)restResponse.StatusCode == 200)
                        {

                        }
                        else
                        {

                        }

                        i++;
                    }



                }   

                return mensaje = "Atributos actualizados con éxito";

            }
            catch (Exception ex)
            {

                return mensaje + " " + ex.Message.ToString();
            }

        }

        //Obtener los atributos editados recientemente 
        public string obtener_atributos_de_eliminados()
        {

            string json = "";

            try
            {

                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_obtener_atributos_eliminados");
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

        //Eliminar atribitos de nuestras tablas y de woocomerce
        public string eliminar_atributos_de_nuestras_tablas()
        {

            string mensaje = "";

            try
            {

                string json = obtener_atributos_de_eliminados();

                List<Clases.AtributosWoocomerce> lista_atributos = JsonConvert.DeserializeObject<List<Clases.AtributosWoocomerce>>(json);

                if (lista_atributos.Count > 0)
                {

                    MetodosConfiguracionWoocomerce met_configuracion = new MetodosConfiguracionWoocomerce();
                    string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                    string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                    string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos

                    int i = 0;

                    while (i < lista_atributos.Count)
                    {


                        string jso_atributo = JsonConvert.SerializeObject(lista_atributos[i]);

                        RestClient restClient = new RestClient();
                        restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/attributes/" + lista_atributos[i].id.ToString() + "?force=true");
                        RestRequest restRequest = new RestRequest(Method.DELETE);
                        restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                        //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                        restRequest.AddHeader("Content-Type", "application/json");
                        restRequest.AddParameter("application/json", jso_atributo, ParameterType.RequestBody);
                        IRestResponse restResponse = restClient.Execute(restRequest);

                        //Si  Woocomerce responde creado
                        if ((int)restResponse.StatusCode == 200)
                        {


                            //Convertimos el atributo en un nuevo Json para agregar el atributo
                            string json_obj_atributos = JsonConvert.SerializeObject(lista_atributos[i]);

                            RestClient restClient3 = new RestClient();
                            restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomcerce_eliminar_atributos_tablas_propias");
                            RestRequest restRequest3 = new RestRequest(Method.DELETE);
                            restRequest3.AddHeader("Content-Type", "application/json");
                            restRequest3.AddParameter("application/json", json_obj_atributos, ParameterType.RequestBody);
                            IRestResponse restResponse3 = restClient3.Execute(restRequest3);


                        }
                        else
                        {

                        }

                        i++;
                    }



                }

                return mensaje = "Atributos eliminados con éxito";

            }
            catch (Exception ex)
            {

                return mensaje + " " + ex.Message.ToString();
            }

        }


        //*********** WOOCOMERCE ***************************************


    }

}
