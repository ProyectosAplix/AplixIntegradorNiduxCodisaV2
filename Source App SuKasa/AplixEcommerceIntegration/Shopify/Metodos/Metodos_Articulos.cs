using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplixEcommerceIntegration.Globales;
using System.Data;
using System.Data.SqlClient;
using RestSharp;
using System.Configuration;
using Newtonsoft.Json;
using AplixEcommerceIntegration.Shopify.Clases;
using RestSharp.Authenticators;
using System.Text.RegularExpressions;

namespace AplixEcommerceIntegration.Shopify.Metodos
{
    class Metodos_Articulos
    {
        //Conexion a base de datos
        Conexion cnn = new Conexion();
        Metodo_Credenciales metodo_credenciales = new Metodo_Credenciales();

        //Métodos que llenan la tabla de los articulos
        public DataTable articulos()
        {
            DataTable dt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.MOSTRAR_ARTICULOS";
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

        //Método que llena el combobox para la tabla de articulos como para el mantenimiento
        public DataTable articulos_estados()
        {
            DataTable dt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT ID, DESCRIPCION FROM SHOPIFY.ARTICULOS_ESTADO";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
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

        //Método que llena el combobox para las colecciones
        public DataTable articulos_colecciones()
        {
            DataTable dt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT NOMBRE FROM SHOPIFY.COLECCIONES";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
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

        //Métodos que llenan la tabla de los articulos pero en mantenimiento
        public DataTable articulos_nuevos()
        {
            DataTable dt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.MOSTRAR_ARTICULOS_NO_ACTIVOS";
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

        //Métodos que llenan la tabla de los articulos que se van hacer variantes
        public DataTable articulos_variantes()
        {
            DataTable dt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.MOSTRAR_ARTICULOS_TODOS";//
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

        //Métodos que llenan la tabla de las colecciones en mantenimiento de articulos
        public DataTable articulos_coleccion_mantenimiento()
        {
            DataTable dt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.MOSTRAR_COLECCIONES_MANTENIMIENTO";
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

        //Método que nos trae el valor del impuesto del articulo seleccionado
        public string articulo_impuesto(string sku)
        {
            try
            {
                string impuesto = "";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.MOSTRAR_IMPUESTO_ARTICULO";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SKU", sku);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    impuesto = rd["IMPUESTO"].ToString();
                }
                rd.Close();
                return impuesto;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        //Metodo que actualiza la informacion del articulo base
        public string actualizar_articulos(string nombre, string descripcion, int estado, string impuesto, string sku)
        {
            try
            {
                //limpiamos la descripcion
                string body_html = descripcion.Replace("\r\n", " ");
                string body_html_limpio = body_html.Replace("\t", " ");
                //Actualizamos la opcion de valor
                SqlCommand scmd = new SqlCommand();
                scmd.Connection = cnn.AbrirConexion();
                scmd.CommandText = "SHOPIFY.ACTUALIZAR_ARTICULOS";
                scmd.CommandTimeout = 0;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@NOMBRE", nombre);
                scmd.Parameters.AddWithValue("@DESCRIPCION", body_html_limpio);
                scmd.Parameters.AddWithValue("@ESTADO", estado);
                scmd.Parameters.AddWithValue("@IMPUESTO", impuesto);
                scmd.Parameters.AddWithValue("@SKU", sku);
                scmd.ExecuteNonQuery();
                cnn.CerrarConexion();
                return "exito";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        //Metodo que ingresa las variantes del articulo
        public string insertar_variantes(string sku_variante, string sku_articulo, string variantes)
        {
            try
            {
                string[] opciones_lista = variantes.Split(',');
                int opcion_1 = 0;
                int opcion_2 = 0;
                int opcion_3 = 0;
                if (opciones_lista.Length == 3)
                {
                    opcion_1 = Convert.ToInt32(opciones_lista[0]);
                    opcion_2 = Convert.ToInt32(opciones_lista[1]);
                    opcion_3 = Convert.ToInt32(opciones_lista[2]);
                }
                else if (opciones_lista.Length == 2)
                {
                    opcion_1 = Convert.ToInt32(opciones_lista[0]);
                    opcion_2 = Convert.ToInt32(opciones_lista[1]);
                }
                else
                {
                    opcion_1 = Convert.ToInt32(opciones_lista[0]);
                }

                SqlCommand scmd = new SqlCommand();
                scmd.Connection = cnn.AbrirConexion();
                scmd.CommandText = "SHOPIFY.INSERTAR_VARIANTES";
                scmd.CommandTimeout = 0;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@SKU_VARIANTE", sku_variante);
                scmd.Parameters.AddWithValue("@SKU_ARTICULO", sku_articulo);
                scmd.Parameters.AddWithValue("@OPCION_1", opcion_1);
                scmd.Parameters.AddWithValue("@OPCION_2", opcion_2);
                scmd.Parameters.AddWithValue("@OPCION_3", opcion_3);
                scmd.ExecuteNonQuery();
                cnn.CerrarConexion();
                return "exito";
            }
            catch (Exception ex)
            {
                return "Error en variante: " + sku_variante + ", mensaje de Error: " + ex.Message.ToString();
            }
        }

        //Metodo que ingresa las colecciones del articulo
        public string insertar_colecciones(string coleccion, string articulo)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.INSERTAR_COLECCIONES_ARTICULO";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COLECCION", coleccion);
                cmd.Parameters.AddWithValue("@SKU_ARTICULO", articulo);
                cmd.ExecuteNonQuery();
                cnn.CerrarConexion();
                return "exito";
            }
            catch (Exception ex)
            {
                return "Error en Coleccion: " + coleccion + ", mensaje de Error: " + ex.Message.ToString();
            }
        }

        //Metodo que sincroniza los articulos sin variantes
        public string sincronizar_articulos_sin_variantes()//
        {
            try
            {
                //nos traemos los articulos sin variantes
                var client = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/agregar_articulos_sin_variantes");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                IRestResponse response = client.Execute(request);
                if ((int)response.StatusCode == 200)
                {
                    int cont = 0;
                    List<Products> lista_producto = JsonConvert.DeserializeObject<List<Products>>(response.Content);
                    if (lista_producto.Count > 0)
                    {
                        List<Credenciales> credenciales = metodo_credenciales.obtener_credenciales();
                        if (credenciales == null)
                        {
                            //error en credenciales
                        }
                        else
                        {
                            foreach (Credenciales item in credenciales)
                            {
                                while (cont < lista_producto.Count)
                                {
                                    var restClient = new RestClient("" + item.url_tienda + "");
                                    restClient.Timeout = -1;
                                    restClient.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);

                                    string resource = "/admin/api/2021-01/products.json";
                                    var restRequest = new RestRequest(resource, Method.POST);
                                    restRequest.AddHeader("header", "Content-Type: application/json");

                                    string json = "{\r\n    \"product\": {\r\n        \"title\": \"" + lista_producto[cont].title + "\",\r\n        \"body_html\": \"" + lista_producto[cont].body_html + "\",\r\n        \"status\": \"" + lista_producto[cont].status +"\",\r\n        \"published_scope\": \"" + lista_producto[cont].published_scope +"\",\r\n        \"variants\": [\r\n            {\r\n                \"price\": \"" + lista_producto[cont].variants[0].price.Remove(lista_producto[cont].variants[0].price.Length - 6) + "\",\r\n                \"sku\": \"" + lista_producto[cont].variants[0].sku +"\",\r\n                \"inventory_policy\": \"" + lista_producto[cont].variants[0].inventory_policy +"\",\r\n                \"compare_at_price\": \"" + lista_producto[cont].variants[0].compare_at_price.Remove(lista_producto[cont].variants[0].compare_at_price.Length - 6) +"\",\r\n                \"inventory_management\": \"" + lista_producto[cont].variants[0].inventory_management +"\",\r\n                \"taxable\": " + lista_producto[cont].variants[0].taxable +",\r\n                \"weight\": " + Convert.ToInt32(Convert.ToDecimal(lista_producto[cont].variants[0].weight))  +",\r\n                \"weight_unit\": \"" + lista_producto[cont].variants[0].weight_unit + "\",\r\n                \"inventory_quantity\": " + Convert.ToInt32(Convert.ToDecimal(lista_producto[cont].variants[0].inventory_quantity)) + "\r\n            }\r\n        ]\r\n    }\r\n}";

                                    restRequest.AddParameter("application/json", json, ParameterType.RequestBody);

                                    IRestResponse restResponse = restClient.Execute(restRequest);

                                    if (restResponse.StatusCode.ToString() == "Created")
                                    {
                                        respuesta respuesta = JsonConvert.DeserializeObject<respuesta>(restResponse.Content);
                                        SqlCommand cmd = new SqlCommand();
                                        cmd.Connection = cnn.AbrirConexion();
                                        cmd.CommandText = "SHOPIFY.ACTUALIZAR_ID_SHOPIFY";
                                        cmd.CommandTimeout = 0;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@ID", respuesta.product.id);
                                        cmd.Parameters.AddWithValue("@SKU", lista_producto[cont].variants[0].sku);
                                        cmd.ExecuteNonQuery();
                                        cnn.CerrarConexion();
                                    }
                                    cont++;
                                }
                            }
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

        //Metodo que sincroniza los articulos con Variantes
        public string sincronizar_articulos_con_variantes()//
        {
            try
            {
                //nos traemos los articulos padres con variantes
                var client = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/agregar_articulos_con_variantes");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                IRestResponse response = client.Execute(request);
                if ((int)response.StatusCode == 200)
                {
                    int cont = 0;
                    List<Articulos_Padre> lista_padres = JsonConvert.DeserializeObject<List<Articulos_Padre>>(response.Content);
                    if (lista_padres.Count > 0)
                    {
                        List<Credenciales> credenciales = metodo_credenciales.obtener_credenciales();
                        if (credenciales == null)
                        {
                            //error en credenciales
                        }
                        else
                        {
                            foreach (Credenciales item in credenciales)
                            {
                                while (cont < lista_padres.Count)
                                {
                                    //llenado de encabezado
                                    Product_Variantes encabezado = new Product_Variantes();
                                    encabezado.title = lista_padres[cont].title;
                                    encabezado.body_html = lista_padres[cont].body_html;
                                    encabezado.status = lista_padres[cont].status;
                                    encabezado.published_scope = lista_padres[cont].published_scope;

                                    //por cada articulo padre vamos a traernos la informacion de las variantes
                                    var cliente_variantes = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/info_variantes_articulos/" + lista_padres[cont].sku);
                                    cliente_variantes.Timeout = -1;
                                    var request_variantes = new RestRequest(Method.GET);

                                    IRestResponse response_variantes = cliente_variantes.Execute(request_variantes);
                                    if ((int)response_variantes.StatusCode == 200)
                                    {
                                        List<Variant_Variantes> lista_variantes = JsonConvert.DeserializeObject<List<Variant_Variantes>>(response_variantes.Content);
                                        encabezado.variants = lista_variantes;

                                        //por cada articulo variante nos vamos a traer sus opciones
                                        var cliente_opciones = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/variantes_articulos_options/" + lista_padres[cont].sku);
                                        cliente_opciones.Timeout = -1;
                                        var request_opciones = new RestRequest(Method.GET);

                                        IRestResponse response_opciones = cliente_opciones.Execute(request_opciones);
                                        if ((int)response_opciones.StatusCode == 200)
                                        {
                                            List<Option_Variantes> lista_opciones = JsonConvert.DeserializeObject<List<Option_Variantes>>(response_opciones.Content);
                                            encabezado.options = lista_opciones;

                                            Articulos_Variantes lista_articulos = new Articulos_Variantes();
                                            lista_articulos.product = encabezado;

                                            string json = JsonConvert.SerializeObject(lista_articulos);

                                            //insertamos en shopify
                                            var restClient = new RestClient("" + item.url_tienda + "");
                                            restClient.Timeout = -1;
                                            restClient.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);

                                            string resource = "/admin/api/2021-01/products.json";
                                            var restRequest = new RestRequest(resource, Method.POST);
                                            restRequest.AddHeader("header", "Content-Type: application/json");

                                            restRequest.AddParameter("application/json", json, ParameterType.RequestBody);

                                            IRestResponse restResponse = restClient.Execute(restRequest);

                                            if (restResponse.StatusCode.ToString() == "Created")
                                            {
                                                respuesta respuesta = JsonConvert.DeserializeObject<respuesta>(restResponse.Content);

                                                //actualizamos el id del padre
                                                SqlCommand cmd = new SqlCommand();
                                                cmd.Connection = cnn.AbrirConexion();
                                                cmd.CommandText = "SHOPIFY.ACTUALIZAR_ID_SHOPIFY";
                                                cmd.CommandTimeout = 0;
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@ID", respuesta.product.id);
                                                cmd.Parameters.AddWithValue("@SKU", lista_padres[cont].sku);
                                                cmd.ExecuteNonQuery();
                                                cnn.CerrarConexion();

                                                int i = 0;
                                                if (respuesta.product.variants.Count > 0)
                                                {
                                                    while (i < respuesta.product.variants.Count)
                                                    {
                                                        //actualizar los id de las variantes
                                                        SqlCommand scmd = new SqlCommand();
                                                        scmd.Connection = cnn.AbrirConexion();
                                                        scmd.CommandText = "SHOPIFY.ACTUALIZAR_ID_SHOPIFY_VARIANTES";
                                                        scmd.CommandTimeout = 0;
                                                        scmd.CommandType = CommandType.StoredProcedure;
                                                        scmd.Parameters.AddWithValue("@ID", respuesta.product.variants[i].id);
                                                        scmd.Parameters.AddWithValue("@SKU", respuesta.product.variants[i].sku);
                                                        scmd.ExecuteNonQuery();
                                                        cnn.CerrarConexion();
                                                        i++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    cont++;
                                }
                            }
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

        //Metodo que sincroniza las colecciones de los articulos
        public string sincronizar_colecciones_articulos()
        {
            try
            {
                //nos traemos los articulos padres con variantes
                var client = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/coleccion_articulos");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                IRestResponse response = client.Execute(request);
                if ((int)response.StatusCode == 200)
                {
                    int cont = 0;
                    List<collect> lista_colecion = JsonConvert.DeserializeObject<List<collect>>(response.Content);
                    if (lista_colecion.Count > 0)
                    {
                        List<Credenciales> credenciales = metodo_credenciales.obtener_credenciales();
                        if (credenciales == null)
                        {
                            //error en credenciales
                        }
                        else
                        {
                            foreach (Credenciales item in credenciales)
                            {
                                while (cont < lista_colecion.Count)
                                {
                                    var restClient = new RestClient("" + item.url_tienda + "");
                                    restClient.Timeout = -1;
                                    restClient.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);

                                    string resource = "/admin/api/2021-01/collects.json";
                                    var restRequest = new RestRequest(resource, Method.POST);
                                    restRequest.AddHeader("header", "Content-Type: application/json");

                                    string json = "{\r\n  \"collect\": {\r\n    \"product_id\": " + lista_colecion[cont].product_id + ",\r\n    \"collection_id\": " + lista_colecion[cont].collection_id + "\r\n  }\r\n}";

                                    restRequest.AddParameter("application/json", json, ParameterType.RequestBody);

                                    IRestResponse restResponse = restClient.Execute(restRequest);

                                    if (restResponse.StatusCode.ToString() == "Created")//422
                                    {
                                        respuesta_coleccion lista_coleccion_respuesta = JsonConvert.DeserializeObject<respuesta_coleccion>(restResponse.Content);
                                        //actualizamos el id del padre
                                        SqlCommand cmd = new SqlCommand();
                                        cmd.Connection = cnn.AbrirConexion();
                                        cmd.CommandText = "SHOPIFY.ACTUALIZAR_ID_SHOPIFY_COLECCION";
                                        cmd.CommandTimeout = 0;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@ID", lista_coleccion_respuesta.collect.id);
                                        cmd.Parameters.AddWithValue("@SKU_ARTICULO", lista_colecion[cont].sku);
                                        cmd.Parameters.AddWithValue("@COLECCION", lista_colecion[cont].collection_id);
                                        cmd.ExecuteNonQuery();
                                        cnn.CerrarConexion();
                                    }
                                    cont++;
                                }
                            }
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

        public void actualizar_fecha()
        {
            //actualizamos fecha
            var client = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/actualizar_fecha");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
        }

        //Metodo que nos trae los datos de las colecciones del articulo
        public DataTable articulos_editados_colecciones(string sku)
        {
            DataTable dt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.MOSTRAR_ARTICULOS_EDITADOS_COLECCION";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SKU", sku);
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

        //Metodo que nos trae los datos de las variantes del articulo
        public DataTable articulos_editados_variantes(string sku)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.MOSTRAR_ARTICULOS_EDITADOS_VARIANTES";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SKU", sku);
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

        //Metodo que nos trae los datos de las articulos
        public DataTable articulos_editados(string sku)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.MOSTRAR_ARTICULOS_EDITADOS";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SKU", sku);
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

        //Metodo que actualiza las colecciones
        public string actualizar_colecciones(string coleccion, string articulo, string activo)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.ACTUALIZAR_COLECCIONES_ARTICULO";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COLECCION", coleccion);
                cmd.Parameters.AddWithValue("@SKU_ARTICULO", articulo);
                cmd.Parameters.AddWithValue("@ACTIVO", activo);
                cmd.ExecuteNonQuery();
                cnn.CerrarConexion();
                return "exito";
            }
            catch (Exception ex)
            {
                return "Error en Coleccion: " + coleccion + ", mensaje de Error: " + ex.Message.ToString();
            }
        }

        //Metodo que actualiza las variantes del articulo
        public string actualizar_variantes(string sku_variante, string sku_articulo, string variantes, string activo)
        {
            try
            {
                string[] opciones_lista = variantes.Split(',');
                int opcion_1 = 0;
                int opcion_2 = 0;
                int opcion_3 = 0;
                if (opciones_lista.Length == 3)
                {
                    opcion_1 = Convert.ToInt32(opciones_lista[0]);
                    opcion_2 = Convert.ToInt32(opciones_lista[1]);
                    opcion_3 = Convert.ToInt32(opciones_lista[2]);
                }
                else if (opciones_lista.Length == 2)
                {
                    opcion_1 = Convert.ToInt32(opciones_lista[0]);
                    opcion_2 = Convert.ToInt32(opciones_lista[1]);
                }
                else
                {
                    opcion_1 = Convert.ToInt32(opciones_lista[0]);
                }

                SqlCommand scmd = new SqlCommand();
                scmd.Connection = cnn.AbrirConexion();
                scmd.CommandText = "SHOPIFY.ACTUALIZAR_VARIANTES";
                scmd.CommandTimeout = 0;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@SKU_VARIANTE", sku_variante);
                scmd.Parameters.AddWithValue("@SKU_ARTICULO", sku_articulo);
                scmd.Parameters.AddWithValue("@OPCION_1", opcion_1);
                scmd.Parameters.AddWithValue("@OPCION_2", opcion_2);
                scmd.Parameters.AddWithValue("@OPCION_3", opcion_3);
                scmd.Parameters.AddWithValue("@ACTIVO", activo);
                scmd.ExecuteNonQuery();
                cnn.CerrarConexion();
                return "exito";
            }
            catch (Exception ex)
            {
                return "Error en variante: " + sku_variante + ", mensaje de Error: " + ex.Message.ToString();
            }
        }

        //Metodo que sincroniza los articulos sin variantes que se han modificado
        public string actualizar_articulos_sin_variantes(string sku)
        {
            try
            {
                //nos traemos los articulos sin variantes
                var client = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/actualizar_articulos_sin_variantes/" + sku);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                IRestResponse response = client.Execute(request);
                if ((int)response.StatusCode == 200)
                {
                    productos producto = JsonConvert.DeserializeObject<productos>(response.Content);
                    if (producto.product != null)
                    {
                        List<Credenciales> credenciales = metodo_credenciales.obtener_credenciales();
                        if (credenciales == null)
                        {
                            //error en credenciales
                        }
                        else
                        {
                            foreach (Credenciales item in credenciales)
                            {
                                var restClient = new RestClient("" + item.url_tienda + "");
                                restClient.Timeout = -1;
                                restClient.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);
                                long id = producto.product.id;
                                string resource = "/admin/api/2021-01/products/" + id.ToString() +".json";
                                var restRequest = new RestRequest(resource, Method.PUT);
                                restRequest.AddHeader("header", "Content-Type: application/json");

                                string json = JsonConvert.SerializeObject(producto);

                                restRequest.AddParameter("application/json", json, ParameterType.RequestBody);

                                IRestResponse restResponse = restClient.Execute(restRequest);

                                if (restResponse.StatusCode.ToString() == "OK")
                                {
                                   
                                }
                            }
                        }
                    }
                }
                else
                {
                    return "API";
                }
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        //Metodo que sincroniza los articulos con variantes que se han modificado
        public string actualizar_articulos_con_variantes(string sku)
        {
            try
            {
                //nos traemos los articulos sin variantes
                var client = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/actualizar_articulos_con_variantes/" + sku);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                IRestResponse response = client.Execute(request);
                if ((int)response.StatusCode == 200)
                {
                    productos_variantes producto = JsonConvert.DeserializeObject<productos_variantes>(response.Content);
                    if (producto.product != null)
                    {
                        List<Credenciales> credenciales = metodo_credenciales.obtener_credenciales();
                        if (credenciales == null)
                        {
                            //error en credenciales
                        }
                        else
                        {
                            foreach (Credenciales item in credenciales)
                            {
                                var restClient = new RestClient("" + item.url_tienda + "");
                                restClient.Timeout = -1;
                                restClient.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);
                                long id = producto.product.id;
                                string resource = "/admin/api/2021-01/products/" + id.ToString() + ".json";
                                var restRequest = new RestRequest(resource, Method.PUT);
                                restRequest.AddHeader("header", "Content-Type: application/json");

                                string json = JsonConvert.SerializeObject(producto);

                                restRequest.AddParameter("application/json", json, ParameterType.RequestBody);

                                IRestResponse restResponse = restClient.Execute(restRequest);

                                if (restResponse.StatusCode.ToString() == "OK")
                                {
                                    //actualizar el id de la variante
                                    respuesta respuesta = JsonConvert.DeserializeObject<respuesta>(restResponse.Content);
                                    int i = 0;
                                    if (respuesta.product.variants.Count > 0)
                                    {
                                        while (i < respuesta.product.variants.Count)
                                        {
                                            //actualizar los id de las variantes
                                            SqlCommand scmd = new SqlCommand();
                                            scmd.Connection = cnn.AbrirConexion();
                                            scmd.CommandText = "SHOPIFY.ACTUALIZAR_ID_SHOPIFY_VARIANTES";
                                            scmd.CommandTimeout = 0;
                                            scmd.CommandType = CommandType.StoredProcedure;
                                            scmd.Parameters.AddWithValue("@ID", respuesta.product.variants[i].id);
                                            scmd.Parameters.AddWithValue("@SKU", respuesta.product.variants[i].sku);
                                            scmd.ExecuteNonQuery();
                                            cnn.CerrarConexion();
                                            i++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    return "API";
                }
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        //Metodo que elimina las colecciones asignadas al articulo ya sea con variantes o sin variantes
        public string eliminar_articulos_colecciones(string sku)
        {
            try
            {
                //nos traemos las colecciones del articulo que se van a eliminar
                var client = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/coleccion_eliminar/" + sku);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                IRestResponse response = client.Execute(request);
                if ((int)response.StatusCode == 200)
                {
                    int cont = 0;
                    List<collect> lista_colecion = JsonConvert.DeserializeObject<List<collect>>(response.Content);
                    if (lista_colecion.Count > 0)
                    {
                        List<Credenciales> credenciales = metodo_credenciales.obtener_credenciales();
                        if (credenciales == null)
                        {
                            //error en credenciales
                        }
                        else
                        {
                            foreach (Credenciales item in credenciales)
                            {
                                while (cont < lista_colecion.Count)
                                {
                                    var restClient = new RestClient("" + item.url_tienda + "");
                                    restClient.Timeout = -1;
                                    restClient.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);

                                    string resource = "/admin/api/2021-01/collects/" + lista_colecion[cont].product_id + ".json";
                                    var restRequest = new RestRequest(resource, Method.DELETE);
                                    restRequest.AddHeader("header", "Content-Type: application/json");

                                    IRestResponse restResponse = restClient.Execute(restRequest);

                                    if (restResponse.StatusCode.ToString() == "OK")
                                    {
                                        //Eliminamos de la DB
                                        SqlCommand cmd = new SqlCommand();
                                        cmd.Connection = cnn.AbrirConexion();
                                        cmd.CommandText = "SHOPIFY.ELIMINAR_COLECCIONES_ASIGNADAS";
                                        cmd.CommandTimeout = 0;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@ID", lista_colecion[cont].product_id);
                                        cmd.ExecuteNonQuery();
                                        cnn.CerrarConexion();
                                    }
                                    cont++;
                                }
                            }
                        }
                    }
                }
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        //metodo que elimina las variantes de shopify y tablas propias
        public string eliminar_variantes(string sku)
        {
            try
            {
                //nos traemos los articulos sin variantes
                var client = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/variantes_eliminar/" + sku);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                IRestResponse response = client.Execute(request);
                if ((int)response.StatusCode == 200)
                {
                    List<Variantes_Delete> producto = JsonConvert.DeserializeObject<List<Variantes_Delete>>(response.Content);
                    if (producto.Count > 0)
                    {
                        List<Credenciales> credenciales = metodo_credenciales.obtener_credenciales();
                        if (credenciales == null)
                        {
                            //error en credenciales
                        }
                        else
                        {
                            foreach (Credenciales item in credenciales)
                            {
                                int cont = 0;
                                while (cont < producto.Count)
                                {
                                    //eliminamos de shopify
                                    var restClient = new RestClient("" + item.url_tienda + "");
                                    restClient.Timeout = -1;
                                    restClient.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);
                                    string resource = "/admin/api/2021-01/products/" + producto[cont].articulo + "/variants/" + producto[cont].variante +".json";
                                    var restRequest = new RestRequest(resource, Method.DELETE);
                                    //restRequest.AddHeader("header", "Content-Type: application/json");

                                    IRestResponse restResponse = restClient.Execute(restRequest);

                                    if (restResponse.StatusCode.ToString() == "OK")
                                    {
                                        //Eliminamos de la DB
                                        SqlCommand cmd = new SqlCommand();
                                        cmd.Connection = cnn.AbrirConexion();
                                        cmd.CommandText = "SHOPIFY.ELIMINAR_VARIANTES_ASIGNADAS";
                                        cmd.CommandTimeout = 0;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@ID", producto[cont].variante);
                                        cmd.ExecuteNonQuery();
                                        cnn.CerrarConexion();
                                    }
                                    cont++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    return "API";
                }
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        //Metodo que se trae los articulos de la tienda para ser ingresados en las tablas
        public string obtener_articulos_tienda()
        {
            try
            {
                const string reduceMultiSpace = @"[ ]{2,}";
                List<Credenciales> credenciales = metodo_credenciales.obtener_credenciales();
                if (credenciales == null)
                {
                    return "Error en Login";
                }
                else
                {
                    foreach (Credenciales item in credenciales)
                    {
                        bool indicador = true;
                        long id = 0;
                        while (indicador == true)
                        {
                            var restClient = new RestClient("" + item.url_tienda + "");
                            restClient.Timeout = -1;
                            restClient.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);

                            string resource = "/admin/api/2021-01/products.json?limit=1&fields=id,title,body_html,status,variants,options&since_id=" + id;
                            var request = new RestRequest(resource, Method.GET);
                            request.AddHeader("header", "Content-Type: application/json");

                            IRestResponse response = restClient.Execute(request);
                            if (response.StatusCode.ToString() == "OK")
                            {
                                shopify json_products = JsonConvert.DeserializeObject<shopify>(response.Content);

                                if (json_products.products.Count > 0)
                                {
                                    id = json_products.products[0].id;

                                    if (json_products.products[0].variants.Count > 1)
                                    {
                                        //productos con variantes
                                        //ingresamos las opciones
                                        int op = 0;
                                        while (op < json_products.products[0].options.Count)
                                        {

                                            string opcion = json_products.products[0].options[op].name;
                                            string [] all_values = json_products.products[0].options[op].values;
                                            string opcion1 = "";
                                            string opcion2 = "";
                                            string opcion3 = "";
                                            if (all_values.Length == 3)
                                            {
                                                opcion1 = all_values[0];
                                                opcion2 = all_values[1];
                                                opcion3 = all_values[2];
                                            }
                                            else if (all_values.Length == 2)
                                            {
                                                opcion1 = all_values[0];
                                                opcion2 = all_values[1];
                                            }
                                            else
                                            {
                                                opcion1 = all_values[0];
                                            }

                                            SqlCommand cmd = new SqlCommand();
                                            cmd.Connection = cnn.AbrirConexion();
                                            cmd.CommandText = "SHOPIFY.INSERTAR_OPCIONES_ARTICULOS_TIENDA";
                                            cmd.CommandTimeout = 0;
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@NOMBRE", opcion);
                                            cmd.Parameters.AddWithValue("@OPCION1", opcion1);
                                            cmd.Parameters.AddWithValue("@OPCION2", opcion2);
                                            cmd.Parameters.AddWithValue("@OPCION3", opcion3);
                                            cmd.ExecuteNonQuery();
                                            cnn.CerrarConexion();
                                            op++;
                                        }

                                        //ingresar articulos
                                        int art = 0;
                                        while (art < json_products.products[0].variants.Count)
                                        {
                                            //posicion 1 sera siempre el articulo padre en ERP y se tiene que ingresar como variante tambien
                                            if (json_products.products[0].variants[art].position == 1)
                                            {
                                                //se inserta como padre y variante
                                                SqlCommand cmd = new SqlCommand();
                                                cmd.Connection = cnn.AbrirConexion();
                                                cmd.CommandText = "SHOPIFY.INSERTAR_ARTICULOS_TIENDA";
                                                cmd.CommandTimeout = 0;
                                                cmd.CommandType = CommandType.StoredProcedure;

                                                cmd.Parameters.AddWithValue("@ID_SHOPIFY", json_products.products[0].id.ToString());
                                                cmd.Parameters.AddWithValue("@NOMBRE", json_products.products[0].title.ToString());
                                                cmd.Parameters.AddWithValue("@NOMBRE_SHOPIFY", json_products.products[0].title.ToString());

                                                if (json_products.products[0].body_html.ToString() == null)
                                                {
                                                    cmd.Parameters.AddWithValue("@DESCRIPCION", "");
                                                    cmd.Parameters.AddWithValue("@DESCRIPCION_SHOPIFY", "");
                                                }
                                                else
                                                {
                                                    string nombre = (Regex.Replace((json_products.products[0].body_html), @"<[^>]+>|&nbsp;", String.Empty));
                                                    string descripcion = Regex.Replace(nombre.Replace("\t", " "), reduceMultiSpace, " ");
                                                    cmd.Parameters.AddWithValue("@DESCRIPCION", descripcion);
                                                    cmd.Parameters.AddWithValue("@DESCRIPCION_SHOPIFY", descripcion);
                                                }
                                                cmd.Parameters.AddWithValue("@SKU", json_products.products[0].variants[art].sku);
                                                cmd.Parameters.AddWithValue("@PESO", json_products.products[0].variants[art].weight);
                                                cmd.Parameters.AddWithValue("@ESTADO", json_products.products[0].status);
                                                cmd.Parameters.AddWithValue("@CANTIDAD", json_products.products[0].variants[art].inventory_quantity);
                                                cmd.Parameters.AddWithValue("@PRECIO", json_products.products[0].variants[art].price);
                                                if (json_products.products[0].variants[art].compare_at_price == null)
                                                {
                                                    cmd.Parameters.AddWithValue("@DESCUENTO", "0.0");
                                                }
                                                else
                                                {
                                                    cmd.Parameters.AddWithValue("@DESCUENTO", json_products.products[0].variants[art].compare_at_price);
                                                }
                                                if (json_products.products[0].variants[art].taxable == "true")
                                                {
                                                    cmd.Parameters.AddWithValue("@IMPUESTO", "S");
                                                }
                                                else
                                                {
                                                    cmd.Parameters.AddWithValue("@IMPUESTO", "N");
                                                }
                                                cmd.ExecuteNonQuery();
                                                cnn.CerrarConexion();

                                                //insertamos en variantes para position 1
                                                SqlCommand cmds = new SqlCommand();
                                                cmds.Connection = cnn.AbrirConexion();
                                                cmds.CommandText = "SHOPIFY.INSERTA_ARTICULOS_VARIANTES_TIENDA";
                                                cmds.CommandTimeout = 0;
                                                cmds.CommandType = CommandType.StoredProcedure;
                                                cmds.Parameters.AddWithValue("@SKU_VARIANTE", json_products.products[0].variants[art].sku);
                                                cmds.Parameters.AddWithValue("@SKU_ARTICULO", json_products.products[0].variants[art].sku);
                                                cmds.Parameters.AddWithValue("@ID_SHOPIFY", json_products.products[0].variants[art].id);
                                                if (json_products.products[0].variants[art].option1 == null)
                                                {
                                                    cmds.Parameters.AddWithValue("@OPCION1", "");
                                                }
                                                else
                                                {
                                                    cmds.Parameters.AddWithValue("@OPCION1", json_products.products[0].variants[art].option1);
                                                }
                                                if (json_products.products[0].variants[art].option2 == null)
                                                {
                                                    cmds.Parameters.AddWithValue("@OPCION2", "");
                                                }
                                                else
                                                {
                                                    cmds.Parameters.AddWithValue("@OPCION2", json_products.products[0].variants[art].option2);
                                                }
                                                if (json_products.products[0].variants[art].option3 == null)
                                                {
                                                    cmds.Parameters.AddWithValue("@OPCION3", "");
                                                }
                                                else
                                                {
                                                    cmds.Parameters.AddWithValue("@OPCION3", json_products.products[0].variants[art].option3);
                                                }
                                                cmds.ExecuteNonQuery();
                                                cnn.CerrarConexion();
                                            }
                                            else
                                            {
                                                //se ingresa solo como variantes para las demás variantes
                                                SqlCommand cmd = new SqlCommand();
                                                cmd.Connection = cnn.AbrirConexion();
                                                cmd.CommandText = "SHOPIFY.INSERTA_ARTICULOS_VARIANTES_TIENDA";
                                                cmd.CommandTimeout = 0;
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@SKU_VARIANTE", json_products.products[0].variants[art].sku);
                                                cmd.Parameters.AddWithValue("@SKU_ARTICULO", json_products.products[0].variants[0].sku);
                                                cmd.Parameters.AddWithValue("@ID_SHOPIFY", json_products.products[0].variants[art].id);
                                                if (json_products.products[0].variants[art].option1 == null)
                                                {
                                                    cmd.Parameters.AddWithValue("@OPCION1", "");
                                                }
                                                else
                                                {
                                                    cmd.Parameters.AddWithValue("@OPCION1", json_products.products[0].variants[art].option1);
                                                }
                                                if (json_products.products[0].variants[art].option2 == null)
                                                {
                                                    cmd.Parameters.AddWithValue("@OPCION2", "");
                                                }
                                                else
                                                {
                                                    cmd.Parameters.AddWithValue("@OPCION2", json_products.products[0].variants[art].option2);
                                                }
                                                if (json_products.products[0].variants[art].option3 == null)
                                                {
                                                    cmd.Parameters.AddWithValue("@OPCION3", "");
                                                }
                                                else
                                                {
                                                    cmd.Parameters.AddWithValue("@OPCION3", json_products.products[0].variants[art].option3);
                                                }
                                                cmd.ExecuteNonQuery();
                                                cnn.CerrarConexion();
                                            }
                                            art++;
                                        }

                                        //metemos las colecciones de ese producto
                                        var Client = new RestClient("" + item.url_tienda + "");
                                        Client.Timeout = -1;
                                        Client.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);

                                        string resources = "/admin/api/2021-01/collects.json?fields=id,collection_id,product_id&product_id=" + id;
                                        var requests = new RestRequest(resources, Method.GET);
                                        requests.AddHeader("header", "Content-Type: application/json");

                                        IRestResponse responses = Client.Execute(requests);
                                        if (responses.StatusCode.ToString() == "OK")
                                        {
                                            product_colecciones json_collects = JsonConvert.DeserializeObject<product_colecciones>(responses.Content);

                                            if (json_collects.collects.Count > 0)
                                            {
                                                int i = 0;
                                                while (i < json_collects.collects.Count)
                                                {
                                                    SqlCommand cmds = new SqlCommand();
                                                    cmds.Connection = cnn.AbrirConexion();
                                                    cmds.CommandText = "SHOPIFY.INSERTAR_COLECCIONES_ARTICULOS_TIENDA";
                                                    cmds.CommandTimeout = 0;
                                                    cmds.CommandType = CommandType.StoredProcedure;
                                                    cmds.Parameters.AddWithValue("@COLECCION", json_collects.collects[i].collection_id.ToString());
                                                    cmds.Parameters.AddWithValue("@SKU_ARTICULO", json_products.products[0].variants[0].sku);
                                                    cmds.Parameters.AddWithValue("@ID_SHOPIFY", json_collects.collects[i].id.ToString());
                                                    cmds.ExecuteNonQuery();
                                                    cnn.CerrarConexion();
                                                    i++;
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        //producto sin variantes
                                        SqlCommand cmd = new SqlCommand();
                                        cmd.Connection = cnn.AbrirConexion();
                                        cmd.CommandText = "SHOPIFY.INSERTAR_ARTICULOS_TIENDA";
                                        cmd.CommandTimeout = 0;
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.AddWithValue("@ID_SHOPIFY", json_products.products[0].id.ToString());
                                        cmd.Parameters.AddWithValue("@NOMBRE", json_products.products[0].title.ToString());
                                        cmd.Parameters.AddWithValue("@NOMBRE_SHOPIFY", json_products.products[0].title.ToString());

                                        if (json_products.products[0].body_html.ToString() == null)
                                        {
                                            cmd.Parameters.AddWithValue("@DESCRIPCION", "");
                                            cmd.Parameters.AddWithValue("@DESCRIPCION_SHOPIFY", "");
                                        }
                                        else
                                        {
                                            string nombre = (Regex.Replace((json_products.products[0].body_html), @"<[^>]+>|&nbsp;", String.Empty));
                                            string descripcion = Regex.Replace(nombre.Replace("\t", " "), reduceMultiSpace, " ");
                                            cmd.Parameters.AddWithValue("@DESCRIPCION", descripcion);
                                            cmd.Parameters.AddWithValue("@DESCRIPCION_SHOPIFY", descripcion);
                                        }
                                        cmd.Parameters.AddWithValue("@SKU", json_products.products[0].variants[0].sku);
                                        cmd.Parameters.AddWithValue("@PESO", json_products.products[0].variants[0].weight);
                                        cmd.Parameters.AddWithValue("@ESTADO", json_products.products[0].status);
                                        cmd.Parameters.AddWithValue("@CANTIDAD", json_products.products[0].variants[0].inventory_quantity);
                                        cmd.Parameters.AddWithValue("@PRECIO", json_products.products[0].variants[0].price);
                                        if (json_products.products[0].variants[0].compare_at_price == null)
                                        {
                                            cmd.Parameters.AddWithValue("@DESCUENTO", "0.0");
                                        }
                                        else
                                        {
                                            cmd.Parameters.AddWithValue("@DESCUENTO", json_products.products[0].variants[0].compare_at_price);
                                        }
                                        if (json_products.products[0].variants[0].taxable == "true")
                                        {
                                            cmd.Parameters.AddWithValue("@IMPUESTO", "S");
                                        }
                                        else
                                        {
                                            cmd.Parameters.AddWithValue("@IMPUESTO", "N");
                                        }
                                        cmd.ExecuteNonQuery();
                                        cnn.CerrarConexion();

                                        //metemos las colecciones de ese producto
                                        var Client = new RestClient("" + item.url_tienda + "");
                                        Client.Timeout = -1;
                                        Client.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);

                                        string resources = "/admin/api/2021-01/collects.json?fields=id,collection_id,product_id&product_id=" + id;
                                        var requests = new RestRequest(resources, Method.GET);
                                        requests.AddHeader("header", "Content-Type: application/json");

                                        IRestResponse responses = Client.Execute(requests);
                                        if (responses.StatusCode.ToString() == "OK")
                                        {
                                            product_colecciones json_collects = JsonConvert.DeserializeObject<product_colecciones>(responses.Content);

                                            if (json_collects.collects.Count > 0)
                                            {
                                                int i = 0;
                                                while (i < json_collects.collects.Count)
                                                {
                                                    SqlCommand cmds = new SqlCommand();
                                                    cmds.Connection = cnn.AbrirConexion();
                                                    cmds.CommandText = "SHOPIFY.INSERTAR_COLECCIONES_ARTICULOS_TIENDA";
                                                    cmds.CommandTimeout = 0;
                                                    cmds.CommandType = CommandType.StoredProcedure;
                                                    cmds.Parameters.AddWithValue("@COLECCION", json_collects.collects[i].collection_id.ToString());
                                                    cmds.Parameters.AddWithValue("@SKU_ARTICULO", json_products.products[0].variants[0].sku);
                                                    cmds.Parameters.AddWithValue("@ID_SHOPIFY", json_collects.collects[i].id.ToString());
                                                    cmds.ExecuteNonQuery();
                                                    cnn.CerrarConexion();
                                                    i++;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    indicador = false;
                                }
                            }
                        }
                    }
                }
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        //Metodo para eliminar articulos de la tienda
        public string eliminar_articulos_tienda(string id)
        {
            try
            {
                string resultado = "";
                List<Credenciales> credenciales = metodo_credenciales.obtener_credenciales();
                if (credenciales == null)
                {
                    //error en credenciales
                }
                else
                {
                    foreach (Credenciales item in credenciales)
                    {
                        var restClient = new RestClient("" + item.url_tienda + "");
                        restClient.Timeout = -1;
                        restClient.Authenticator = new HttpBasicAuthenticator(item.calve_api, item.password_api);
                        string resource = "/admin/api/2021-01/products/" + id + ".json";
                        var restRequest = new RestRequest(resource, Method.DELETE);
                        restRequest.AddHeader("header", "Content-Type: application/json");

                        IRestResponse restResponse = restClient.Execute(restRequest);

                        if (restResponse.StatusCode.ToString() == "OK")
                        {
                            resultado = "ok";
                        }
                        else
                        {
                            resultado = "error";
                        }
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        //Metodo que elimina los articulos de la BD
        public string eliminar_articulos(string articulo)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.ELIMINAR_ARTICULOS_TIENDA";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SKU", articulo);
                cmd.ExecuteNonQuery();
                cnn.CerrarConexion();
                return "exito";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

    }
}
