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
                logsFile.WriteLogs("Error al consultar el token error"+ ex.Message +" ( " + DateTime.Now + " )");
                var list_Credenciales = new List<Valores_credencial>();
                Valores_credencial val_cre = new Valores_credencial();
                val_cre.usuario = "ND";
                val_cre.contrasena = "ND";
                val_cre.storeId = 0;
                val_cre.mensaje = "ERROR";
                return "ERROR";
            }
            finally {
                cnn.CerrarConexion();
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

            string queryPedidos = "SELECT ORDERID, CLIENTE, IDENTIFICACION, TELEFONO_MOVIL, CORREO, FECHA_ORDEN, CODIGO_AUTORIZACION, WISH_ID, CASE WHEN RECOGER_SUCURSAL IS NULL THEN CONCAT(DETALLE_DIRECCION_ENVIO, ',', PROVINCIA_ENVIO) ELSE RECOGER_SUCURSAL END AS DIRECCION  FROM " + schema + ".PEDIDOS WHERE WISH_ID IS NOT NULL AND WISH_ID <> 0 AND WISK_ID_SINCRONIZADO = 'N' ";

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

                        #region "Listas de regalo"
                        if (!string.IsNullOrEmpty(ID_WISH))
                        {

                            logsFile.WriteLogs("INICIO LISTA REGALOS");
                            //post lISTA DE REGALOS
                            var urlGetListaRegalo = new RestClient(urlBaseNidux);
                            var requestGetListaRegalo = new RestRequest("v3/giftlists/" + dtrEncabezado["WISH_ID"].ToString()).AddHeader("Authorization", "Bearer " + token);
                            var responseListaRegalo = await urlGetListaRegalo.GetAsync(requestGetListaRegalo); //aqui se me esta pegando

                            ListaRegalos respuestaListaRegalos = new ListaRegalos();

                            if (responseListaRegalo.StatusCode == System.Net.HttpStatusCode.OK)
                            {


                                respuestaListaRegalos = JsonConvert.DeserializeObject<ListaRegalos>(responseListaRegalo.Content);

                                int contProductos = 0;
                                int contadorLineaListaRegalo = 1;
                                foreach (var item in respuestaListaRegalos.Giftlist.Products.Available)
                                {
                                    //insersion a oracle de la lista 
                                    string query = "NAF5M.NIDUX_LISTA_REGALOS.REG_LISTA_REGALOS";
                                    cmd.CommandText = query;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("pno_lista", OracleType.VarChar, 8).Value = Convert.ToString(respuestaListaRegalos.Giftlist.wish_id);
                                    cmd.Parameters.Add("pno_linea", OracleType.Number, 10).Value = contadorLineaListaRegalo;//Convert.ToInt32(respuestaListaRegalos.Giftlist.ListName);
                                    cmd.Parameters.Add("pno_arti", OracleType.VarChar, 15).Value = respuestaListaRegalos.Giftlist.Products.Available[contProductos].product_code;
                                    cmd.Parameters.Add("pcantidad", OracleType.Number, 10).Value = respuestaListaRegalos.Giftlist.Products.Available[contProductos].Stock;
                                    cmd.Parameters.Add("pmensaje", OracleType.VarChar, 255).Direction = ParameterDirection.Output;
                                    cmd.ExecuteNonQuery();
                                    string respuesta2 = cmd.Parameters["pmensaje"].Value.ToString();
                                    cmd.Parameters.Clear();

                                    logsFile.WriteLogs($"LISTA REGALO INSERTADA---- WISHID {respuestaListaRegalos.Giftlist.wish_id} ListName: {respuestaListaRegalos.Giftlist.ListName} ProductName: {respuestaListaRegalos.Giftlist.Products.Available[contProductos].ProductName} Stock: {respuestaListaRegalos.Giftlist.Products.Available[contProductos].Stock}");

                                    contProductos++;
                                    contadorLineaListaRegalo++;
                                }

                                #region "Consulto direccion cliente"

                                #region "Consulto cliente"

                                //Consulto direccion cliente 
                                var urlGetCliente = new RestClient(urlBaseNidux);
                                var requestGetCliente = new RestRequest("v3/customers/" + respuestaListaRegalos.Giftlist.OwnerId.ToString()).AddHeader("Authorization", "Bearer " + token);
                                var responseGetCliente = await urlGetCliente.GetAsync(requestGetCliente);

                                if (responseGetCliente.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var direcciones = @"""direcciones"":[]";
                                    var direccionesNull = @"""direcciones"":null";

                                    var Json = responseGetCliente.Content;
                                    Json = Json.Replace(direcciones, direccionesNull);

                                    Cliente respuestaListaClientes = JsonConvert.DeserializeObject<Cliente>(Json);
                                    string telefono = string.Empty;
                                    DateTime dt = new DateTime();
                                    DateTime dt2 = new DateTime();
                                    dt = Convert.ToDateTime(respuestaListaClientes.Customer.creado);
                                    dt2 = Convert.ToDateTime(respuestaListaRegalos.Giftlist.eventDate);

                                    #region validamos si telefono es vacio 
                                    if (string.IsNullOrEmpty(respuestaListaClientes.Customer.Telefono1))
                                    {
                                        telefono = respuestaListaClientes.Customer.Telefono2;
                                    }
                                    else if (string.IsNullOrEmpty(respuestaListaClientes.Customer.Telefono1) && string.IsNullOrEmpty(respuestaListaClientes.Customer.Telefono2))
                                    {
                                        telefono = "0";
                                    }
                                    else
                                    {
                                        telefono = respuestaListaClientes.Customer.Telefono1;
                                    }
                                    #endregion

                                    //INSERTAR CLIENTE 
                                    string query3 = "NAF5M.NIDUX_LISTA_REGALOS.REG_CLIENTE";
                                    cmd.CommandText = query3;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("pid_identificacion", OracleType.VarChar, 20).Value = Convert.ToString(respuestaListaClientes.Customer.Identificacion);
                                    cmd.Parameters.Add("pnombre", OracleType.VarChar, 60).Value = respuestaListaClientes.Customer.Nombre;
                                    if (respuestaListaClientes.Customer.Direcciones.Count == 0)
                                    {
                                        cmd.Parameters.Add("pdireccion", OracleType.VarChar, 240).Value = "ND";
                                    }
                                    else
                                    {
                                        cmd.Parameters.Add("pdireccion", OracleType.VarChar, 240).Value = respuestaListaClientes.Customer.Direcciones[0].Detalle;
                                    }
                                    cmd.Parameters.Add("ptelefono", OracleType.VarChar, 20).Value = telefono;
                                    cmd.Parameters.Add("pemail", OracleType.VarChar, 120).Value = respuestaListaClientes.Customer.Correo;

                                    if (string.IsNullOrEmpty(respuestaListaClientes.Customer.Genero))
                                    {
                                        cmd.Parameters.Add("pgenero", OracleType.VarChar, 1).Value = "N";
                                    }
                                    else
                                    {
                                        cmd.Parameters.Add("pgenero", OracleType.VarChar, 1).Value = respuestaListaClientes.Customer.Genero;
                                    }

                                    cmd.Parameters.Add("pfnacimiento", OracleType.DateTime, 30).Value = Convert.ToDateTime(respuestaListaClientes.Customer.fecha_de_nacimiento);
                                    cmd.Parameters.Add("pmensaje", OracleType.VarChar, 255).Direction = ParameterDirection.Output;
                                    cmd.ExecuteNonQuery();
                                    string respuesta3 = cmd.Parameters["pmensaje"].Value.ToString();
                                    cmd.Parameters.Clear();

                                    logsFile.WriteLogs($"INSERTO EN REG CLIENTE-- Nombre: {respuestaListaClientes.Customer.Nombre} Detalle: {respuestaListaClientes.Customer.Direcciones} Telefono {telefono} Correo: {respuestaListaClientes.Customer.Correo} Genero: {respuestaListaClientes.Customer.Genero} FechaDeNacimiento: {respuestaListaClientes.Customer.fecha_de_nacimiento}");

                                    //INSERTAR EVENTO
                                    string query4 = "NAF5M.NIDUX_LISTA_REGALOS.REG_EVENTO";
                                    cmd.CommandText = query4;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("pno_lista", OracleType.VarChar, 8).Value = Convert.ToString(respuestaListaRegalos.Giftlist.wish_id);
                                    cmd.Parameters.Add("pcod_cliente1", OracleType.VarChar, 15).Value = Convert.ToString(respuestaListaClientes.Customer.id);
                                    cmd.Parameters.Add("pcod_cliente2", OracleType.VarChar, 15).Value = Convert.ToString(respuestaListaRegalos.Giftlist.extraOwnerId);
                                    cmd.Parameters.Add("pfecha_crea", OracleType.DateTime, 30).Value = respuestaListaClientes.Customer.creado;
                                    cmd.Parameters.Add("ptipo_lista", OracleType.VarChar, 2).Value = Convert.ToString(respuestaListaRegalos.Giftlist.modeList);
                                    cmd.Parameters.Add("pfecha_evento", OracleType.DateTime, 30).Value = respuestaListaRegalos.Giftlist.eventDate;
                                    cmd.Parameters.Add("ptipo_envoltura", OracleType.VarChar, 1).Value = Convert.ToString(respuestaListaRegalos.Giftlist.EcoFriendly.id);
                                    cmd.Parameters.Add("pmensaje", OracleType.VarChar, 100).Direction = ParameterDirection.Output;
                                    cmd.ExecuteNonQuery();
                                    string respuesta4 = cmd.Parameters["pmensaje"].Value.ToString();
                                    cmd.Parameters.Clear();

                                    logsFile.WriteLogs($"INSERTO EN REG_EVENTO-- WishId: {respuestaListaRegalos.Giftlist.wish_id} ID: {respuestaListaClientes.Customer.id} ExtraOwnerId: {respuestaListaRegalos.Giftlist.extraOwnerId} Creado: {respuestaListaClientes.Customer.creado} ListType: {respuestaListaRegalos.Giftlist.ListType} EventDate: {respuestaListaRegalos.Giftlist.eventDate}");

                                }
                                else
                                {
                                    logsFile.WriteLogs("Error al consultar Cliente:" + respuestaListaRegalos.Giftlist.OwnerId + ", Status Code: " + responseGetCliente.StatusCode.ToString() + ", Mensaje Error: " + responseGetCliente.ErrorMessage);
                                }
                                #endregion

                                #endregion

                                //actualizo estado de la lista 
                                actualizar_Estado_IdWish(dtrEncabezado["ORDERID"].ToString(), "S");

                                logsFile.WriteLogs("FIN LISTA REGALOS CON EXITO");
                            }
                            else
                            {
                                logsFile.WriteLogs("Error al consultar lista de regalo: " + respuestaListaRegalos.Giftlist.wish_id.ToString() + ", Status Code: " + responseListaRegalo.StatusCode.ToString() + ", Mensaje Error: " + responseListaRegalo.ErrorMessage);
                            }
                        }
                        #endregion

                    }
                }
                catch (Exception ex)
                {
                    logsFile.WriteLogs("Error: " + ex.Message);
                }
            }

        }

        public void actualizar_Estado_IdWish(string orderId,string estado)
        {
            string Esquema = ConfigurationManager.AppSettings["Schema"].ToString();

            Conexion con = new Conexion();
            int record = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.AbrirConexion();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "" + Esquema + ".ACTUALIZO_WISK_ID_SINCRONIZADO";
                cmd.Parameters.AddWithValue("@ORDERID", orderId);
                cmd.Parameters.AddWithValue("@ESTADO", estado);
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

