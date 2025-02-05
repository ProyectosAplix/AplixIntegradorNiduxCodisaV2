using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AplixEcommerceIntegration.Globales;
using RestSharp;
using Newtonsoft.Json;
using System.Configuration;

namespace AplixEcommerceIntegration.Nidux
{
    public partial class Agregar_Categorias : Form
    {
        Conexion cnn = new Conexion();
        public int n = 0;
        static string com = ConfigurationManager.AppSettings["Company_Nidux"];
        public Agregar_Categorias()
        {
            InitializeComponent();
            CargaDatosCombobox();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //Ingresa la nueva Categoria a nidux
            string nombre = txtNombre.Text;
            if (nombre != "")
            {
                string descripcion = txtDescripcion.Text;
                string padre = cbbCategoriasPadre.Text;
                int codigo_padre = 0;
                try
                {
                    if (padre == "")
                    {
                        codigo_padre = 0;
                        string categoria = AgregarCategoria(nombre, descripcion, codigo_padre);
                        if (categoria.Equals("Error"))
                        {
                            MessageBox.Show("Error en Método de agregar categorias a Nidux", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            //ingresamos la categoria a base de datos
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn.AbrirConexion();
                            cmd.CommandText = "" + com + ".INSERTAR_CATEGORIAS_APP_SIMPLE";
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@NOMBRE", nombre);
                            cmd.Parameters.AddWithValue("@DESCRIPCION", descripcion);
                            cmd.Parameters.AddWithValue("@CODIGO", Convert.ToInt32(categoria));//codigo de nidux
                            cmd.Parameters.AddWithValue("@CODIGO_PADRE", codigo_padre);
                            cmd.ExecuteNonQuery();
                            cnn.CerrarConexion();
                            
                            MessageBox.Show("Categoría agregada con éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            n = 1;
                            txtNombre.Clear();
                            txtDescripcion.Clear();
                            cbbCategoriasPadre.Text = "";
                        }
                    }
                    else
                    {
                        codigo_padre = Convert.ToInt32(cbbCategoriasPadre.SelectedValue.ToString());

                        string categoria = AgregarCategoria(nombre, descripcion, codigo_padre);
                        if (categoria.Equals("Error"))
                        {
                            MessageBox.Show("Error en Método de agregar categorias a Nidux", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            //ingresamos la categoria a base de datos
                            SqlCommand cmd_app = new SqlCommand();
                            cmd_app.Connection = cnn.AbrirConexion();
                            cmd_app.CommandText = "" + com + ".INSERTAR_CATEGORIAS_APP_SIMPLE";
                            cmd_app.CommandTimeout = 0;
                            cmd_app.CommandType = CommandType.StoredProcedure;
                            cmd_app.Parameters.AddWithValue("@NOMBRE", nombre);
                            cmd_app.Parameters.AddWithValue("@DESCRIPCION", descripcion);
                            cmd_app.Parameters.AddWithValue("@CODIGO", Convert.ToInt32(categoria));//codigo de nidux
                            cmd_app.Parameters.AddWithValue("@CODIGO_PADRE", codigo_padre);
                            cmd_app.ExecuteNonQuery();
                            cnn.CerrarConexion();
                            MessageBox.Show("Categoría agregada con éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            n = 1;
                            txtNombre.Clear();
                            txtDescripcion.Clear();
                            cbbCategoriasPadre.Text = "";
                        }
                    }
                    cbbCategoriasPadre.DataSource = null;
                    CargaDatosCombobox();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Campo Nombre Categoría vacío, por favor ingresa un nombre para la nueva categoría", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void CargaDatosCombobox()
        {
            try
            { 
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT CONCAT(NOMBRE,' (',CODIGO_NIDUX,')') AS NOMBRE, CODIGO_NIDUX FROM " + com + ".CATEGORIAS WHERE CODIGO_NIDUX IS NOT NULL";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cbbCategoriasPadre.DataSource = dt;
                cbbCategoriasPadre.DisplayMember = "NOMBRE";
                cbbCategoriasPadre.ValueMember = "CODIGO_NIDUX";
                //foreach (DataRow dr in dt.Rows)
                //{
                //    cbbCategoriasPadre.Items.Add(dr["NOMBRE"].ToString());
                //}
                cnn.CerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string AgregarCategoria(string nombre, string descripcion, int codigo_padre)
        {
            string cat_id = "";
            Metodos.LoginNidux obj_metodos = new Metodos.LoginNidux();
            string token = obj_metodos.Obtener_Token();
            if (token.Equals("Error en login"))
            {
                MessageBox.Show("Error en el login", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var client = new RestClient("https://api.nidux.dev/v1/categories/");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\r\n   \"nombre\":\"" + nombre + "\",\r\n   \"categoria_padre\":" + codigo_padre + ",\r\n   \"descripcion\":\"<p>" + descripcion + "</p>\",\r\n   \"estado\":1,\r\n   \"peso_precedencia\":0,\r\n   \"categoria_en_malls\":0,\r\n   \"traducciones\":[\r\n      {\r\n         \"idioma\":1,\r\n         \"nombre\":\"Name Dafualt \",\r\n         \"descripcion\":\"<p>Description Dafualt</p>\"\r\n      }\r\n   ]\r\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if ((int)response.StatusCode == 200)
                {
                    Clases.Respuesta_Categoria lista_respuesta = JsonConvert.DeserializeObject<Clases.Respuesta_Categoria>(response.Content);
                    cat_id = lista_respuesta.id.ToString();
                }
                else
                {
                    cat_id = "Error";
                }
            }
            return cat_id;
        }

        private void Agregar_Categorias_Load(object sender, EventArgs e)
        {

        }
    }
}
