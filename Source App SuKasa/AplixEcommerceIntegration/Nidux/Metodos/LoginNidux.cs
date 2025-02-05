using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using System.Data;
using System.Configuration;

namespace AplixEcommerceIntegration.Nidux.Metodos
{
    class LoginNidux
    {
        Globales.Conexion cnn = new Globales.Conexion();
        static string com = ConfigurationManager.AppSettings["Company_Nidux"];
        public string Obtener_Token()
        {
            try
            {
                string user = "";
                string pws = "";
                int storeid = 0;

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT USUARIO, CONTRASENA, STOREID  FROM " + com + ".CREDENCIALES";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataReader da = cmd.ExecuteReader();
                while (da.Read())
                {
                    user = da["USUARIO"].ToString();
                    pws = da["CONTRASENA"].ToString();
                    storeid = Int32.Parse(da["STOREID"].ToString());
                }
                cnn.CerrarConexion();

                var client = new RestClient("https://api.nidux.dev/login");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");

                request.AddParameter("application/json", "{\r\n  \"username\":\"" + user + "\",\r\n \"password\":\"" + pws + "\",\r\n \"storeId\":" + storeid + "\r\n}", ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                Clases.Login lista_token = JsonConvert.DeserializeObject<Clases.Login>(response.Content);
                string token = lista_token.token.ToString();

                return token;
            }
            catch (Exception)
            {
                return "Error en login";
            }
        }
    }
}
