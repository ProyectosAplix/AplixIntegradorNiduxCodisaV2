using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SincronizadorAplix_Nidux.models;
using System.Data.OracleClient;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Newtonsoft.Json;
using RestSharp.Serializers;

namespace SincronizadorAplix_Nidux
{
    class Nidux
    {
        Conexion conexion = new Conexion();
        LogsFile logsFile = new LogsFile();

        private string urlBaseApi = "http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/";
        private string urlBaseNidux = "https://api.nidux.dev/";

        private string schema = ConfigurationSettings.AppSettings["Schema"].ToString();

        public async Task<Credencial> GetCredenciales()
        {
            var url = new RestClient(urlBaseApi);
            var request = new RestRequest("api/obtener_credenciales");



            return await url.GetAsync<Credencial>(request);
        }

        public async Task<string> GetTokenNidux(string user, string password, int storedId)
        {
            string json = "";
            string mensaje = "";
            Conexion cnn = new Conexion();
            try
            {

                var list_Credenciales = new List<Valores_credencial>();
                //SqlConnection con = new SqlConnection();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "" + schema + ".APX_CREDENCIALES_CONEXION_NIDUX";
                cmd.CommandType = CommandType.StoredProcedure;
                //con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                Valores_credencial val_cre = new Valores_credencial();

                while (rdr.Read())
                {

                    val_cre.usuario = rdr["USUARIO"].ToString();
                    val_cre.contrasena = rdr["CONTRASENA"].ToString();
                    val_cre.storeId = Int32.Parse(rdr["STOREID"].ToString());
                    mensaje = rdr["MENSAJE"].ToString();

                    list_Credenciales.Add(val_cre);
                }
                cnn.CerrarConexion();

                if (mensaje.Equals("PEDIRTOKEN"))
                {
                    Login body = new Login();
                    body.username = val_cre.usuario;
                    body.password = val_cre.contrasena;
                    body.storeId = val_cre.storeId;

                    var url = new RestClient(urlBaseNidux);
                    var request = new RestRequest("login").AddJsonBody(body);

                    var response = await url.PostAsync<ResponseLogin>(request);

                    if (response.token != null)
                    {
                        mensaje = response.token;
                    }

                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "" + schema + ".ACTUALIZO_TOKEN_NIDUX";
                    cmd.Parameters.AddWithValue("@TOKEN_NIDUX", mensaje);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //con.Open();
                    cmd.ExecuteNonQuery();

                }


                return mensaje;

            }
            catch (Exception ex)
            {
                logsFile.WriteLogs("Error al consultar el token error" + ex.Message + " ( " + DateTime.Now + " )");
                var list_Credenciales = new List<Valores_credencial>();
                Valores_credencial val_cre = new Valores_credencial();
                val_cre.usuario = "ND";
                val_cre.contrasena = "ND";
                val_cre.storeId = 0;
                val_cre.mensaje = "ERROR";
                return "ERROR";
            }
            finally
            {
                cnn.CerrarConexion();
            }
        }

        //SUKASA
        #region GetArticulosOracle
        public void GetArticulosOracle()
        {
            string art = string.Empty;
            List<string> ListError = new List<string>();
            try
            {
                //obtener valores de oracle
                string query = "SELECT NO_ARTI, DESCRIPCION, PESO, CARACTERISTICAS, PRECIO, PORC_DESC, EXISTENCIA, NIDUX_COD_CATEGORIA, " +
                    "NIDUX_COD_MARCA, DESTACADO, TAGS, SEO_TAG, URL_VIDEO, RECORDDATE, ESTADO FROM naf5m.NIDUX_OBTENER_PRODUCTO_SUKASA";
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conexion.open();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                OracleDataAdapter da = new OracleDataAdapter();
                da.SelectCommand = cmd;

                DataTable dt = new DataTable();
                da.Fill(dt);

                conexion.close();

                //inserto valores de articulos en tablas intermedias SQL SERVER
                foreach (DataRow item in dt.Rows)
                {
                    try
                    {

                        SqlCommand scmd = new SqlCommand();
                        scmd.Connection = conexion.AbrirConexion();
                        //insertado en tabla articulos
                        scmd.CommandText = schema + ".INSERTAR_ARTICULOS_CODISA";
                        scmd.CommandTimeout = 0;
                        scmd.CommandType = CommandType.StoredProcedure;
                        art = item["NO_ARTI"].ToString();//Es para el catch
                        scmd.Parameters.AddWithValue("@ARTICULO", item["NO_ARTI"].ToString());
                        //limpiamos de caracteres especiales
                        string nombre = item["DESCRIPCION"].ToString().Replace("\n", "");
                        string nombre_limpio = nombre.Replace("\"", "");
                        scmd.Parameters.AddWithValue("@NOMBRE", nombre_limpio);
                        //limpiamos de caracteres especiales
                        string descripcion = item["CARACTERISTICAS"].ToString().Replace("\n", "");
                        string descripcion_limpia = descripcion.Replace("\"", "");
                        scmd.Parameters.AddWithValue("@DESCRIPCION", descripcion_limpia);
                        scmd.Parameters.AddWithValue("@PESO", Convert.ToDecimal(item["PESO"].ToString()));
                        scmd.Parameters.AddWithValue("@IMPUESTO", 0);
                        scmd.Parameters.AddWithValue("@CANTIDAD", Convert.ToDecimal(item["EXISTENCIA"].ToString()));
                        string precio = item["PRECIO"].ToString().Replace('.', ',');
                        scmd.Parameters.AddWithValue("@PRECIO", item["PRECIO"].ToString());
                        string descuento = item["PORC_DESC"].ToString().Replace('.', ',');
                        scmd.Parameters.AddWithValue("@DESCUENTO", Convert.ToDecimal(descuento));
                        scmd.Parameters.AddWithValue("@CATEGORIAS", item["NIDUX_COD_CATEGORIA"].ToString());
                        scmd.Parameters.AddWithValue("@MARCA", item["NIDUX_COD_MARCA"].ToString());

                        //validamos el estado segun la cantidad
                        //if (Convert.ToDecimal(item["EXISTENCIA"].ToString()) > 0)
                        //{
                        //    scmd.Parameters.AddWithValue("@ESTADO", 3);
                        //}
                        //else
                        //{
                        //    scmd.Parameters.AddWithValue("@ESTADO", 2);
                        //}
                        scmd.Parameters.AddWithValue("@ESTADO", item["ESTADO"].ToString());
                        scmd.Parameters.AddWithValue("@DESTACADO", item["DESTACADO"].ToString());
                        scmd.Parameters.AddWithValue("@TAGS", item["TAGS"].ToString());
                        scmd.Parameters.AddWithValue("@SEOTAG", item["SEO_TAG"].ToString());
                        scmd.Parameters.AddWithValue("@VIDEO", item["URL_VIDEO"].ToString());
                        scmd.Parameters.AddWithValue("@RECORDDATE", Convert.ToDateTime(item["RECORDDATE"]));
                        scmd.ExecuteNonQuery();
                        scmd.Parameters.Clear();
                        conexion.CerrarConexion();
                    }
                    catch (Exception ex)
                    {
                        ListError.Add($"Articulo: {art} Error: {ex.Message}");
                    }
                }
                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                logsFile.WriteLogs("\t Error a la hora de obtener e insertar los articulos provenientes de Oracle, Error: " + ex.Message);
            }
            finally
            {
                if (ListError.Count > 0)
                {
                    foreach (var error in ListError)
                    {
                        logsFile.WriteLogs("Error: " + error);
                    }
                }
            }
        }
        #endregion

        public async Task PostArticulosNidux(string token)
        {
            var url = new RestClient(urlBaseApi);
            var request = new RestRequest("api/actualizar_articulos_editados_simple");
            List<Articulos> lista = await url.GetAsync<List<Articulos>>(request);

            if (lista.Count() > 0)
            {
                int contadorInterno = 0;
                int contadorPeticiones = 0;

                while (contadorInterno < lista.Count())
                {
                    try
                    {

                        token = await GetTokenNidux("", "", 0);

                        if (contadorPeticiones == 295)
                        {
                            //espere 60 seg
                            Thread.Sleep(60000);
                            contadorPeticiones = 0;
                        }
                        //revisamos el id del articulos, si ya tiene id hay que actualizarlo sino hay que ingresarlo como nuevo
                        if (lista[contadorInterno].id == "")
                        {
                            string jsonBody = $@"{{
                                ""add"": [
                                    {{
                                        ""brand_id"": {lista[contadorInterno].id_marca},
                                        ""categorias"": [{string.Join(",", lista[contadorInterno].categorias)}],
                                        ""product_code"": ""{lista[contadorInterno].sku}"",
                                        ""product_name"": ""{lista[contadorInterno].nombre}"",
                                        ""product_description"": ""{lista[contadorInterno].descripcion}"",
                                        ""product_price"": ""{lista[contadorInterno].precio}"",
                                        ""product_shipping"": {lista[contadorInterno].costo_shipping_individual ?? "0"},
                                        ""product_weight"": {lista[contadorInterno].peso_producto ?? "0"},
                                        ""product_sale"": {lista[contadorInterno].porcentaje_oferta ?? "0"},
                                        ""product_status"": {lista[contadorInterno].estado_de_producto ?? "0"},
                                        ""product_home"": {lista[contadorInterno].es_destacado ?? "0"},
                                        ""product_stock"": {lista[contadorInterno].stock_principal},
                                        ""product_video"": ""{lista[contadorInterno].video_youtube_url}"",
                                        ""product_hidestock"": {lista[contadorInterno].ocultar_indicador_stock ?? "0"},
                                        ""product_reserve"": {lista[contadorInterno].producto_permite_reservacion ?? "0"},
                                        ""product_reserve_limit"": {lista[contadorInterno].limite_para_reservar_en_carrito ?? "0"},
                                        ""product_reserve_percentage"": {lista[contadorInterno].porcentaje_para_reservar ?? "0"},
                                        ""product_tax"": {lista[contadorInterno].impuesto_producto},
                                        ""seo_tags"": [{string.Join(",", lista[contadorInterno].seo_tags.Select(tag => $"\"{tag}\""))}],
                                        ""tags"": [{string.Join(",", lista[contadorInterno].tags.Select(tag => $"\"{tag}\""))}]
                                    }}
                                ]
                            }}";

                            //post articulo
                            var urlPostArticulo = new RestClient(urlBaseNidux);
                            var requestPostArticulo = new RestRequest("v3/products")
                                .AddHeader("Authorization", "Bearer " + token)
                                .AddStringBody(jsonBody, ContentType.Json);
                            var responseArticulo = await urlPostArticulo.PostAsync(requestPostArticulo);

                            if (responseArticulo.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                Respuesta respuesta = JsonConvert.DeserializeObject<Respuesta>(responseArticulo.Content);

                                respuesta.sku = lista[contadorInterno].sku;
                                //update ID
                                var urlUpdate = new RestClient(urlBaseApi);
                                var requestUpdate = new RestRequest("api/actualizar_id_articulos").AddJsonBody(respuesta);
                                var responseUpdate = await urlUpdate.PutAsync(requestUpdate);

                                if (responseUpdate.StatusCode != System.Net.HttpStatusCode.OK)
                                {
                                    logsFile.WriteLogs("Status Code: " + responseUpdate.StatusCode + " Error: " + responseUpdate.ErrorMessage);
                                }
                            }
                            else
                            {
                                logsFile.WriteLogs("Error en Articulo: " + lista[contadorInterno].sku + ", Status Code: " + responseArticulo.StatusCode.ToString() + ", Mensaje Error: " + responseArticulo.ErrorMessage);
                            }
                        }
                        else
                        {

                            string jsonBody = $@"{{
                                        ""id"": ""{lista[contadorInterno].id}"",
                                        ""operation_type"": ""replace"",
                                        ""brand_id"": {lista[contadorInterno].id_marca},
                                        ""categorias"": [{string.Join(",", lista[contadorInterno].categorias)}],
                                        ""product_code"": ""{lista[contadorInterno].sku}"",
                                        ""product_name"": ""{lista[contadorInterno].nombre}"",
                                        ""product_description"": ""{lista[contadorInterno].descripcion}"",
                                        ""product_price"": ""{lista[contadorInterno].precio}"",
                                        ""product_shipping"": {lista[contadorInterno].costo_shipping_individual ?? "0"},
                                        ""product_weight"": {lista[contadorInterno].peso_producto ?? "0"},
                                        ""product_sale"": {lista[contadorInterno].porcentaje_oferta ?? "0"},
                                        ""product_status"": {lista[contadorInterno].estado_de_producto ?? "0"},
                                        ""product_home"": {lista[contadorInterno].es_destacado ?? "0"},
                                        ""product_stock"": {lista[contadorInterno].stock_principal},
                                        ""product_video"": ""{lista[contadorInterno].video_youtube_url}"",
                                        ""product_hidestock"": {lista[contadorInterno].ocultar_indicador_stock ?? "0"},
                                        ""product_reserve"": {lista[contadorInterno].producto_permite_reservacion ?? "0"},
                                        ""product_reserve_limit"": {lista[contadorInterno].limite_para_reservar_en_carrito ?? "0"},
                                        ""product_reserve_percentage"": {lista[contadorInterno].porcentaje_para_reservar ?? "0"},
                                        ""product_tax"": {lista[contadorInterno].impuesto_producto},
                                        ""seo_tags"": [{string.Join(",", lista[contadorInterno].seo_tags.Select(tag => $"\"{tag}\""))}],
                                        ""tags"": [{string.Join(",", lista[contadorInterno].tags.Select(tag => $"\"{tag}\""))}]
                                                                 
                            }}";
                            //put articulo
                            var urlPutArticulo = new RestClient(urlBaseNidux);
                            var requestPutArticulo = new RestRequest("v3/products/")
                                .AddHeader("Authorization", "Bearer " + token)
                                .AddStringBody(jsonBody, ContentType.Json);
                            var responseArticulo = await urlPutArticulo.PatchAsync(requestPutArticulo);

                            if (responseArticulo.StatusCode != System.Net.HttpStatusCode.OK)
                            {
                                logsFile.WriteLogs("Error en Articulo: " + lista[contadorInterno].sku + ", Status Code: " + responseArticulo.StatusCode.ToString() + ", Mensaje Error: " + responseArticulo.ErrorMessage);
                            }
                        }

                        contadorPeticiones++;
                        contadorInterno++;
                    }
                    catch (Exception ex)
                    {
                        logsFile.WriteLogs("Error En procedimiento de sincronizacion de articulos, Articulo: " + lista[contadorInterno].sku + ", Mensaje de Error: " + ex.Message);

                        //Validacion del Error Request failed with status code NotFound
                        //sucede cuando borraron el articulo desde NIDUX por lo que esta en tablas propias y no en nidux
                        //lo que se hace es desactivar el producto para que no vuelva a dar error
                        if (ex.Message.Equals("Request failed with status code NotFound"))
                        {
                            try
                            {
                                SqlCommand v_comando = new SqlCommand();
                                v_comando.Connection = conexion.AbrirConexion();
                                v_comando.CommandText = "CEMACO.DESACTIVAR_ARTICULOS";
                                v_comando.CommandType = CommandType.StoredProcedure;
                                v_comando.Parameters.AddWithValue("@ARTICULO", lista[contadorInterno].sku);
                                v_comando.ExecuteReader();

                            }
                            catch (Exception e)
                            {
                                logsFile.WriteLogs("No se pudo desactivar el Articulo " + e.Message);
                            }
                        }
                        contadorPeticiones++;
                        contadorInterno++;
                    }
                    finally
                    {
                        conexion.CerrarConexion();
                    }
                }
            }
            else
            {
                logsFile.WriteLogs("Mensaje: no hay articulos para actualizar");
            }

        }

        public async Task UpdateDate()
        {
            var url = new RestClient(urlBaseApi);
            var request = new RestRequest("api/actualizar_fecha");
            var resp = await url.GetAsync(request);
        }


        //Comente la llamada a el Metodo para que no diera error hasta terminar lo de los pedidos 
        //comentario en linea 357 solo ir a descomentar para que vuelva a funcionar la insersion de pedidos
        private async Task GetPedidos(string token)
        {
            try
            {
                token = await GetTokenNidux("", "", 0);
                var client = new RestClient(urlBaseNidux);
                var request = new RestRequest("v3/orders/").AddHeader("Authorization", "Bearer " + token).AddStringBody("{\r\n   \"pagina\":1,\r\n   \"cantidad_ordenes\":100,\r\n   \"estado_orden\" : 0\r\n}", "application/json");
                var response = await client.PostAsync(request);

                var envio = @"""envio"":[]";
                var envioNUll = @"""envio"":null";

                var facturacion = @"""facturacion"":[]";
                var facturacionNull = @"""facturacion"":null";

                var Json = response.Content;
                Json = Json.Replace(envio, envioNUll).Replace(facturacion, facturacionNull);

                var facPedidos = JsonConvert.DeserializeObject<Pedidos>(Json);

                int contador = 0;
                int cantidad_pedidos = facPedidos.ordenes.data.Count;

                if (facPedidos.ordenes.data.Count > 0)
                {
                    while (contador < cantidad_pedidos)
                    {
                        if (facPedidos.ordenes.data[contador].estado_pago != "Pagado")
                        {
                            facPedidos.ordenes.data.RemoveAt(contador);
                            cantidad_pedidos--;
                        }
                        else
                        {

                            contador++;
                        }
                    }

                    int count_data = 0;
                    Conexion con = new Conexion();
                    while (count_data < facPedidos.ordenes.data.Count)
                    {
                        try
                        {
                            //id_error = pedido.ordenes.data[count_data].orderId;
                            int count_detalle_sp = 0;
                            //insertamos el encabezado del pedido
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = con.AbrirConexion();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "" + schema + ".AGREGAR_PEDIDOS";
                            cmd.Parameters.AddWithValue("@orderId", ((object)facPedidos.ordenes.data[count_data].orderId) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@wish_id", ((object)facPedidos.ordenes.data[count_data].wish_id) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@cliente", ((object)facPedidos.ordenes.data[count_data].cliente) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@identificacion", ((object)facPedidos.ordenes.data[count_data].identificacion) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@correo", ((object)facPedidos.ordenes.data[count_data].correo) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@telefono_fijo", ((object)facPedidos.ordenes.data[count_data].telefono_fijo) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@telefono_movil", ((object)facPedidos.ordenes.data[count_data].telefono_movil) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@es_anonimo", ((object)facPedidos.ordenes.data[count_data].es_anonimo) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@monto_impuestos", ((object)facPedidos.ordenes.data[count_data].monto_impuestos) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@fecha_orden", ((object)facPedidos.ordenes.data[count_data].fecha_orden) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@orderGiftpointsUsed", ((object)facPedidos.ordenes.data[count_data].orderGiftpointsUsed) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@estado_orden", ((object)facPedidos.ordenes.data[count_data].estado_orden) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@moneda", ((object)facPedidos.ordenes.data[count_data].moneda) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@observaciones", ((object)facPedidos.ordenes.data[count_data].observaciones) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@codigo_autorizacion", ((object)facPedidos.ordenes.data[count_data].codigo_autorizacion) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@ip_origen", ((object)facPedidos.ordenes.data[count_data].ip_origen) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@estado_pago", ((object)facPedidos.ordenes.data[count_data].estado_pago) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@medio_pago", ((object)facPedidos.ordenes.data[count_data].medio_pago) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@total", ((object)facPedidos.ordenes.data[count_data].total) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@cuponUsado", ((object)facPedidos.ordenes.data[count_data].cuponUsado) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@cuponTipo", ((object)facPedidos.ordenes.data[count_data].cuponTipo) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@sucursal", ((object)facPedidos.ordenes.data[count_data].sucursal) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@recoger_sucursal", ((object)facPedidos.ordenes.data[count_data].recoger_sucursal) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@codigo_metodo_pago", ((object)facPedidos.ordenes.data[count_data].codigo_metodo_pago) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@moneda_fe", ((object)facPedidos.ordenes.data[count_data].moneda_fe) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@tipo_envio", ((object)facPedidos.ordenes.data[count_data].tipo_envio) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@costo_total_shipping", ((object)facPedidos.ordenes.data[count_data].costo_total_shipping) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@tasa_impuesto_shipping", ((object)facPedidos.ordenes.data[count_data].tasa_impuesto_shipping) ?? DBNull.Value);
                            //datos del envio
                            if (facPedidos.ordenes.data[count_data].direcciones.envio == null)
                            {
                                cmd.Parameters.AddWithValue("@nombre_destinatario_envio", DBNull.Value);
                                cmd.Parameters.AddWithValue("@identificacion_envio", DBNull.Value);
                                cmd.Parameters.AddWithValue("@tipo_identificacion_envio", DBNull.Value);
                                cmd.Parameters.AddWithValue("@correo_envio", DBNull.Value);
                                cmd.Parameters.AddWithValue("@telefono_envio", DBNull.Value);
                                cmd.Parameters.AddWithValue("@movil_envio", DBNull.Value);
                                cmd.Parameters.AddWithValue("@pais_envio", DBNull.Value);
                                cmd.Parameters.AddWithValue("@provincia_envio", DBNull.Value);
                                cmd.Parameters.AddWithValue("@canton_envio", DBNull.Value);
                                cmd.Parameters.AddWithValue("@distrito_envio", DBNull.Value);
                                cmd.Parameters.AddWithValue("@detalle_direccion_envio", DBNull.Value);
                                cmd.Parameters.AddWithValue("@ciudad_envio", DBNull.Value);
                                cmd.Parameters.AddWithValue("@zip_envio", DBNull.Value);
                                cmd.Parameters.AddWithValue("@geo_latitud_envio", DBNull.Value);
                                cmd.Parameters.AddWithValue("@geo_longitud_envio", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@nombre_destinatario_envio", ((object)facPedidos.ordenes.data[count_data].direcciones.envio.nombre_destinatario) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@identificacion_envio", ((object)facPedidos.ordenes.data[count_data].direcciones.envio.identificacion) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@tipo_identificacion_envio", ((object)facPedidos.ordenes.data[count_data].direcciones.envio.tipo_identificacion) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@correo_envio", ((object)facPedidos.ordenes.data[count_data].direcciones.envio.correo) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@telefono_envio", ((object)facPedidos.ordenes.data[count_data].direcciones.envio.telefono) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@movil_envio", ((object)facPedidos.ordenes.data[count_data].direcciones.envio.movil) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@pais_envio", ((object)facPedidos.ordenes.data[count_data].direcciones.envio.pais) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@provincia_envio", ((object)facPedidos.ordenes.data[count_data].direcciones.envio.provincia) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@canton_envio", DBNull.Value);//nidux no vanda estos valores
                                cmd.Parameters.AddWithValue("@distrito_envio", DBNull.Value);//nidux no vanda estos valores
                                cmd.Parameters.AddWithValue("@detalle_direccion_envio", ((object)facPedidos.ordenes.data[count_data].direcciones.envio.detalle_direccion) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ciudad_envio", ((object)facPedidos.ordenes.data[count_data].direcciones.envio.ciudad) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@zip_envio", ((object)facPedidos.ordenes.data[count_data].direcciones.envio.zip) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@geo_latitud_envio", ((object)facPedidos.ordenes.data[count_data].direcciones.envio.geo_latitud) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@geo_longitud_envio", ((object)facPedidos.ordenes.data[count_data].direcciones.envio.geo_longitud) ?? DBNull.Value);
                            }
                            //datos facturacion
                            if (facPedidos.ordenes.data[count_data].direcciones.facturacion == null)
                            {
                                cmd.Parameters.AddWithValue("@nombre_destinatario_fac", DBNull.Value);
                                cmd.Parameters.AddWithValue("@identificacion_fac", DBNull.Value);
                                cmd.Parameters.AddWithValue("@tipo_identificacion_fac", DBNull.Value);
                                cmd.Parameters.AddWithValue("@correo_fac", DBNull.Value);
                                cmd.Parameters.AddWithValue("@telefono_fac", DBNull.Value);
                                cmd.Parameters.AddWithValue("@movil_fac", DBNull.Value);
                                cmd.Parameters.AddWithValue("@pais_fac", DBNull.Value);
                                cmd.Parameters.AddWithValue("@provincia_fac", DBNull.Value);
                                cmd.Parameters.AddWithValue("@canton_fac", DBNull.Value);
                                cmd.Parameters.AddWithValue("@distrito_fac", DBNull.Value);
                                cmd.Parameters.AddWithValue("@detalle_direccion_fac", DBNull.Value);
                                cmd.Parameters.AddWithValue("@ciudad_fac", DBNull.Value);
                                cmd.Parameters.AddWithValue("@zip_fac", DBNull.Value);
                                cmd.Parameters.AddWithValue("@geo_latitud_fac", DBNull.Value);
                                cmd.Parameters.AddWithValue("@geo_longitud_fac", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@nombre_destinatario_fac", ((object)facPedidos.ordenes.data[count_data].direcciones.facturacion.nombre_destinatario) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@identificacion_fac", ((object)facPedidos.ordenes.data[count_data].direcciones.facturacion.identificacion) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@tipo_identificacion_fac", ((object)facPedidos.ordenes.data[count_data].direcciones.facturacion.tipo_identificacion) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@correo_fac", ((object)facPedidos.ordenes.data[count_data].direcciones.facturacion.correo) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@telefono_fac", ((object)facPedidos.ordenes.data[count_data].direcciones.facturacion.telefono) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@movil_fac", ((object)facPedidos.ordenes.data[count_data].direcciones.facturacion.movil) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@pais_fac", ((object)facPedidos.ordenes.data[count_data].direcciones.facturacion.pais) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@provincia_fac", ((object)facPedidos.ordenes.data[count_data].direcciones.facturacion.provincia_fe) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@canton_fac", DBNull.Value);//nidux no vanda estos valores
                                cmd.Parameters.AddWithValue("@distrito_fac", DBNull.Value);//nidux no vanda estos valores
                                cmd.Parameters.AddWithValue("@detalle_direccion_fac", ((object)facPedidos.ordenes.data[count_data].direcciones.facturacion.detalle_direccion) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ciudad_fac", ((object)facPedidos.ordenes.data[count_data].direcciones.facturacion.ciudad) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@zip_fac", ((object)facPedidos.ordenes.data[count_data].direcciones.facturacion.zip) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@geo_latitud_fac", ((object)facPedidos.ordenes.data[count_data].direcciones.facturacion.geo_latitud) ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@geo_longitud_fac", ((object)facPedidos.ordenes.data[count_data].direcciones.facturacion.geo_longitud) ?? DBNull.Value);
                            }
                            //con.Open();
                            cmd.ExecuteNonQuery();
                            con.CerrarConexion();

                            while (count_detalle_sp < facPedidos.ordenes.data[count_data].detalles.Count)
                            {
                                /*llamo al sp de lineas pedido*/
                                SqlCommand cmd2 = new SqlCommand();
                                cmd2.Connection = con.AbrirConexion();
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.CommandText = "" + schema + ".AGREGAR_PEDIDOS_LINEA";
                                cmd2.Parameters.AddWithValue("@orderId", ((object)facPedidos.ordenes.data[count_data].orderId) ?? DBNull.Value);
                                cmd2.Parameters.AddWithValue("@id_producto", ((object)facPedidos.ordenes.data[count_data].detalles[count_detalle_sp].id_producto) ?? DBNull.Value);
                                cmd2.Parameters.AddWithValue("@id_variacion", ((object)facPedidos.ordenes.data[count_data].detalles[count_detalle_sp].id_variacion) ?? DBNull.Value);
                                cmd2.Parameters.AddWithValue("@sku", ((object)facPedidos.ordenes.data[count_data].detalles[count_detalle_sp].sku) ?? DBNull.Value);
                                cmd2.Parameters.AddWithValue("@nombre_producto", ((object)facPedidos.ordenes.data[count_data].detalles[count_detalle_sp].nombre_producto) ?? DBNull.Value);
                                cmd2.Parameters.AddWithValue("@precio", ((object)facPedidos.ordenes.data[count_data].detalles[count_detalle_sp].precio) ?? DBNull.Value);
                                cmd2.Parameters.AddWithValue("@cantidad", ((object)facPedidos.ordenes.data[count_data].detalles[count_detalle_sp].cantidad) ?? DBNull.Value);
                                cmd2.Parameters.AddWithValue("@porcentaje_descuento", ((object)facPedidos.ordenes.data[count_data].detalles[count_detalle_sp].porcentaje_descuento) ?? DBNull.Value);
                                cmd2.Parameters.AddWithValue("@subtotal_descuento", ((object)facPedidos.ordenes.data[count_data].detalles[count_detalle_sp].subtotal_descuento) ?? DBNull.Value);
                                cmd2.Parameters.AddWithValue("@subtotal_linea", ((object)facPedidos.ordenes.data[count_data].detalles[count_detalle_sp].subtotal_linea) ?? DBNull.Value);
                                cmd2.Parameters.AddWithValue("@impuesto", ((object)facPedidos.ordenes.data[count_data].detalles[count_detalle_sp].impuesto) ?? DBNull.Value);
                                cmd2.Parameters.AddWithValue("@subtotal_impuestos", ((object)facPedidos.ordenes.data[count_data].detalles[count_detalle_sp].subtotal_impuestos) ?? DBNull.Value);
                                //con.Open();
                                cmd2.ExecuteNonQuery();
                                con.CerrarConexion();
                                count_detalle_sp++;
                            }
                            //id_last = facPedidos.ordenes.data[count_data].orderId;
                        }
                        catch (Exception e)
                        {
                            con.CerrarConexion();
                            //lista_error.Add("Fallo en el pedido numero: " + id_error.ToString() + ", Fecha error: " + day.ToString("MM/dd/yy HH:mm:ss"));
                        }
                        finally
                        {
                            con.CerrarConexion();
                        }
                        count_data++;
                    }

                    //inserto el wihs id
                    #region inserto wish id y pedidos por medio del api

                    //foreach (var item in facPedidos.ordenes.data)
                    //{
                    //    if (item.orderId != null && item.wish_id != null)
                    //    {
                    //        SqlCommand scmd = new SqlCommand();
                    //        scmd.Connection = conexion.AbrirConexion();
                    //        scmd.CommandText = schema + ".INSERTAR_ID_WiSH";
                    //        scmd.CommandTimeout = 0;
                    //        scmd.CommandType = CommandType.StoredProcedure;
                    //        scmd.Parameters.AddWithValue("@WISH_ID", item.wish_id);
                    //        scmd.Parameters.AddWithValue("@ORDERID", item.orderId);
                    //        scmd.ExecuteNonQuery();
                    //        scmd.Parameters.Clear();

                    //        conexion.CerrarConexion();
                    //    }
                    //}


                    //string nuevoFactPedidos = JsonConvert.SerializeObject(facPedidos, Formatting.Indented);

                    //var clientPedido = new RestClient(urlBaseApi);
                    //var requestPedido = new RestRequest("api/insertar_pedidos").AddStringBody(nuevoFactPedidos, "application/json");
                    //var responsePedido = await clientPedido.PostAsync(requestPedido);

                    //if (responsePedido.StatusCode == System.Net.HttpStatusCode.OK)
                    //{
                    //    Respuesta responseApi = JsonConvert.DeserializeObject<Respuesta>(responsePedido.Content);
                    //    if (responseApi.error.Count > 0)
                    //    {
                    //        int n = 0;
                    //        while (n < responseApi.error.Count)
                    //        {
                    //            logsFile.WriteLogs("\t Error al insertar los pedidos de Nidux en tablas intermedias:, Error: " + responseApi.error[n].ToString());
                    //            n++;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    logsFile.WriteLogs("\t Error al insertar los pedidos de Nidux en tablas intermedias:, Error: " + responsePedido.Content);
                    //}
                    #endregion

                }
            }
            catch (Exception ex)
            {
                logsFile.WriteLogs("\t Error a la hora de obtener e insertar los articulos provenientes de Nidux, Error: " + ex.Message);

            }
        }

        private async Task UpdatePedidos(string token)
        {
            try
            {
                var client = new RestClient(urlBaseApi);
                var request = new RestRequest("api/actualizar_estado_pedido");
                var response = await client.GetAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<Estados> listaPedidos = JsonConvert.DeserializeObject<List<Estados>>(response.Content);

                    if (listaPedidos.Count > 0)
                    {
                        int n = 0;
                        token = await GetTokenNidux("", "", 0);
                        while (n < listaPedidos.Count)
                        {
                            var clientEstado = new RestClient(urlBaseNidux);
                            var requestEstado = new RestRequest("v3/orders/" + listaPedidos[n].orderId.ToString() + "/orderStatus")
                                .AddHeader("Authorization", "Bearer " + token).AddStringBody("{\r\n        \"nuevo_estado\": " + listaPedidos[n].nuevo_estado.ToString() + "\r\n}", "application/json");
                            var responseEstado = await clientEstado.PutAsync(requestEstado);

                            if (responseEstado.StatusCode != System.Net.HttpStatusCode.OK)
                            {
                                logsFile.WriteLogs("\t Error a la hora de actualizar los estado del Pedido:" + listaPedidos[n].orderId.ToString() + ", Error: " + responseEstado.Content);
                            }
                            n++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logsFile.WriteLogs("\t Error a la hora de actualizar los estado de los Pedidos en Nidux, Error: " + ex.Message);
            }
        }

        private DataTable GetData(string sql)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conexion.AbrirConexion();
                cmd.CommandText = sql;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter dr = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dr.Fill(dt);
                conexion.CerrarConexion();
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SincronizarPedidosOracle(string token)
        {
            //insertamos los pedidos en el sincronizador
            await GetPedidos(token);

            string queryPedidos = "SELECT ORDERID, CLIENTE, IDENTIFICACION, TELEFONO_MOVIL, CORREO, FECHA_ORDEN, CODIGO_AUTORIZACION, WISH_ID, CASE WHEN RECOGER_SUCURSAL IS NULL THEN CONCAT(DETALLE_DIRECCION_ENVIO, ',', PROVINCIA_ENVIO) ELSE RECOGER_SUCURSAL END AS DIRECCION  FROM " + schema + ".PEDIDOS WHERE CONSECUTIVO IS NULL";

            //obtenemos los pedidos para sincronizar
            DataTable dtPedidos = GetData(queryPedidos);

            if (dtPedidos.Rows.Count > 0)
            {
                //Empezamos con las inserciones en oracle
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conexion.open();
                string ID_WISH = "";
                try
                {
                    foreach (DataRow dtrEncabezado in dtPedidos.Rows)
                    {
                        ID_WISH = dtrEncabezado["WISH_ID"].ToString();
                        string JO = "";
                        //insertamos el encabezado
                        #region "Inserta el Encabezado Oracle"
                        string query_enca = "NAF5M.nidux_pvecom.REG_PVENC_ECOM";
                        cmd.CommandText = query_enca;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("pNO_TRANSA_MOV", OracleType.VarChar, 12).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("pNOMBRE_CLIENTE", OracleType.VarChar, 60).Value = dtrEncabezado["CLIENTE"].ToString();
                        cmd.Parameters.Add("pTELEFONO", OracleType.VarChar, 20).Value = dtrEncabezado["TELEFONO_MOVIL"].ToString();
                        cmd.Parameters.Add("pEMAIL", OracleType.VarChar, 60).Value = dtrEncabezado["CORREO"].ToString();
                        cmd.Parameters.Add("pCEDULA", OracleType.VarChar, 20).Value = dtrEncabezado["IDENTIFICACION"].ToString();

                        DateTime fecha = Convert.ToDateTime(dtrEncabezado["FECHA_ORDEN"]);
                        string fecha_slash = fecha.ToString("yyyy/MM/dd");
                        string fecha_final = fecha_slash.Replace("/", "");

                        cmd.Parameters.Add("pFECHA_REG", OracleType.VarChar, 10).Value = fecha_final;//yyyymmdd
                        cmd.Parameters.Add("pREFERENCIA", OracleType.VarChar, 15).Value = dtrEncabezado["ORDERID"].ToString();//numero de pedido en nidux
                        cmd.Parameters.Add("pLISTAREG", OracleType.VarChar, 15).Value = dtrEncabezado["WISH_ID"].ToString();//se pasa cero si no hay lista asociada

                        ID_WISH = dtrEncabezado["WISH_ID"].ToString();

                        cmd.Parameters.Add("pcodigo_web", OracleType.VarChar, 15).Value = 1;  //SI ES CEMACO 1 Y SI ES BEBEMUNDO ENVIO 2
                        cmd.Parameters.Add("pMENSAJE", OracleType.VarChar, 100).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        string no_transa = cmd.Parameters["pNO_TRANSA_MOV"].Value.ToString();
                        string respuesta = cmd.Parameters["pMENSAJE"].Value.ToString();

                        cmd.Parameters.Clear();
                        #endregion
                        if (respuesta == "")
                        {
                            DataTable dtPedido_linea = GetData("SELECT SKU, CANTIDAD, PRECIO FROM " + schema + ".PEDIDOS_LINEA WHERE ORDERID = " + dtrEncabezado["ORDERID"].ToString());

                            //insertamos las lineas
                            #region "Insertamos lineas en oracle"
                            foreach (DataRow dtrLinea in dtPedido_linea.Rows)
                            {
                                string query_linea = "NAF5M.nidux_pvecom.REG_PVDET_ECOM";
                                cmd.CommandText = query_linea;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("pNO_TRANSA_MOV", OracleType.VarChar, 12).Value = no_transa;
                                cmd.Parameters.Add("pNO_ARTI", OracleType.VarChar, 60).Value = dtrLinea["SKU"].ToString();
                                cmd.Parameters.Add("pCANTIDAD", OracleType.Number, 10).Value = Convert.ToInt32(dtrLinea["CANTIDAD"].ToString());

                                string precioPunto = dtrLinea["PRECIO"].ToString().Replace(',', '.');
                                string precioDecimal = precioPunto.Replace('.', ',');

                                cmd.Parameters.Add("pPRECIO", OracleType.Number, 20).Value = Convert.ToDecimal(precioDecimal);//
                                cmd.Parameters.Add("pMENSAJE", OracleType.VarChar, 100).Direction = ParameterDirection.Output;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            #endregion
                            //insertamos en la nota final
                            #region "nota final"
                            string query = "NAF5M.nidux_pvecom.FIN_PEDIDO";
                            cmd.CommandText = query;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("pNO_TRANSA_MOV", OracleType.VarChar, 12).Value = no_transa;
                            cmd.Parameters.Add("pESTADO", OracleType.VarChar, 10).Value = "A";// para facturar
                            cmd.Parameters.Add("PAUTORIZACION", OracleType.VarChar, 10).Value = dtrEncabezado["CODIGO_AUTORIZACION"].ToString();//codigo de autorizacion nidux "codigo_autorizacion"
                            cmd.Parameters.Add("PDIRECCION", OracleType.VarChar, 255).Value = dtrEncabezado["DIRECCION"].ToString();
                            cmd.Parameters.Add("pMENSAJE", OracleType.VarChar, 100).Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            string respuesta2 = cmd.Parameters["pMENSAJE"].Value.ToString();
                            cmd.Parameters.Clear();

                            if (respuesta2 == "")
                            {
                                //si todo se completa tenemos que actualizar el estado en las tablas para que despues actualizarlo en Nidux
                                //actualizar el estado y el consecutivo
                                string query_pedido = "UPDATE " + schema + ".PEDIDOS SET ESTADO_ORDEN = 'En Proceso', CONSECUTIVO = '" + no_transa + "' where ORDERID = " + dtrEncabezado["ORDERID"].ToString();
                                SqlCommand cmd_sql = new SqlCommand();
                                cmd_sql.Connection = conexion.AbrirConexion();
                                cmd_sql.CommandText = query_pedido;
                                cmd_sql.CommandTimeout = 0;
                                cmd_sql.CommandType = CommandType.Text;
                                cmd_sql.ExecuteNonQuery();
                                conexion.CerrarConexion();
                            }
                            else
                            {
                                logsFile.WriteLogs("Ocurrio un error a la hora de insertar el pedido en detalle final: " + dtrEncabezado["ORDERID"].ToString() + ", Mensaje: " + respuesta2);
                            }
                            #endregion

                        }
                        else
                        {
                            //error en el pedido encabezado
                            logsFile.WriteLogs("Ocurrio un error a la hora de insertar el pedido: " + dtrEncabezado["ORDERID"].ToString() + ", Mensaje: " + respuesta);
                        }


                        #region "Listas de regalo"
                        //if (!string.IsNullOrEmpty(ID_WISH))
                        //{

                        //    logsFile.WriteLogs("INICIO LISTA REGALOS");
                        //    //post lISTA DE REGALOS
                        //    var urlGetListaRegalo = new RestClient(urlBaseNidux);
                        //    var requestGetListaRegalo = new RestRequest("v2/giftlists/" + dtrEncabezado["WISH_ID"].ToString()).AddHeader("Authorization", "Bearer " + token);
                        //    var responseListaRegalo = await urlGetListaRegalo.GetAsync(requestGetListaRegalo); //aqui se me esta pegando

                        //    ListaRegalos respuestaListaRegalos = new ListaRegalos();

                        //    if (responseListaRegalo.StatusCode == System.Net.HttpStatusCode.OK)
                        //    {


                        //        respuestaListaRegalos = JsonConvert.DeserializeObject<ListaRegalos>(responseListaRegalo.Content);

                        //        int contProductos = 0;
                        //        int contadorLineaListaRegalo = 1;
                        //        foreach (var item in respuestaListaRegalos.Giftlist.Products.Available)
                        //        {
                        //            //insersion a oracle de la lista 
                        //            string query = "NAF5M.NIDUX_LISTA_REGALOS.REG_LISTA_REGALOS";
                        //            cmd.CommandText = query;
                        //            cmd.CommandType = CommandType.StoredProcedure;
                        //            cmd.Parameters.Add("pno_lista", OracleType.VarChar, 8).Value = Convert.ToString(respuestaListaRegalos.Giftlist.wish_id);
                        //            cmd.Parameters.Add("pno_linea", OracleType.Number, 10).Value = contadorLineaListaRegalo;//Convert.ToInt32(respuestaListaRegalos.Giftlist.ListName);
                        //            cmd.Parameters.Add("pno_arti", OracleType.VarChar, 15).Value = respuestaListaRegalos.Giftlist.Products.Available[contProductos].product_code;
                        //            cmd.Parameters.Add("pcantidad", OracleType.Number, 10).Value = respuestaListaRegalos.Giftlist.Products.Available[contProductos].Stock;
                        //            cmd.Parameters.Add("pmensaje", OracleType.VarChar, 255).Direction = ParameterDirection.Output;
                        //            cmd.ExecuteNonQuery();
                        //            string respuesta2 = cmd.Parameters["pmensaje"].Value.ToString();
                        //            cmd.Parameters.Clear();

                        //            logsFile.WriteLogs($"LISTAS REGALO---- WISHID {respuestaListaRegalos.Giftlist.wish_id} ListName: {respuestaListaRegalos.Giftlist.ListName} ProductName: {respuestaListaRegalos.Giftlist.Products.Available[contProductos].ProductName} Stock: {respuestaListaRegalos.Giftlist.Products.Available[contProductos].Stock}");

                        //            contProductos++;
                        //            contadorLineaListaRegalo++;
                        //        }

                        //        #region "Consulto direccion cliente"

                        //        #region "Consulto cliente"

                        //        //Consulto direccion cliente 
                        //        var urlGetCliente = new RestClient(urlBaseNidux);
                        //        var requestGetCliente = new RestRequest("v2/customers/" + respuestaListaRegalos.Giftlist.OwnerId.ToString()).AddHeader("Authorization", "Bearer " + token);
                        //        var responseGetCliente = await urlGetCliente.GetAsync(requestGetCliente);

                        //        if (responseGetCliente.StatusCode == System.Net.HttpStatusCode.OK)
                        //        {
                        //            var direcciones = @"""direcciones"":[]";
                        //            var direccionesNull = @"""direcciones"":null";

                        //            var Json = responseGetCliente.Content;
                        //            Json = Json.Replace(direcciones, direccionesNull);

                        //            Cliente respuestaListaClientes = JsonConvert.DeserializeObject<Cliente>(Json);
                        //            string telefono = string.Empty;
                        //            DateTime dt = new DateTime();
                        //            DateTime dt2 = new DateTime();
                        //            dt = Convert.ToDateTime(respuestaListaClientes.Customer.creado);
                        //            dt2 = Convert.ToDateTime(respuestaListaRegalos.Giftlist.eventDate);

                        //            #region validamos si telefono es vacio 
                        //            if (string.IsNullOrEmpty(respuestaListaClientes.Customer.Telefono1))
                        //            {
                        //                telefono = respuestaListaClientes.Customer.Telefono2;
                        //            }
                        //            else if (string.IsNullOrEmpty(respuestaListaClientes.Customer.Telefono1) && string.IsNullOrEmpty(respuestaListaClientes.Customer.Telefono2))
                        //            {
                        //                telefono = "0";
                        //            }
                        //            else
                        //            {
                        //                telefono = respuestaListaClientes.Customer.Telefono1;
                        //            }
                        //            #endregion

                        //            //INSERTAR CLIENTE 
                        //            string query3 = "NAF5M.NIDUX_LISTA_REGALOS.REG_CLIENTE";
                        //            cmd.CommandText = query3;
                        //            cmd.CommandType = CommandType.StoredProcedure;
                        //            cmd.Parameters.Add("pid_identificacion", OracleType.VarChar, 20).Value = Convert.ToString(respuestaListaClientes.Customer.Identificacion);
                        //            cmd.Parameters.Add("pnombre", OracleType.VarChar, 60).Value = respuestaListaClientes.Customer.Nombre;
                        //            if (respuestaListaClientes.Customer.Direcciones.Count == 0)
                        //            {
                        //                cmd.Parameters.Add("pdireccion", OracleType.VarChar, 240).Value = "ND";
                        //            }
                        //            else
                        //            {
                        //                cmd.Parameters.Add("pdireccion", OracleType.VarChar, 240).Value = respuestaListaClientes.Customer.Direcciones[0].Detalle;
                        //            }
                        //            cmd.Parameters.Add("ptelefono", OracleType.VarChar, 20).Value = telefono;
                        //            cmd.Parameters.Add("pemail", OracleType.VarChar, 120).Value = respuestaListaClientes.Customer.Correo;

                        //            if (string.IsNullOrEmpty(respuestaListaClientes.Customer.Genero))
                        //            {
                        //                cmd.Parameters.Add("pgenero", OracleType.VarChar, 1).Value = "N";
                        //            }
                        //            else {
                        //                cmd.Parameters.Add("pgenero", OracleType.VarChar, 1).Value = respuestaListaClientes.Customer.Genero;
                        //            }

                        //            cmd.Parameters.Add("pfnacimiento", OracleType.DateTime, 30).Value = Convert.ToDateTime(respuestaListaClientes.Customer.fecha_de_nacimiento);
                        //            cmd.Parameters.Add("pmensaje", OracleType.VarChar, 255).Direction = ParameterDirection.Output;
                        //            cmd.ExecuteNonQuery();
                        //            string respuesta3 = cmd.Parameters["pmensaje"].Value.ToString();
                        //            cmd.Parameters.Clear();

                        //            logsFile.WriteLogs($"INSERTO EN REG CLIENTE-- Nombre: {respuestaListaClientes.Customer.Nombre} Detalle: {respuestaListaClientes.Customer.Direcciones} Telefono {telefono} Correo: {respuestaListaClientes.Customer.Correo} Genero: {respuestaListaClientes.Customer.Genero} FechaDeNacimiento: {respuestaListaClientes.Customer.fecha_de_nacimiento}");

                        //            //INSERTAR EVENTO
                        //            string query4 = "NAF5M.NIDUX_LISTA_REGALOS.REG_EVENTO";
                        //            cmd.CommandText = query4;
                        //            cmd.CommandType = CommandType.StoredProcedure;
                        //            cmd.Parameters.Add("pno_lista", OracleType.VarChar, 8).Value = Convert.ToString(respuestaListaRegalos.Giftlist.wish_id);
                        //            cmd.Parameters.Add("pcod_cliente1", OracleType.VarChar, 15).Value = Convert.ToString(respuestaListaClientes.Customer.id);
                        //            cmd.Parameters.Add("pcod_cliente2", OracleType.VarChar, 15).Value = Convert.ToString(respuestaListaRegalos.Giftlist.extraOwnerId);
                        //            cmd.Parameters.Add("pfecha_crea", OracleType.DateTime, 30).Value = respuestaListaClientes.Customer.creado;
                        //            cmd.Parameters.Add("ptipo_lista", OracleType.VarChar, 2).Value = Convert.ToString(respuestaListaRegalos.Giftlist.modeList);
                        //            cmd.Parameters.Add("pfecha_evento", OracleType.DateTime, 30).Value = respuestaListaRegalos.Giftlist.eventDate;
                        //            cmd.Parameters.Add("ptipo_envoltura", OracleType.VarChar, 1).Value = Convert.ToString(respuestaListaRegalos.Giftlist.EcoFriendly.id);
                        //            cmd.Parameters.Add("pmensaje", OracleType.VarChar, 100).Direction = ParameterDirection.Output;
                        //            cmd.ExecuteNonQuery();
                        //            string respuesta4 = cmd.Parameters["pmensaje"].Value.ToString();
                        //            cmd.Parameters.Clear();

                        //            logsFile.WriteLogs($"INSERTO EN REG_EVENTO-- WishId: {respuestaListaRegalos.Giftlist.wish_id} ID: {respuestaListaClientes.Customer.id} ExtraOwnerId: {respuestaListaRegalos.Giftlist.extraOwnerId} Creado: {respuestaListaClientes.Customer.creado} ListType: {respuestaListaRegalos.Giftlist.ListType} EventDate: {respuestaListaRegalos.Giftlist.eventDate}");

                        //        }
                        //        else
                        //        {
                        //            logsFile.WriteLogs("Error al consultar Cliente:" + respuestaListaRegalos.Giftlist.OwnerId + ", Status Code: " + responseGetCliente.StatusCode.ToString() + ", Mensaje Error: " + responseGetCliente.ErrorMessage);
                        //        }
                        //        #endregion

                        //        #endregion
                        //        logsFile.WriteLogs("FIN LISTA REGALOS CON EXITO");
                        //    }
                        //    else
                        //    {
                        //        logsFile.WriteLogs("Error al consultar lista de regalo: " + respuestaListaRegalos.Giftlist.wish_id.ToString() + ", Status Code: " + responseListaRegalo.StatusCode.ToString() + ", Mensaje Error: " + responseListaRegalo.ErrorMessage);
                        //    }
                        //}
                        #endregion

                    }

                    //actualizamos el estado en Nidux de los pedidos
                    //await UpdatePedidos(token); //Comentado porque no esta implementado 

                }
                catch (Exception ex)
                {
                    logsFile.WriteLogs("Error: " + ex.Message);
                }
            }

        }

        //Consulta la fecha actual del servidor 
        public string ConsultoFecha2()
        {
            string Esquema = ConfigurationManager.AppSettings["Schema"].ToString();
            string fecha = string.Empty;
            Conexion cnn = new Conexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn.AbrirConexion();
            cmd.CommandText = "" + Esquema + ".APX_FECHA_ACTUAL";
            cmd.CommandType = CommandType.StoredProcedure;
            //con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                fecha = rdr["FECHA"].ToString();
            }
            cnn.CerrarConexion();
            return fecha;
        }

        public void actualizar_fechaV2(string fecha)
        {
            string Esquema = ConfigurationManager.AppSettings["Schema"].ToString();

            Conexion con = new Conexion();
            int record = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.AbrirConexion();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "" + Esquema + ".ACT_FECHA_CONSULTA";
                cmd.Parameters.AddWithValue("@FECHA", Convert.ToDateTime(fecha));
                //con.AbrirConexion();
                record = cmd.ExecuteNonQuery();
                con.CerrarConexion();
            }
            catch (SqlException e)
            {
                con.CerrarConexion();
            }
            finally
            {
                con.CerrarConexion();
            }

        }

    }
}

