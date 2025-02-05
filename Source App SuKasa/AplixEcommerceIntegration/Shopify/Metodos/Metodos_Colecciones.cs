using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using AplixEcommerceIntegration.Globales;
using AplixEcommerceIntegration.Shopify.Clases;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using System.Configuration;

namespace AplixEcommerceIntegration.Shopify.Metodos
{
    class Metodos_Colecciones
    {
        //Conexion a base de datos
        Conexion cnn = new Conexion();
        Metodo_Credenciales metodo_Credenciales = new Metodo_Credenciales();

        //Métodos que llenan la tabla de las colecciones 
        public DataTable mostar_colecciones()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.MOSTRAR_COLECCIONES";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                    cnn.CerrarConexion();
                    return dt;
                }
            }
            catch (Exception)
            {
                dt = null;
                cnn.CerrarConexion();
                return dt;
            }
        }

        //Método que agrega una nueva coleccion, esto agrega a nivel de base de datos y a Shopify
        public int agregar_colecciones(string nombre, string descripcion)
        {
            try
            {
                int n = 0;
                //nos traemos los credenciales de conexion
                List<Credenciales> credenciales = metodo_Credenciales.obtener_credenciales();
                if (credenciales == null)
                {
                    return 0;
                }
                else
                {
                    foreach (Credenciales item in credenciales)
                    {
                        //Insertamos la collecion en shopify
                        var restClient = new RestClient("" + item.url_tienda + "");
                        restClient.Timeout = -1;
                        restClient.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);

                        string resource = "/admin/api/2021-01/custom_collections.json";
                        var request = new RestRequest(resource, Method.POST);
                        request.AddHeader("header", "Content-Type: application/json");
                        request.AddParameter("application/json", "{\r\n \"custom_collection\": \r\n    {\r\n    \"title\": \"" + nombre + "\", \r\n    \"body_html\": \"" + descripcion + "\", \r\n    \"published_scope\": \"global\" \r\n    } \r\n}", ParameterType.RequestBody);

                        IRestResponse response = restClient.Execute(request);
                        if (response.StatusCode.ToString() == "Created")
                        {
                            Colecciones json_colecciones = JsonConvert.DeserializeObject<Colecciones>(response.Content);
                            //Insertamos la nueva coleccion
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn.AbrirConexion();
                            cmd.CommandText = "SHOPIFY.INSERTAR_COLECCIONES";
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@NOMBRE", nombre);
                            cmd.Parameters.AddWithValue("@DESCRIPCION", descripcion);
                            cmd.Parameters.AddWithValue("@ID", json_colecciones.custom_collection.id);
                            cmd.ExecuteNonQuery();
                            cnn.CerrarConexion();
                            n = 1;
                        }
                        else
                        {
                            //No se pudo ingresar la coleccion a shopify
                            n = 2;
                        }
                    }
                }
                return n;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //Método que guarda los cambios de las colecciones
        public int guardar_colecciones(string id, string descripcion)
        {
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.ACTUALIZAR_COLECCIONES";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DESCRIPCION", descripcion);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
                cnn.CerrarConexion();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //Método que actualiza la descripcion de las colecciones
        public string actualizar_colecciones()
        {
            try
            {
                //nos traemos las colecciones editadas
                var client = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/actualizar_colecciones");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                IRestResponse response = client.Execute(request);
                if ((int)response.StatusCode == 200)
                {
                    int cont_cole = 0;
                    List<Colecciones> lista_coleccion = JsonConvert.DeserializeObject<List<Colecciones>>(response.Content);
                    if (lista_coleccion.Count > 0)
                    {
                        List<Credenciales> credenciales = metodo_Credenciales.obtener_credenciales();
                        if (credenciales == null)
                        {
                            //error en credenciales
                        }
                        else
                        {
                            foreach (Credenciales item in credenciales)
                            {
                                while (cont_cole < lista_coleccion.Count)
                                {
                                    var restClient = new RestClient("" + item.url_tienda + "");
                                    restClient.Timeout = -1;
                                    restClient.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);

                                    string resource = "/admin/api/2021-01/custom_collections/" + lista_coleccion[cont_cole].custom_collection.id + ".json";
                                    var restRequest = new RestRequest(resource, Method.PUT);
                                    restRequest.AddHeader("header", "Content-Type: application/json");
                                    restRequest.AddParameter("application/json", "{\r\n \"custom_collection\": \r\n    {\r\n    \"id\": \"" + lista_coleccion[cont_cole].custom_collection.id + "\", \r\n    \"body_html\": \"" + lista_coleccion[cont_cole].custom_collection.body_html + "\"\r\n    } \r\n}", ParameterType.RequestBody);

                                    IRestResponse restResponse = restClient.Execute(restRequest);
                                    cont_cole++;
                                }
                            }
                            //actualizamos fecha
                            var client2 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/actualizar_fecha");
                            client2.Timeout = -1;
                            var request2 = new RestRequest(Method.GET);
                            IRestResponse response2 = client2.Execute(request2);
                        }
                    }
                    else
                    {
                        //la lista viene vacia
                        return "lista";
                    }
                }
                else
                {
                    //fallo en el API local
                    return "API";
                }
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        //Método que obtiene las colecciones y las ingresa en nuestras tablas
        public int obtener_colecciones()
        {
            try
            {
                int n = 0;
                //nos traemos los credenciales de conexion
                List<Credenciales> credenciales = metodo_Credenciales.obtener_credenciales();
                if (credenciales == null)
                {
                    return 3;
                }
                else
                {
                    foreach (Credenciales item in credenciales)
                    {
                        //Insertamos la collecion en shopify
                        var restClient = new RestClient("" + item.url_tienda + "");
                        restClient.Timeout = -1;
                        restClient.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);

                        string resource = "/admin/api/2021-01/custom_collections.json?limit=250";
                        var request = new RestRequest(resource, Method.GET);
                        request.AddHeader("header", "Content-Type: application/json");

                        IRestResponse response = restClient.Execute(request);
                        if (response.StatusCode.ToString() == "OK")
                        {
                            Collection json_colecciones = JsonConvert.DeserializeObject<Collection>(response.Content);

                            if (json_colecciones.custom_collections.Count > 0)
                            {
                                //eliminamos las colecciones que están existentes
                                SqlCommand scmd = new SqlCommand();
                                scmd.Connection = cnn.AbrirConexion();
                                scmd.CommandText = "SHOPIFY.ELIMINAR_COLECCIONES_TIENDA";
                                scmd.CommandTimeout = 0;
                                scmd.CommandType = CommandType.StoredProcedure;
                                scmd.ExecuteNonQuery();
                                cnn.CerrarConexion();

                                int i = 0;
                                while (i < json_colecciones.custom_collections.Count)
                                {
                                    //Insertamos las nuevas colecciones
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.Connection = cnn.AbrirConexion();
                                    cmd.CommandText = "SHOPIFY.INSERTAR_COLECCIONES_TIENDA";
                                    cmd.CommandTimeout = 0;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@NOMBRE", json_colecciones.custom_collections[i].title);
                                    if (json_colecciones.custom_collections[i].body_html == null)
                                    {
                                        cmd.Parameters.AddWithValue("@DESCRIPCION", "");
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@DESCRIPCION", json_colecciones.custom_collections[i].body_html);
                                    }
                                    cmd.Parameters.AddWithValue("@ID", json_colecciones.custom_collections[i].id);
                                    cmd.ExecuteNonQuery();
                                    cnn.CerrarConexion();
                                    i++;
                                }
                                n = 1;
                            }
                        }
                        else
                        {
                            //No se pudo obtener la coleccion de shopify
                            n = 2;
                        }
                    }
                }
                return n;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //Método que elimina las colecciones seleccionadas
        public int eliminar_colecciones(string coleccion)
        {
            try
            {
                int n = 0;
                //nos traemos los credenciales de conexion
                List<Credenciales> credenciales = metodo_Credenciales.obtener_credenciales();
                if (credenciales == null)
                {
                    return 0;
                }
                else
                {
                    foreach (Credenciales item in credenciales)
                    {
                        //Insertamos la collecion en shopify
                        var restClient = new RestClient("" + item.url_tienda + "");
                        restClient.Timeout = -1;
                        restClient.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);

                        string resource = "/admin/api/2021-01/custom_collections/"+ coleccion + ".json";
                        var request = new RestRequest(resource, Method.DELETE);
                        request.AddHeader("header", "Content-Type: application/json");

                        IRestResponse response = restClient.Execute(request);
                        if (response.StatusCode.ToString() == "OK")
                        {
                            //Insertamos la nueva coleccion
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn.AbrirConexion();
                            cmd.CommandText = "SHOPIFY.ELIMINAR_COLECCIONES";
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", coleccion);
                            cmd.ExecuteNonQuery();
                            cnn.CerrarConexion();
                            n = 1;
                        }
                        else
                        {
                            //No se pudo eliminar la coleccion de shopify
                            n = 2;
                        }
                    }
                }
                return n;
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}
