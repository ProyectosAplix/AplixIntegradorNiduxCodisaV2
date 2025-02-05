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
using System.Configuration;

namespace AplixEcommerceIntegration.Nidux
{
    public partial class Activar_Articulos : Form
    {
        Globales.Conexion cnn = new Conexion();
        public int n = 0;
        static List<Clases.Articulos> lista_articulos_editados = new List<Clases.Articulos>();
        static string com = ConfigurationManager.AppSettings["Company_Nidux"];

        public Activar_Articulos()
        {
            InitializeComponent();
        }

        private void Activar_Articulos_Load(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();

                cmd.CommandText = "SELECT AR.ARTICULO, AR.NOMBRE FROM " + com + ".ARTICULOS AR INNER JOIN " + com + ".ARTICULOS_CANTIDAD AS AC ON AR.ARTICULO = AC.ARTICULO" +
                        " INNER JOIN " + com + ".ARTICULOS_PRECIO AS AP ON AR.ARTICULO = AP.ARTICULO WHERE AR.ACTIVO = 'N' OR AR.ACTIVO IS NULL";

                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);
                cnn.CerrarConexion();

                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvArticulos.Rows.Add();

                    dgvArticulos.Rows[n].Cells[0].Value = item["ARTICULO"].ToString();
                    dgvArticulos.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Buscar_Articulos(string articulo)
        {
            try
            {
                dgvArticulos.Rows.Clear();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();

                cmd.CommandText = "SELECT AR.ARTICULO, AR.NOMBRE FROM " + com + ".ARTICULOS AR INNER JOIN " + com + ".ARTICULOS_CANTIDAD AS AC ON AR.ARTICULO = AC.ARTICULO " +
                    " INNER JOIN " + com + ".ARTICULOS_PRECIO AS AP ON AR.ARTICULO = AP.ARTICULO WHERE (AR.ARTICULO LIKE '%" + articulo + "%' OR AR.NOMBRE LIKE '%" + articulo + "%') AND (AR.ACTIVO = 'N' OR AR.ACTIVO IS NULL)";

                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);
                cnn.CerrarConexion();

                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvArticulos.Rows.Add();

                    Clases.Articulos product = lista_articulos_editados.FirstOrDefault(x => x.sku == dt.Rows[n]["ARTICULO"].ToString());

                    if (product == null)
                    {
                        dgvArticulos.Rows[n].Cells[0].Value = item["ARTICULO"].ToString();
                        dgvArticulos.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                    }
                    else
                    {
                        dgvArticulos.Rows[n].Cells[0].Value = product.sku;
                        dgvArticulos.Rows[n].Cells[1].Value = product.nombre;
                        dgvArticulos.Rows[n].Cells[2].Value = product.activo;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtArticulo_TextChanged(object sender, EventArgs e)
        {
            Buscar_Articulos(txtArticulo.Text);
        }

        private void Activar_Articulos_FormClosing(object sender, FormClosingEventArgs e)
        {
            lista_articulos_editados.Clear();
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            try
            {

                for (int i = 0; i < lista_articulos_editados.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    if (lista_articulos_editados[i].activo.Equals("True"))
                    {
                        cmd.CommandText = "" + com + ".ACT_ACTIVAR_ARTICULOS_APP";
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ARTICULO", lista_articulos_editados[i].sku.ToString());
                        cmd.Parameters.AddWithValue("@ACTIVO", "S");

                        cmd.ExecuteNonQuery();
                    }
                    cnn.CerrarConexion();
                }
                n = 1;
                MessageBox.Show("Artículos actualizados con Éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvArticulos.EndEdit();
            if (e.ColumnIndex == 2)
            {
                Clases.Articulos art = new Clases.Articulos();

                art.sku = this.dgvArticulos.CurrentRow.Cells[0].Value.ToString();
                art.nombre = this.dgvArticulos.CurrentRow.Cells[1].Value.ToString();
                art.activo = this.dgvArticulos.CurrentRow.Cells[2].Value.ToString();


                Clases.Articulos product = lista_articulos_editados.FirstOrDefault(x => x.sku == this.dgvArticulos.CurrentRow.Cells[0].Value.ToString());

                if (product == null)
                {
                    lista_articulos_editados.Add(art);
                }
                else
                {
                    lista_articulos_editados.RemoveAll(x => x.sku == (art.sku));
                    lista_articulos_editados.Add(art);
                }
                dgvArticulos.EndEdit();
            }
        }
    }
}
