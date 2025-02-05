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
    public class MetodosArticulosWoocomerce
    {

        public string ip = ConfigurationSettings.AppSettings["Ip"].ToString();

        //Establecer la instancia con la clase Conexion
        private Conexion v_conexion = new Globales.Conexion();
        SqlDataReader v_leer;
        SqlCommand v_comando = new SqlCommand();
        DataTable v_tabla = new DataTable();


        //Mostrar los atributos
        public DataTable mostrar_articulos_para_activar_sincronizador()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_ARTICULOS_INACTIVOS";
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

        //mostrar articulos encontrados por filtro de busqueda
        public DataTable mostrar_buscar_articulos(Clases.ArticulosBusquedaWoocomerce obj_articulos)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.BUSCAR_ARTICULOS_SINCRONIZA";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@DATO", ((object)obj_articulos.dato.ToString()) ?? DBNull.Value);
                v_comando.Parameters.AddWithValue("@PARAMETRO", ((object)obj_articulos.parametro.ToString()) ?? DBNull.Value);
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

        //Metodo para actualizar articulos a estado S
        public void actualizar_articulos_estados(Clases.ArticulosBusquedaWoocomerce obj_articulos)
        {
            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE_ACTUALIZAR_ESTADO_ACTIVO";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@SKU", obj_articulos.sku);
                v_comando.ExecuteNonQuery();
                v_conexion.CerrarConexion();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Obtener todos los nombres de los estados de un articulo
        public List<Clases.EstadosArticuloWoocomerce> obtener_estados_articulos()
        {
            try
            {

                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.CARGAR_ESTADOS_ARTICULOS";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_leer = v_comando.ExecuteReader();

                List<Clases.EstadosArticuloWoocomerce> lista_estados = new List<Clases.EstadosArticuloWoocomerce>();

                while (v_leer.Read())
                {
                    Clases.EstadosArticuloWoocomerce obj_estados = new Clases.EstadosArticuloWoocomerce();
                    obj_estados.descripcion = v_leer.GetString(1).ToString();
                    lista_estados.Add(obj_estados);

                }

                v_comando.Connection = v_conexion.CerrarConexion();

                return lista_estados;
            }
            catch (Exception)
            {
                throw;
            }

        }

        //Obtener todos los nombres de los estados de un articulo
        public List<Clases.EstadosStockWoocomerce> obtener_estados_stock()
        {
            try
            {

                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.CARGAR_ESTADOS_STOCK";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_leer = v_comando.ExecuteReader();

                List<Clases.EstadosStockWoocomerce> lista_estados = new List<Clases.EstadosStockWoocomerce>();

                while (v_leer.Read())
                {
                    Clases.EstadosStockWoocomerce obj_estados = new Clases.EstadosStockWoocomerce();
                    obj_estados.descripcion = v_leer.GetString(1).ToString();
                    lista_estados.Add(obj_estados);

                }

                v_comando.Connection = v_conexion.CerrarConexion();

                return lista_estados;
            }
            catch (Exception)
            {
                throw;
            }

        }

        //Obtener todos los nombres de los estados de un articulo
        public List<Clases.TipoArticuloWoocomerce> obtener_tipos_articulo()
        {
            try
            {

                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.CARGAR_TIPOS_ARTICULOS";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_leer = v_comando.ExecuteReader();

                List<Clases.TipoArticuloWoocomerce> lista_estados = new List<Clases.TipoArticuloWoocomerce>();

                while (v_leer.Read())
                {

                    Clases.TipoArticuloWoocomerce obj_estados = new Clases.TipoArticuloWoocomerce();
                    obj_estados.descripcion = v_leer.GetString(1).ToString();
                    lista_estados.Add(obj_estados);

                }

                v_comando.Connection = v_conexion.CerrarConexion();

                return lista_estados;
            }
            catch (Exception)
            {
                throw;
            }

        }

        //Mostrar los atributos
        public DataTable mostrar_articulos_sincronizados()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_ARTICULOS_SINCRONIZADOS";
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

        //Sincronizador de articulos, precios, cantidades nuevos
        public void sincronizador_de_nuevos_articulos_precios_cantidades()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.CARGAR_ARTICULOS";
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

        //Mostrar los articulos para variaciones
        public DataTable mostrar_articulos_para_variacion()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTAR_ARTICULOS_ASIGNAR_EN_VARIACION";
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



        //********************************************************* API *********************************************************

        //obtener los articulos editados recientemente de nuestras tablas
        public string obtener_articulos_editados_recientemente_de_nuestro_api()
        {

            string json = "";

            try
            {


                //Enviamos la clase creada a nuestro api para que los inserte o actualice en nuestra tablas propias de woocomerce
                RestClient restClient3 = new RestClient();
                restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomcerce_obtener_articulos_editados_recientemente");
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

        //insertar los articulos editados en la tienda de woocomerce si su id viene vacio
        //este se inserta y si no este se actualiza
        //Actualizar categorias de woocomercer
        public void actualizar_articulos_o_insertarlos_en_woocomerce()
        {

            int i = 0;

            try
            {

                string json = obtener_articulos_editados_recientemente_de_nuestro_api();

                List<Clases.ArticulosWoocomerce> lista_articulos = JsonConvert.DeserializeObject<List<Clases.ArticulosWoocomerce>>(json);

                if (lista_articulos.Count > 0)
                {

                    MetodosConfiguracionWoocomerce met_configuracion = new MetodosConfiguracionWoocomerce();
                    string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                    string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                    string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos

                    while ( i < lista_articulos.Count) {

                        //Se debe de insertar
                        if (lista_articulos[i].id == 0)
                        {

                            //Convertimos el articulo de la lista en un nuevo Json para agregar el atributo
                            string json_articulo = JsonConvert.SerializeObject(lista_articulos[i], Formatting.Indented);

                            RestClient restClient = new RestClient();
                            restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products");
                            RestRequest restRequest = new RestRequest(Method.POST);
                            restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                            //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                            restRequest.AddHeader("Content-Type", "application/json");
                            restRequest.AddParameter("application/json", json_articulo, ParameterType.RequestBody);
                            IRestResponse restResponse = restClient.Execute(restRequest);

                                //Si  Woocomerce responde creado
                                if ((int)restResponse.StatusCode == 201)
                                {

                                //Clases.ArticulosWoocomerce obj_art_id_woocomerce = JsonConvert.DeserializeObject<Clases.ArticulosWoocomerce>(restResponse.Content);


                                //Enviamos la clase creada a nuestro api para que los inserte o actualice en nuestra tablas propias de woocomerce
                                RestClient restClient3 = new RestClient();
                                restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_actualizar_id_articulo");
                                RestRequest restRequest3 = new RestRequest(Method.PUT);
                                restRequest3.AddHeader("Content-Type", "application/json");
                                restRequest3.AddParameter("application/json", restResponse.Content, ParameterType.RequestBody);
                                IRestResponse restResponse3 = restClient3.Execute(restRequest3);

                                //Si Woocomerce responde entoces que continue el proceso
                                if ((int)restResponse3.StatusCode == 200)
                                {
                                    string cadena = restResponse3.Content;
                                }


                                }


                        }
                            //Se debe de actualizar
                        else {

                            //Convertimos el articulo de la lista en un nuevo Json para agregar el atributo
                            string json_articulo = JsonConvert.SerializeObject(lista_articulos[i]);

                            RestClient restClient = new RestClient();
                            restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/" + lista_articulos[i].id.ToString());
                            RestRequest restRequest = new RestRequest(Method.PUT);
                            restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                            //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                            restRequest.AddHeader("Content-Type", "application/json");
                            restRequest.AddParameter("application/json", json_articulo, ParameterType.RequestBody);
                            IRestResponse restResponse = restClient.Execute(restRequest);

                            if ((int)restResponse.StatusCode == 200)
                            {

                            }


                        }

                        i++;

                    }

                }

            }
            catch (Exception ex)
            {
                //Si algun error ocurre entonce que el contador de articulos siga con los siguientes
                i++;
                throw;
            }


        }

        //Obtener los articulos que se actualizaron sus atributos
        public string obtener_articulos_que_editaron_sus_atributos()
        {

            string json = "";

            try
            {


                //Enviamos la clase creada a nuestro api para obtener los articulos padres que fueron actualizados
                RestClient restClient3 = new RestClient();
                restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_obtener_articulos_editados_en_articulos_atributo");
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

        //Obtener los atributos del articulo padre
        public string obtener_atributos_del_articulo_padre()
        {

            string json = "";

            try
            {

                MetodosConfiguracionWoocomerce met_configuracion = new MetodosConfiguracionWoocomerce();
                string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos

                //Obtenemos los articulos padre
                json = obtener_articulos_que_editaron_sus_atributos();

                //Hacemos una lista con los padres
                List<Clases.ActualizarArticulosAtributos> lista_articulos_padre = JsonConvert.DeserializeObject<List<Clases.ActualizarArticulosAtributos>>(json);

                int contador_padres = 0;

                while (contador_padres < lista_articulos_padre.Count) {

                    Clases.AtributosPadreWoocomerce obj_lista_atr = new Clases.AtributosPadreWoocomerce();
                    List<Clases.AtributosWoocomerceActualizar> obj_lista = new List<Clases.AtributosWoocomerceActualizar>();



                    //Enviamos la clase creada a nuestro api para obtener los articulos padres que fueron actualizados
                    RestClient restClient3 = new RestClient();
                    restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_obtener_atributos_padre/" + lista_articulos_padre[contador_padres].sku);
                    RestRequest restRequest3 = new RestRequest(Method.GET);
                    IRestResponse restResponse3 = restClient3.Execute(restRequest3);

                    //Si nuestro api responde 200 es decir que tiene atributos para colocar
                    if ((int)restResponse3.StatusCode == 200)
                    {

                        //Hacemos una lista con los atributos que tiene el padre
                        List<Clases.AtributosPadre> lista_atributos_padre = JsonConvert.DeserializeObject<List<Clases.AtributosPadre>>(restResponse3.Content);

                        int contador_atributos = 0;

                        //la cantidad de atributos que tenga el articulo padre
                        while (contador_atributos < lista_atributos_padre.Count) {

                            Clases.AtributosWoocomerceActualizar obj_atributoswoo = new Clases.AtributosWoocomerceActualizar();

                            obj_atributoswoo.id = lista_atributos_padre[contador_atributos].id;
                            obj_atributoswoo.visible = true;
                            obj_atributoswoo.variation = true;


                            RestClient restClient4 = new RestClient();
                            restClient4.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_obtener_valores_de_un_atributo/" + lista_atributos_padre[contador_atributos].id + "/" + lista_articulos_padre[contador_padres].sku );
                            RestRequest restRequest4 = new RestRequest(Method.GET);
                            IRestResponse restResponse4 = restClient4.Execute(restRequest4);

                            //Si nuestro api responde 200 es decir que tiene atributos para colocar
                            if ((int)restResponse4.StatusCode == 200)
                            {

                                int contador_terminos_atrbutos = 0;

                                //Hacemos una lista con los atributos que tiene el padre
                                List<Clases.ValoresAtributosPadre> lista_terminos_atributos = JsonConvert.DeserializeObject<List<Clases.ValoresAtributosPadre>>(restResponse4.Content);

                                List<string> lista = new List<string>();

                                //miesntras los atributos envien terminos de atributos
                                while ( contador_terminos_atrbutos < lista_terminos_atributos.Count) {

                                    lista.Add(lista_terminos_atributos[contador_terminos_atrbutos].name);

                                    contador_terminos_atrbutos++;

                                }

                                obj_atributoswoo.options = lista;

                                // lista final con sus atributps para ese padre

                                obj_lista.Add(obj_atributoswoo);                           


                            } // respues de terminos atributos padre


                           contador_atributos++;

                        } // while de atributos padre



                    } // Si el metodod de obtener padres trae atributos

                    obj_lista_atr.attributes = obj_lista;

                    string jsonData = JsonConvert.SerializeObject(obj_lista_atr, Formatting.Indented);

                    //Aqui debemos de mandar la lista con atributos a sus respectivos padres

                    RestClient restClient = new RestClient();
                    restClient.BaseUrl = new Uri("https://" + tienda + "//wp-json/wc/v3/products/" + lista_articulos_padre[contador_padres].id);
                    RestRequest restRequest = new RestRequest(Method.PUT);
                    restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                    //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    restRequest.AddHeader("Content-Type", "application/json");
                    restRequest.AddParameter("application/json", jsonData, ParameterType.RequestBody);
                    IRestResponse restResponse = restClient.Execute(restRequest);

                    //Si  Woocomerce responde creado
                    if ((int)restResponse.StatusCode == 200)
                    {

                    }


                        contador_padres++; 

                } //while de padres



              



                return json;

            }
            catch (Exception ex)
            {
                throw;
            }

        }




    }

}
