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
    public class MetodosTerminoAtributosWoocomerce
    {
        public string ip = ConfigurationSettings.AppSettings["Ip"].ToString();

        //Establecer la instancia con la clase Conexion
        private Conexion v_conexion = new Globales.Conexion();
        SqlDataReader v_leer;
        SqlCommand v_comando = new SqlCommand();
        DataTable v_tabla = new DataTable();


        /***************   APILICACION  *****************/

        //Mostrar los terminos de atributos
        public DataTable mostrar_terminos_atributos()
        {
            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_TERMINOS_ATRIBUTOS";
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

        //Bucar terminos de atributos
        public DataTable buscar_terminos_atributos(WooCommerce.Clases.BusquedaTerminoAtributosWoocomerce obj_atributos)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.BUSCAR_TERMINOS_ATRIBUTOS";
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

        //Limiar tabla de terminos
        public void limpiar_terminos_atributos()
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.ELIMINAR_TERMINOS_ATRIBUTO";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.ExecuteNonQuery();
                v_conexion.CerrarConexion();

            }
            catch (Exception)
            {
                throw;
            }

        }

        //Bucar terminos de atributos por atributo
        public DataTable buscar_terminos_atributos_atributo(WooCommerce.Clases.BusquedaTerminoAtributosWoocomerce obj_atributos)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.MOSTRAR_TERMINOS_ATRIBUTOS_PARAMETRO";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@ID", ((object)obj_atributos.dato.ToString()) ?? DBNull.Value);
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

        //Actualizar terminos de atributos en nuestras tablas propias
        public void actualizar_terminos_atributos_tablas(Clases.TerminosAtributosCompletoWoocomerce obj_terminos_atributos)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.ACTUALIZAR_TERMINOS_ATRIBUTOS";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@ID", obj_terminos_atributos.id_termino_atributo);
                v_comando.Parameters.AddWithValue("@DESCRIPCION", obj_terminos_atributos.description_termino_atributo);
                v_comando.Parameters.AddWithValue("@NOMBRE", obj_terminos_atributos.name_termino_atributo);
                v_comando.Parameters.AddWithValue("@SLUG", obj_terminos_atributos.slug);
                v_comando.Parameters.AddWithValue("@ESTADO", obj_terminos_atributos.estado);
                v_comando.ExecuteNonQuery();
                v_conexion.CerrarConexion();
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Bucar terminos de atributos padres por busqueda
        public DataTable buscar_terminos_atributos_padres_por_busqueda(WooCommerce.Clases.BusquedaTerminoAtributosWoocomerce obj_atributos)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "WOOCOMERCE.BUSCAR_TERMINOS_ATRIBUTOS";
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


        /***************   API   *****************/


        //obtener terminos de woocomerce 
        public string obtener_lista_terminos_atributos()
        {

            string mensaje = "";

            //Lista donde vamos a;adir todos los terminos de atributos
            List<Clases.TerminosAtributosCompletoWoocomerce> lista_terminos = new List<Clases.TerminosAtributosCompletoWoocomerce>();

            //contador que recorre la lista de atributos
            int i = 0;

            try
            {

                //Llamamos la instancia de los atributos que queremos obtener sus valores
                Metodos.MetodosAtributosWoocomerce met_terminos = new MetodosAtributosWoocomerce();

                //Lista con todos los atributps de la tienda
                List<Clases.AtributosWoocomerce> lista_atributos = JsonConvert.DeserializeObject<List<Clases.AtributosWoocomerce>>(met_terminos.obtener_atributos_de_woocomerce());

                //Validamos que esta lista venga llena
                if (lista_atributos.Count > 0)
                {

                    //Parametros que necestan los mtodos para poder obtener las claves de acceso
                    Metodos.MetodosConfiguracionWoocomerce met_configuracion = new Metodos.MetodosConfiguracionWoocomerce();
                    string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                    string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                    string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos


                    //Contador de los atributos
                    while (i < lista_atributos.Count)
                    {

                        //Si un error ocurre al obtener esos valores de atributos que siga con los siguientes
                        try
                        {                          

                            //Esta variable es la que nos indica que ya no hay terminos de atributo mas que agregar
                            bool bandera = true;


                            //Paginas que tenemos que enviar por parametro
                            int pagina = 1;


                            //mientras la bandera se mantenga en true entonces sigue trayendo terminos de atributos
                            while (bandera == true) {

                                RestClient restClient = new RestClient();
                                restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/attributes/" + lista_atributos[i].id + "/terms?page=" + pagina.ToString());
                                RestRequest restRequest = new RestRequest(Method.GET);                              
                                restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                                //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                                IRestResponse restResponse = restClient.Execute(restRequest);

                                if ((int)restResponse.StatusCode == 200)
                                {
                                    pagina++;

                                    List<Clases.TerminosAtributoWoocomerce> lista_terminos_atributos = new List<Clases.TerminosAtributoWoocomerce>();
                                    lista_terminos_atributos = JsonConvert.DeserializeObject<List<Clases.TerminosAtributoWoocomerce>>(restResponse.Content);

                                    //si esa pagina ya viene vacia entonces hasta ahi es la lista
                                    if (lista_terminos_atributos.Count > 0)
                                    {

                                        int contador = 0;

                                        while (contador < lista_terminos_atributos.Count) {


                                            //Llenamos un objeto con todos los valores
                                            Clases.TerminosAtributosCompletoWoocomerce obj = new Clases.TerminosAtributosCompletoWoocomerce();
                                            obj.id_atributo = lista_atributos[i].id;
                                            obj.name_atributo = lista_atributos[i].name;
                                            obj.id_termino_atributo = lista_terminos_atributos[contador].id;
                                            obj.name_termino_atributo = lista_terminos_atributos[contador].name;
                                            obj.description_termino_atributo = lista_terminos_atributos[contador].description;
                                            obj.slug = lista_terminos_atributos[contador].slug;

                                            //Lo a;adimos a la lista
                                            lista_terminos.Add(obj);

                                            contador++;
                                        }


                                    }
                                    else {

                                        bandera = false;

                                    }


                                }
                                else {

                                    bandera = false;

                                }

                            }// llave de while de bool bandera

                        } //llave de try
                        catch (Exception)
                        {
                            i++;
                        }

                     i++; //contador de las lista de atributos

                    }

                }            

            }
            catch (Exception ex)
            {
                i++;              
            }

            return mensaje = JsonConvert.SerializeObject(lista_terminos);

        }

        //insertar terminos de atributo en nuestras tablas
        public string insertar_terminos_atributo() {

            string mensaje = "";
           
            try
            {
                string json = obtener_lista_terminos_atributos();

                RestClient restClient3 = new RestClient();
                restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomcerce_insertar_terminos_atributos");
                RestRequest restRequest3 = new RestRequest(Method.POST);
                restRequest3.AddHeader("Content-Type", "application/json");
                restRequest3.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse restResponse3 = restClient3.Execute(restRequest3);

                if ((int)restResponse3.StatusCode == 200)
                {

                    mensaje = "Términos de atribtuos agregados con éxito";

                }
                else {

                    mensaje = "Error al agregar términos de atribtuos";

                }


                    return mensaje;

            }
            catch (Exception ex)
            {

                return ex.Message.ToString();
            }

        }

        //Agregar nuevo termino atributo
        public string agregar_nuevo_atributo(Clases.TerminosAtributoWoocomerce obj_atributos)
        {

            string mensaje = "";

            try
            {
                Metodos.MetodosConfiguracionWoocomerce met_configuracion = new Metodos.MetodosConfiguracionWoocomerce();
                string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos

                Clases.TerminosAtributoWoocomerce res_obj_atributos = new Clases.TerminosAtributoWoocomerce();

                //Convertimos el atributo en un nuevo Json para agregar el atributo
                string json_obj_atributos = JsonConvert.SerializeObject(obj_atributos);

                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/attributes/" + obj_atributos.codigo_atributo.ToString() + "/terms");
                RestRequest restRequest = new RestRequest(Method.POST);
                restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                restRequest.AddHeader("Content-Type", "application/json");
                restRequest.AddParameter("application/json", json_obj_atributos, ParameterType.RequestBody);
                IRestResponse restResponse = restClient.Execute(restRequest);

                if ((int)restResponse.StatusCode == 201)
                {

                    res_obj_atributos = JsonConvert.DeserializeObject<Clases.TerminosAtributoWoocomerce>(restResponse.Content);

                    Clases.TerminosAtributoWoocomerce obj = new Clases.TerminosAtributoWoocomerce();
                    obj.codigo_atributo = obj_atributos.codigo_atributo;
                    obj.id = res_obj_atributos.id;
                    obj.name = res_obj_atributos.name;
                    obj.description = res_obj_atributos.description;
                    obj.slug = res_obj_atributos.slug;

                    List<Clases.TerminosAtributoWoocomerce> lista_obj_atributos = new List<Clases.TerminosAtributoWoocomerce>();
                    lista_obj_atributos.Add(obj);

                    string lista_json = JsonConvert.SerializeObject(lista_obj_atributos);

                    RestClient restClient3 = new RestClient();
                    restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomcerce_insertar_terminos_atributos_tablas");
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
                return mensaje + ex.Message.ToString();
            }

        }

        //Actualizar terminos de atributo en woocomerce
        public string obtener_terminos_editados_recientemente() {

            string json = "";

            try
            {
                RestClient restClient3 = new RestClient();
                restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_obtener_terminos_atrubutos_editados");
                RestRequest restRequest3 = new RestRequest(Method.GET);
                IRestResponse restResponse3 = restClient3.Execute(restRequest3);

                if ((int)restResponse3.StatusCode == 200)
                {
                    json = restResponse3.Content;
                }
                   
                return json;

            }
            catch (Exception)
            {
                return json;
            }

        }

        //Agregar nuevo termino atributo
        public string actualizar_terminos_atributo_woocomerce()
        {

            string mensaje = "";

            //Obtenemos los terminos de atributos que fuero editados recientemente
            string json = obtener_terminos_editados_recientemente();

            try
            {
                Metodos.MetodosConfiguracionWoocomerce met_configuracion = new Metodos.MetodosConfiguracionWoocomerce();
                string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos

                List<Clases.TerminosAtributoWoocomerce> lista_terminos = JsonConvert.DeserializeObject<List<Clases.TerminosAtributoWoocomerce>>(json);

                if (lista_terminos.Count > 0)
                {

                    int cont = 0;

                    while (cont < lista_terminos.Count) {

                        //Convertimos el termino de atributos selccionado en un nuevo Json para actualizar
                        string json_obj_atributos = JsonConvert.SerializeObject(lista_terminos[cont]);
                        RestClient restClient = new RestClient();
                        restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/attributes/" + lista_terminos[cont].codigo_atributo.ToString() + "/terms/" + lista_terminos[cont].id.ToString());                
                        RestRequest restRequest = new RestRequest(Method.PUT);
                        restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                        //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                        restRequest.AddHeader("Content-Type", "application/json");
                        restRequest.AddParameter("application/json", json_obj_atributos, ParameterType.RequestBody);
                        IRestResponse restResponse = restClient.Execute(restRequest);

                        if ((int)restResponse.StatusCode == 200)
                        {
                            mensaje = restResponse.Content + restResponse.StatusCode;
                        }
                        else
                        {
                            mensaje = restResponse.Content + restResponse.StatusCode;
                        }


                        cont++;
                    }

                    

                }
                else {

                    mensaje = "Error al sincronixar los términos de atributo";

                }
     

                return mensaje;
            }
            catch (Exception ex)
            {
                return mensaje + ex.Message.ToString();
            }

        }

        //Obtener terminos de atributo en woocomerce con estado !=S
        public string obtener_terminos_estado_inactivo_recientemente()
        {

            string json = "";

            try
            {
                RestClient restClient3 = new RestClient();
                restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomerce_obtener_terminos_atrubutos_estado_eliminado");
                RestRequest restRequest3 = new RestRequest(Method.GET);
                IRestResponse restResponse3 = restClient3.Execute(restRequest3);

                if ((int)restResponse3.StatusCode == 200)
                {
                    json = restResponse3.Content;
                }

                return json;

            }
            catch (Exception)
            {
                return json;
            }

        }

        //Eliminar atribitos de nuestras tablas y de woocomerce
        public string eliminar_atributos_de_nuestras_tablas()
        {

            string mensaje = "";

            int i = 0;

            try
            {

                string json = obtener_terminos_estado_inactivo_recientemente();

                List<Clases.TerminosAtributoWoocomerce> lista_atributos = JsonConvert.DeserializeObject<List<Clases.TerminosAtributoWoocomerce>>(json);

                if (lista_atributos.Count > 0)
                {

                    MetodosConfiguracionWoocomerce met_configuracion = new MetodosConfiguracionWoocomerce();
                    string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                    string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                    string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos

                    

                    while (i < lista_atributos.Count)
                    {


                        string jso_atributo = JsonConvert.SerializeObject(lista_atributos[i]);

                        RestClient restClient = new RestClient();
                        restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/products/attributes/" + lista_atributos[i].codigo_atributo+"/terms/"+ lista_atributos[i].id +"?force=true");
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
                            restClient3.BaseUrl = new Uri("http://" + ip + "/api/woocomcerce_eliminar_terminos_atributos");
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

                return mensaje = "Términos de atributos eliminados con éxito";

            }
            catch (Exception ex)
            {
                i++;
            }

            return mensaje = "Términos de atributos eliminados con éxito";

        }




    }

}


