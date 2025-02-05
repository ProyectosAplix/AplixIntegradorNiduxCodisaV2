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
    public class MetodosVariacionesWoocomerce
    {
        public string ip = ConfigurationSettings.AppSettings["Ip"].ToString();

        //Establecer la instancia con la clase Conexion
        private Conexion v_conexion = new Globales.Conexion();
        SqlDataReader v_leer;
        SqlCommand v_comando = new SqlCommand();
        DataTable v_tabla = new DataTable();

        //Insertar artiuclos atributos en nuestras tablas
        public void insertar_atributos_articulos(WooCommerce.Clases.ArtributosArticulos obj_atributos)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.INSERTAR_ARTICULOS_ATRIBUTOS";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@ARTICULO", obj_atributos.articulo);
                v_comando.Parameters.AddWithValue("@ID_ATRIBUTO", obj_atributos.id_atributo);
                v_comando.Parameters.AddWithValue("@ID_TERMINO_ATRIBUTO", obj_atributos.id_termino_atributo);
                v_comando.ExecuteNonQuery();
                v_conexion.CerrarConexion();

            }
            catch (Exception)
            {
                throw;
            }


        }

        //Mostrar variaciones de un articulo padre
        public DataTable mostrar_variaciones_padres(WooCommerce.Clases.ArticulosVariaciones obj_variaciones)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE_MOSTRAR_TERMINOS_ATRIBUTOS";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@ARTICULO_PADRE", ((object)obj_variaciones.articulo_padre.ToString()) ?? DBNull.Value);
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

        //Insertar variaciones en nuestras tablas 
        public void Insertar_variaciones(Clases.ArticulosVariaciones obj_var)
        {
            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOOCOMERCE_INSERTAR_VARIACIONES";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@ARTICULO", obj_var.articulo);
                v_comando.Parameters.AddWithValue("@ARTICULO_PADRE", obj_var.articulo_padre);
                v_comando.Parameters.AddWithValue("@ID_TERMINO_ATRIBUTO", obj_var.termino_lista);
                v_comando.Parameters.AddWithValue("@DESCRIPCION_WOOCOMERCE", obj_var.nombre);
                v_comando.Parameters.AddWithValue("@USA_STOCK", obj_var.usa_stock);
                v_comando.Parameters.AddWithValue("@ESTADO", obj_var.estado);
                v_comando.Parameters.AddWithValue("@SINCRONIZA", obj_var.sincroniza);
                v_comando.Parameters.AddWithValue("@ID_WOOCOMERCE", obj_var.id);
                v_comando.ExecuteNonQuery();               

            }
            catch (Exception)
            {
                throw;
            }
        }

        //Mostrar varaiciones ingresadas en un articulo
        public DataTable mostrar_variaciones(WooCommerce.Clases.ArticulosVariaciones obj_variaciones)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_VARIACIONES_ARTICULOS";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@ARTICULO_PADRE", ((object)obj_variaciones.articulo_padre.ToString()) ?? DBNull.Value);
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



        //***************************************** API ***************************************************//


        //Obtener los articulos editados recientemente de nuestras tablas
        public string obtener_padres_editados_recientemente_en_variaciones_de_nuestro_api()
        {

            string json = "";

            try
            {

                RestClient restClient3 = new RestClient();
                restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_padres_de_variaciones_editados_recientemente");
                RestRequest restRequest3 = new RestRequest(Method.GET);
                IRestResponse restResponse3 = restClient3.Execute(restRequest3);

                //Si nuestro api responde con 200 esta bien
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

        //Obtener los articulos editados recientemente de nuestras tablas
        public string obtener_variaciones_editados_recientemente_de_nuestro_api( string articulo)
        {

            string json = "";

            try
            {


                //Enviamos la clase creada a nuestro api para que los inserte o actualice en nuestra tablas propias de woocomerce
                RestClient restClient3 = new RestClient();
                restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_obtener_variaciones_editados_recientemente/" + articulo);
                RestRequest restRequest3 = new RestRequest(Method.GET);
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

        //Obtener los articulos editados recientemente de nuestras tablas
        public string obtener_atributos_de_una_variacion_de_nuestro_api(int termino)
        {

            string json = "";

            try
            {


                //Enviamos la clase creada a nuestro api para que los inserte o actualice en nuestra tablas propias de woocomerce
                RestClient restClient3 = new RestClient();
                restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_obtener_atributos_para_variaciones/" + termino);
                RestRequest restRequest3 = new RestRequest(Method.GET);
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

        //Insertar o actualizar variaciones
        public void actualizar_variaciones_o_insertarlos_en_woocomerce()
        {

            int contador_padres = 0;

           
            try
            {

                //json que contiene la lista de padres 
                string json_padres = obtener_padres_editados_recientemente_en_variaciones_de_nuestro_api();

                List<Clases.PadresVariacionWoocomerce> lista_padres = JsonConvert.DeserializeObject<List<Clases.PadresVariacionWoocomerce>>(json_padres);

                // si la lista trae padres para obtener sus variaciones
                if (lista_padres.Count > 0)
                {

                    MetodosConfiguracionWoocomerce met_configuracion = new MetodosConfiguracionWoocomerce();
                    string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                    string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                    string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos


                    //bucle de lista de padres
                    while (contador_padres < lista_padres.Count) {

                        int contador_variaciones = 0;

                        //Obtenermos las variaciones de ese articulo padre
                        string json_variaciones = obtener_variaciones_editados_recientemente_de_nuestro_api( lista_padres[contador_padres].sku );

                        //Lista de variaciones
                        List<Clases.ActualizarVariacionWoocomerce> lista_variaciones = JsonConvert.DeserializeObject<List<Clases.ActualizarVariacionWoocomerce>>(json_variaciones);

                        List<Clases.AtributosVariacionesWoocomerce> lista_crea_cadena_atributos = new List<Clases.AtributosVariacionesWoocomerce>();

                        List<Clases.AtributosVariacionesWoocomerce> lista_editada_cadena_atributos = new List<Clases.AtributosVariacionesWoocomerce>();

                        //Si la lista de variaciones viene llena
                        if (lista_variaciones.Count > 0) {

                            // while de variaciones
                            while (contador_variaciones < lista_variaciones.Count) {

                                //Si el articulo tare id nulo o 0 entonces hay que insetarlo
                                if (lista_variaciones[contador_variaciones].id == 0)
                                {
                                    int contador_para_obtener_lista_atributos = 0;
                          
                                    //Convertimos el articulo de la lista en un nuevo Json para agregar la variacion
                                    string json_var = JsonConvert.SerializeObject(lista_variaciones[contador_variaciones], Formatting.Indented);

                                    Clases.CrearVariacionesWoocomerce obj = new Clases.CrearVariacionesWoocomerce();
                                    obj.description     = lista_variaciones[contador_variaciones].description;
                                    obj.sku             = lista_variaciones[contador_variaciones].sku;
                                    obj.sale_price           = lista_variaciones[contador_variaciones].sale_price;
                                    obj.regular_price   = lista_variaciones[contador_variaciones].regular_price;
                                    obj.status          = lista_variaciones[contador_variaciones].status;
                                    obj.manage_stock    = lista_variaciones[contador_variaciones].manage_stock;
                                    obj.stock_quantity  = lista_variaciones[contador_variaciones].stock_quantity;
                                    obj.weight          = lista_variaciones[contador_variaciones].weight;

                                    //Hacemos la cadena de atributos
                                    if (lista_variaciones[contador_variaciones] == null)
                                    {}
                                    else {

                                        while (contador_para_obtener_lista_atributos < lista_variaciones[contador_variaciones].attributes.Count) {

                                            int termino = lista_variaciones[contador_variaciones].attributes[contador_para_obtener_lista_atributos].id;

                                            string cadenas_atributos = obtener_atributos_de_una_variacion_de_nuestro_api(termino);

                                            //Metemos ese objeto en una lista
                                            Clases.AtributosVariacionesWoocomerce obj_atributo = JsonConvert.DeserializeObject<Clases.AtributosVariacionesWoocomerce>(cadenas_atributos);

                                            lista_crea_cadena_atributos.Add(obj_atributo);

                                            contador_para_obtener_lista_atributos++;
                                        }

                                    }

                                    obj.attributes = lista_crea_cadena_atributos;

                                    //Objeto listo para enviarse a woocomerce
                                    string objetoson = JsonConvert.SerializeObject(obj, Formatting.Indented);

                                    RestClient restClient = new RestClient();
                                    restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/"+ lista_padres[contador_padres].id.ToString() + "/variations");
                                    //restClient.BaseUrl = new Uri("https://localhost/tiendacommerce/wp-json/wc/v3/products/99/variations");                           
                                    RestRequest restRequest = new RestRequest(Method.POST);
                                    restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                                    //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                                    restRequest.AddHeader("Content-Type", "application/json");
                                    restRequest.AddParameter("application/json", objetoson, ParameterType.RequestBody);
                                    IRestResponse restResponse = restClient.Execute(restRequest);

                                    //Si  Woocomerce responde creado
                                    if ((int)restResponse.StatusCode == 201)
                                    {
                                        //Metemos esa respuesta en un objeto para enviar actualizar a nuestras tablas
                                        Clases.RespuestaIdVariacionWoocomerce obj_respuesta_id = JsonConvert.DeserializeObject<Clases.RespuestaIdVariacionWoocomerce>(restResponse.Content);

                                        //serializamos esa respuesta para enviarla a nuestras tablas
                                        string respuesta_var = JsonConvert.SerializeObject(obj_respuesta_id, Formatting.Indented);

                                        RestClient restClient3 = new RestClient();
                                        restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_actualizar_id_variacion");
                                        RestRequest restRequest3 = new RestRequest(Method.PUT);
                                        restRequest3.AddHeader("Content-Type", "application/json");
                                        restRequest3.AddParameter("application/json", respuesta_var, ParameterType.RequestBody);
                                        IRestResponse restResponse3 = restClient3.Execute(restRequest3);

                                        //Si Woocomerce responde entoces que continue el proceso
                                        if ((int)restResponse3.StatusCode == 200)
                                        {
                                            string cadena = restResponse3.Content;
                                        }

                                    }

                                }
                                else {

                                    //Aqui tenemos que actualizar las variaciones

                                    int contador_para_obtener_lista_atributos = 0;

                                    //Convertimos el articulo de la lista en un nuevo Json para agregar la variacion
                                    string json_var = JsonConvert.SerializeObject(lista_variaciones[contador_variaciones], Formatting.Indented);

                                    Clases.ActualizarVariacionWoocomerce obj = new Clases.ActualizarVariacionWoocomerce();
                                    obj.id = lista_variaciones[contador_variaciones].id;
                                    obj.description = lista_variaciones[contador_variaciones].description;
                                    obj.sku = lista_variaciones[contador_variaciones].sku;
                                    obj.sale_price = lista_variaciones[contador_variaciones].sale_price;
                                    obj.regular_price = lista_variaciones[contador_variaciones].regular_price;
                                    obj.status = lista_variaciones[contador_variaciones].status;
                                    obj.manage_stock = lista_variaciones[contador_variaciones].manage_stock;
                                    obj.stock_quantity = lista_variaciones[contador_variaciones].stock_quantity;
                                    obj.weight = lista_variaciones[contador_variaciones].weight;

                                    //Hacemos la cadena de atributos
                                    if (lista_variaciones[contador_variaciones] == null)
                                    { }
                                    else
                                    {

                                        while (contador_para_obtener_lista_atributos < lista_variaciones[contador_variaciones].attributes.Count)
                                        {

                                            int termino = lista_variaciones[contador_variaciones].attributes[contador_para_obtener_lista_atributos].id;

                                            string cadenas_atributos = obtener_atributos_de_una_variacion_de_nuestro_api(termino);

                                            //Metemos ese objeto en una lista
                                            Clases.AtributosVariacionesWoocomerce obj_atributo = JsonConvert.DeserializeObject<Clases.AtributosVariacionesWoocomerce>(cadenas_atributos);

                                            lista_editada_cadena_atributos.Add(obj_atributo);

                                            contador_para_obtener_lista_atributos++;
                                        }

                                    }

                                    obj.attributes = lista_editada_cadena_atributos;

                                    //Objeto listo para enviarse a woocomerce
                                    string objetoson = JsonConvert.SerializeObject(obj, Formatting.Indented);

                                    RestClient restClient = new RestClient();
                                    restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/" + lista_padres[contador_padres].id.ToString() + "/variations/" + obj.id.ToString());                                               
                                    RestRequest restRequest = new RestRequest(Method.PUT);
                                    restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                                    //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                                    restRequest.AddHeader("Content-Type", "application/json");
                                    restRequest.AddParameter("application/json", objetoson, ParameterType.RequestBody);
                                    IRestResponse restResponse = restClient.Execute(restRequest);

                                    //Si  Woocomerce responde creado
                                    if ((int)restResponse.StatusCode == 200)
                                    {
                                       
                                    }


                                }// Fin del else


                                contador_variaciones++;   

                            }//while de variaciones

                        }// if de lista de variaciones

                        contador_padres++;

                    }// fin de while de lista de padres

                } // if de la lista de padres

                   

            }
            catch (Exception ex)
            {
                //Si algun error ocurre entonce que el contador de articulos siga con los siguientes
                contador_padres++;
                throw;
            }


        }


        //***************************************** API ***************************************************//


    }
}
