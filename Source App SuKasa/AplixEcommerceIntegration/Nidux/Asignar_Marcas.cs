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
using System.Data.SqlClient;
using System.Configuration;

namespace AplixEcommerceIntegration.Nidux
{
    public partial class Asignar_Marcas : Form
    {
        Conexion cnn = new Conexion();
        static string codigoArticulo;
        public int n = 0;
        public string codMarca;
        static string com = ConfigurationManager.AppSettings["Company_Nidux"];

        public Asignar_Marcas(string cod_articulo)
        {
            InitializeComponent();
            codigoArticulo = cod_articulo;
        }

        private void Asignar_Marcas_Load(object sender, EventArgs e)
        {
            try
            {
                dgvMarcas.AllowUserToAddRows = false;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT CODIGO_NIDUX, DESCRIPCION_NIDUX FROM " + com + ".MARCAS";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.CerrarConexion();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvMarcas.Rows.Add();

                    dgvMarcas.Rows[n].Cells[0].Value = item["CODIGO_NIDUX"].ToString();
                    dgvMarcas.Rows[n].Cells[1].Value = item["DESCRIPCION_NIDUX"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuscarMarcas(string marcas)
        {
            try
            {
                dgvMarcas.Rows.Clear();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT CODIGO_NIDUX, DESCRIPCION_NIDUX FROM " + com + ".MARCAS WHERE CODIGO_NIDUX LIKE '%" + marcas + "%' OR DESCRIPCION_NIDUX LIKE '%" + marcas + "%'";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.CerrarConexion();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvMarcas.Rows.Add();

                    dgvMarcas.Rows[n].Cells[0].Value = item["CODIGO_NIDUX"].ToString();
                    dgvMarcas.Rows[n].Cells[1].Value = item["DESCRIPCION_NIDUX"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtMarcas_TextChanged(object sender, EventArgs e)
        {
            BuscarMarcas(txtMarcas.Text.ToString());
        }

        private void dgvMarcas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                try
                {
                    int cod_nidux = Convert.ToInt32(this.dgvMarcas.SelectedCells[0].Value.ToString());
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "" + com + ".ACT_MARCA_ARTICULO_APP_SIMPLE";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CODIGO_NIDUX", cod_nidux);
                    cmd.Parameters.AddWithValue("@ARTICULO", codigoArticulo);
                    cmd.ExecuteNonQuery();
                    cnn.CerrarConexion();
                    n = 1;
                    codMarca = cod_nidux.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.Close();
            }
        }
    }
}
