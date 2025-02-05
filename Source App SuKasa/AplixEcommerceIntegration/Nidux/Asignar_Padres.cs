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
    public partial class Asignar_Padres : Form
    {
        Conexion cnn = new Conexion();
        public int n = 0;
        public string articulo;
        static string codigoArticulo;
        static string com = ConfigurationManager.AppSettings["Company_Nidux"];

        public Asignar_Padres(string cod_articulo)
        {
            InitializeComponent();
            codigoArticulo = cod_articulo;
        }

        private void Asignar_Padres_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable(); 
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT ARTICULO, NOMBRE FROM " + com +".ARTICULOS WHERE ACTIVO='S'";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.CerrarConexion();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvPadre.Rows.Add();

                    dgvPadre.Rows[n].Cells[0].Value = item["ARTICULO"].ToString();
                    dgvPadre.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BuscarPadres(string padre)
        {
            try
            {
                dgvPadre.Rows.Clear();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT ARTICULO, NOMBRE FROM " + com + ".ARTICULOS WHERE ACTIVO ='S' AND (ARTICULO LIKE '%" + padre + "%' OR NOMBRE LIKE '%" + padre + "%')";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.CerrarConexion();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvPadre.Rows.Add();

                    dgvPadre.Rows[n].Cells[0].Value = item["ARTICULO"].ToString();
                    dgvPadre.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPadres_TextChanged(object sender, EventArgs e)
        {
            BuscarPadres(txtPadres.Text.ToString());
        }

        private void dgvPadre_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                try
                {
                    string cod_padre = dgvPadre.SelectedCells[0].Value.ToString();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "" + com + ".ACT_PADRE_DATOS_APP";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CODIGO", cod_padre);
                    cmd.Parameters.AddWithValue("@ARTICULO", codigoArticulo);
                    cmd.ExecuteNonQuery();
                    cnn.CerrarConexion();
                    n = 1;
                    articulo = cod_padre;
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
