using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AplixEcommerceIntegration.Globales;
using Newtonsoft.Json;
using RestSharp;
using System.Data.SqlClient;
using System.Configuration;

namespace AplixEcommerceIntegration.Nidux
{
    public partial class Agregar_Marcas : Form
    {
        Conexion cnn = new Conexion();
        public int n = 0;
        static string com = ConfigurationManager.AppSettings["Company_Nidux"];
        public Agregar_Marcas()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            if (nombre != "")
            {
                try
                {
                    //agregamos la categoria a Nidux
                    Metodos.LoginNidux obj_metodos = new Metodos.LoginNidux();
                    string token = obj_metodos.Obtener_Token();
                    if (token.Equals("Error en login"))
                    {
                        MessageBox.Show("Error en el login", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        var client = new RestClient("https://api.nidux.dev/v1/brands/");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Authorization", "Bearer " + token);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddParameter("application/json", "{\r\n   \"nombre\":\"" + nombre + "\"\r\n}", ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);

                        if ((int)response.StatusCode == 200)
                        {
                            Clases.Respuesta_Marcas lista_respuesta = JsonConvert.DeserializeObject<Clases.Respuesta_Marcas>(response.Content);
                            int id = lista_respuesta.id;

                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn.AbrirConexion();
                            cmd.CommandText = "" + com + ".INSERTAR_MARCAS_APP_SIMPLE";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@NOMBRE", nombre);
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.ExecuteNonQuery();
                            cnn.CerrarConexion();
                            MessageBox.Show("Marca agregada con éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            n = 1;
                            txtNombre.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Error al ingresar marcas en Nidux", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Campo Nombre Marca vacío, por favor ingresa un nombre para la nueva marca", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Agregar_Marcas_Load(object sender, EventArgs e)
        {

        }
    }
}
