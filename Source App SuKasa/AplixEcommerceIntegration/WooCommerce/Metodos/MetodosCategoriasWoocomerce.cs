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
    public class MetodosCategoriasWoocomerce
    {

        public string ip = ConfigurationSettings.AppSettings["Ip"].ToString();


        //Establecer la instancia con la clase Conexion
        private Conexion v_conexion = new Globales.Conexion();
        SqlDataReader v_leer;
        SqlCommand v_comando = new SqlCommand();
        DataTable v_tabla = new DataTable();

        /*METODOS DE APLICACION*/

        //Insertar categorias a nuestras tablas 
        public void insertar_categorias(WooCommerce.Clases.CategoriasWoocomerce obj_categorias) {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.INSERTAR_CATEGORIAS_APP_SIMPLE";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@CODIGO_WOOCOMERCE", obj_categorias.id);
                v_comando.Parameters.AddWithValue("@NOMBRE", obj_categorias.name);
                v_comando.Parameters.AddWithValue("@DESCRIPCION", obj_categorias.description);
                v_comando.Parameters.AddWithValue("@CATEGORIA_PADRE", obj_categorias.parent);
                v_comando.Parameters.AddWithValue("@SLUG", obj_categorias.slug);
                v_comando.Parameters.AddWithValue("@ESTADO", obj_categorias.estado);
                v_comando.ExecuteNonQuery();
                v_conexion.CerrarConexion();

            }
            catch (Exception)
            {
                throw;
            }


        }

        //Mostrar categorias padre
        public DataTable mostrar_categorias_padre()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_CATEGORIAS_PADRE";
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

        //Validar codigo de categoria      
        public string validar_codigo_de_categoria(WooCommerce.Clases.CategoriasWoocomerce obj_categorias)
        {
            string mensaje = "";

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.VALIDAR_CATEGORIA";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@CATEGORIA", obj_categorias.parent);
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

        //Validar nombre de categoria      
        public string validar_nombre_de_categoria(WooCommerce.Clases.CategoriasWoocomerce obj_categorias)
        {
            string mensaje = "";

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.VALIDAR_NOMBRE_CATEGORIA";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@NOMBRE", obj_categorias.name);
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

        //Bucar categorias padre
        public DataTable buscar_categorias_padre(WooCommerce.Clases.BusquedaCategoriasWoocomerce obj_categorias)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.BUSCAR_CATEGORIAS_PADRE";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@DATO", ((object)obj_categorias.dato.ToString()) ?? DBNull.Value);
                v_comando.Parameters.AddWithValue("@PARAMETRO", ((object)obj_categorias.parametro.ToString()) ?? DBNull.Value);
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
       
        //Mostrar categorias padre
        public DataTable mostrar_categorias()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_CATEGORIAS";
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

        //Eliminar categorias de nuestras tablas 
        public void limpiar_tabla_categorias() {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE_BORRAR_CATEGORIAS";
                v_comando.CommandType = CommandType.StoredProcedure;      
                v_comando.ExecuteNonQuery();
                v_conexion.CerrarConexion();

            }
            catch (Exception)
            {
                throw;
            }
        }

        //Bucar categorias padre
        public DataTable buscar_categorias(WooCommerce.Clases.BusquedaCategoriasWoocomerce obj_categorias)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.BUSCAR_CATEGORIAS";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@DATO", ((object)obj_categorias.dato.ToString()) ?? DBNull.Value);
                v_comando.Parameters.AddWithValue("@PARAMETRO", ((object)obj_categorias.parametro.ToString()) ?? DBNull.Value);
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

        //Mostrar categorias para asiganr en la pantalla de articulos
        public DataTable mostrar_categorias_para_asignar()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_CATEGORIAS_ARTICULOS";
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

        /*METODOS DE WOOCOMERCE*/


        //***************************CATEGORIAS********************************************************************

        //Agregar nuevo categiria a woocomercer
        public string agregar_nueva_categoria_woocomerce(WooCommerce.Clases.CategoriasWoocomerce obj_categorias)
        {

            string mensaje = "";

            try
            {

                MetodosConfiguracionWoocomerce met_configuracion = new MetodosConfiguracionWoocomerce();
                string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos
              

                //Convertimos el atributo en un nuevo Json para agregar el atributo
                string json_obj_categorias = JsonConvert.SerializeObject(obj_categorias);

                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/categories");
                RestRequest restRequest = new RestRequest(Method.POST);
                restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                restRequest.AddHeader("Content-Type", "application/json");
                restRequest.AddParameter("application/json", json_obj_categorias, ParameterType.RequestBody);
                IRestResponse restResponse = restClient.Execute(restRequest);


                //Si  Woocomerce responde creado
                if ((int)restResponse.StatusCode == 201)
                {

                    // Obtenemos la respuesta con el id de categorias 
                   WooCommerce.Clases.CategoriasWoocomerce obje_respuesta_categoria  = JsonConvert.DeserializeObject<WooCommerce.Clases.CategoriasWoocomerce>(restResponse.Content);
                   MetodosCategoriasWoocomerce met_categorias = new MetodosCategoriasWoocomerce();
                   obj_categorias.id = obje_respuesta_categoria.id;
                   obj_categorias.slug = obje_respuesta_categoria.slug; 
                   met_categorias.insertar_categorias(obj_categorias);

                    mensaje = "Categoría:" + obj_categorias.name + " agregada con éxito con el código: " + obj_categorias.id;

                }
                else
                {

                    mensaje = "Error al insertar la categoria " + obj_categorias.name;

                }

                return mensaje;

            }
            catch (Exception ex )
            {

                return mensaje + " " +  ex.Message.ToString();
            }

        }

        //Obtener categorias de woocomerce
        public string obtener_categorias_woocoemrce() {

            string json = "";

            try
            {
                Metodos.MetodosConfiguracionWoocomerce met_configuracion = new Metodos.MetodosConfiguracionWoocomerce();
                string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos
                                                                                            //Hacemos una lista de terminos de clase de atributos con sus terminos atributos
                List<Clases.CategoriasWoocomerce> lista_obj_atributos_con_terminos_atributo = new List<Clases.CategoriasWoocomerce>();

                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/categories");
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

        //Insertar categorias a nuestras tablas por medio del api
        public string insertar_categorias_tablas_propias() {

            string json = obtener_categorias_woocoemrce();

            string mensaje = "";
            
            try
            {
                if (json != "")
                {

                    //Enviamos la clase creada a nuestro api para que los inserte o actualice en nuestra tablas propias de woocomerce
                    RestClient restClient3 = new RestClient();
                    restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomcerce_insertar_categorias");
                    RestRequest restRequest3 = new RestRequest(Method.POST);
                    restRequest3.AddHeader("Content-Type", "application/json");
                    restRequest3.AddParameter("application/json", json, ParameterType.RequestBody);
                    IRestResponse restResponse3 = restClient3.Execute(restRequest3);

                    //Si Woocomerce responde entoces que continue el proceso
                    if ((int)restResponse3.StatusCode == 200)
                    {
                        mensaje = restResponse3.Content;
                    }
                    else
                    {

                        mensaje = restResponse3.Content;
                    }

                }
                else {

                    mensaje = "Lista de categorias Woocomerce vacía";

                }


                return mensaje;

            }
            catch (Exception ex )
            {
                return mensaje + " " + ex.Message.ToString();
            } 

        }

        //Insertar categorias a nuestras tablas por medio del api
        public string obtener_categorias_tablas_propias_editadas()
        {

            string json = "";

            try
            {
               

                    //Enviamos la clase creada a nuestro api para que los inserte o actualice en nuestra tablas propias de woocomerce
                    RestClient restClient3 = new RestClient();
                    restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_obtener_categorias_editados");
                    RestRequest restRequest3 = new RestRequest(Method.GET);
                    restRequest3.AddHeader("Content-Type", "application/json");
                    restRequest3.AddParameter("application/json", json, ParameterType.RequestBody);
                    IRestResponse restResponse3 = restClient3.Execute(restRequest3);

                    //Si Woocomerce responde entoces que continue el proceso
                    if ((int)restResponse3.StatusCode == 200)
                    {
                        json = restResponse3.Content;
                    }
             
              

                return json;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //Actualizar categorias de woocomercer
        public void actualizar_categorias_woocomerce( Clases.CategoriasWoocomerce obj_cat )
        {

            try
            {

             
                MetodosConfiguracionWoocomerce met_configuracion = new MetodosConfiguracionWoocomerce();
                string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos


                //Convertimos la categorias enviada en una nueva
                string json_obj_categorias = JsonConvert.SerializeObject(obj_cat);

                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/categories/" + obj_cat.id.ToString());
                RestRequest restRequest = new RestRequest(Method.PUT);
                restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                restRequest.AddHeader("Content-Type", "application/json");
                restRequest.AddParameter("application/json", json_obj_categorias, ParameterType.RequestBody);
                IRestResponse restResponse = restClient.Execute(restRequest);

                //Si  Woocomerce responde el acrualizao
                if ((int)restResponse.StatusCode == 201){} else{}

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //Insertar categorias a nuestras tablas por medio del api
        public string obtener_categorias_tablas_propias_eliminadas()
        {

            string json = "";

            try
            {


                //Enviamos la clase creada a nuestro api para que los inserte o actualice en nuestra tablas propias de woocomerce
                RestClient restClient3 = new RestClient();
                restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_obtener_categorias_eliminados");
                RestRequest restRequest3 = new RestRequest(Method.GET);
                restRequest3.AddHeader("Content-Type", "application/json");
                restRequest3.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse restResponse3 = restClient3.Execute(restRequest3);

                //Si Woocomerce responde entoces que continue el proceso
                if ((int)restResponse3.StatusCode == 200)
                {
                    json = restResponse3.Content;
                }



                return json;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //Actualizar categorias de woocomercer
        public void eliminarr_categorias_woocomerce()
        {

            try
            {

                Metodos.MetodosCategoriasWoocomerce met_cat = new MetodosCategoriasWoocomerce();

                //obtenermos las categorias eliminadas
                string json = met_cat.obtener_categorias_tablas_propias_eliminadas();

                //Deserealizamos esas categorias
                List<Clases.CategoriasWoocomerce> lista_obj_categorias = JsonConvert.DeserializeObject<List<Clases.CategoriasWoocomerce>>(json);


                if (lista_obj_categorias.Count > 0)
                {

                    MetodosConfiguracionWoocomerce met_configuracion = new MetodosConfiguracionWoocomerce();
                    string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                    string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                    string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos

                    int i = 0;

                    while ( i<lista_obj_categorias.Count ) {


                        RestClient restClient = new RestClient();
                        restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/categories/"+ lista_obj_categorias[i].id.ToString()+"?force=true");
                        RestRequest restRequest = new RestRequest(Method.DELETE);
                        restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                        //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                        restRequest.AddHeader("Content-Type", "application/json");
                        IRestResponse restResponse = restClient.Execute(restRequest);

                        //Si  Woocomerce responde el elinado correcto
                        if ((int)restResponse.StatusCode == 200) {

                            //Enviamos el id de elimnar 

                            //Convertimos el atributo en un nuevo Json para agregar el atributo
                            string json_obj_categoria = JsonConvert.SerializeObject(lista_obj_categorias[i]);

                            RestClient restClient3 = new RestClient();
                            restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomcerce_eliminar_categorias_tablas_propias");
                            RestRequest restRequest3 = new RestRequest(Method.POST);
                            restRequest3.AddHeader("Content-Type", "application/json");
                            restRequest3.AddParameter("application/json", json_obj_categoria, ParameterType.RequestBody);
                            IRestResponse restResponse3 = restClient3.Execute(restRequest3);
                          


                        } else { }

                        i++;
                    }

                }
               
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //***************************CATEGORIAS********************************************************************



    }
}
