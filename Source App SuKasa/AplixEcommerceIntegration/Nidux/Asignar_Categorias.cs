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
    public partial class Asignar_Categorias : Form
    {
        Conexion cnn = new Conexion();
        static string codigoArticulo;
        public int n = 0;
        public string codCat;
        static string com = ConfigurationManager.AppSettings["Company_Nidux"];

        static List<Clases.Categorias> lista_categorias_editados = new List<Clases.Categorias>();
        public Asignar_Categorias(string cod_articulo)
        {
            InitializeComponent();
            codigoArticulo = cod_articulo;
        }

        private void Asignar_Categorias_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT NOMBRE, CODIGO_NIDUX, SUBCATEGORIA_NIDUX FROM " + com + ".CATEGORIAS WHERE CODIGO_NIDUX IS NOT NULL";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.CerrarConexion();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvCategoriasSelect.Rows.Add();

                    dgvCategoriasSelect.Rows[n].Cells[0].Value = item["NOMBRE"].ToString();
                    dgvCategoriasSelect.Rows[n].Cells[1].Value = item["CODIGO_NIDUX"].ToString();//codigo de nidux
                    dgvCategoriasSelect.Rows[n].Cells[2].Value = item["SUBCATEGORIA_NIDUX"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuscarCategoria(string categoria)
        {
            try
            {
                dgvCategoriasSelect.Rows.Clear();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT NOMBRE, CODIGO_NIDUX, SUBCATEGORIA_NIDUX FROM " + com + ".CATEGORIAS WHERE (CODIGO_NIDUX IS NOT NULL) AND (NOMBRE LIKE '%" + categoria + "%' OR CODIGO_NIDUX  LIKE '%" + categoria + "%')";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.CerrarConexion();

                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvCategoriasSelect.Rows.Add();

                    Clases.Categorias product = lista_categorias_editados.FirstOrDefault(x => x.codigo_categoria == dt.Rows[n]["CODIGO_NIDUX"].ToString());

                    if (product == null)
                    {
                        dgvCategoriasSelect.Rows[n].Cells[0].Value = item["NOMBRE"].ToString();
                        dgvCategoriasSelect.Rows[n].Cells[1].Value = item["CODIGO_NIDUX"].ToString();//codigo de nidux
                        dgvCategoriasSelect.Rows[n].Cells[2].Value = item["SUBCATEGORIA_NIDUX"].ToString();
                    }
                    else
                    {
                        dgvCategoriasSelect.Rows[n].Cells[0].Value = product.nombre;
                        dgvCategoriasSelect.Rows[n].Cells[1].Value = product.codigo_categoria;
                        dgvCategoriasSelect.Rows[n].Cells[2].Value = product.categoria_padre;
                        dgvCategoriasSelect.Rows[n].Cells[3].Value = product.activo;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCategorias_TextChanged(object sender, EventArgs e)
        {
            BuscarCategoria(txtCategorias.Text.ToString());
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            try
            {
                string cod_nidux = "";
                for (int i = 0; i < lista_categorias_editados.Count; i++)
                {
                    if (lista_categorias_editados[i].activo.Equals("True"))
                    {
                        cod_nidux = lista_categorias_editados[i].codigo_categoria + "," + cod_nidux;
                    }
                }
                cod_nidux = cod_nidux.Remove(cod_nidux.Length - 1);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "" + com + ".ACT_CATEGORIAS_ARTICULO_APP_SIMPLE";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CATEGORIAS", cod_nidux);
                cmd.Parameters.AddWithValue("@ARTICULO", codigoArticulo);
                cmd.ExecuteNonQuery();
                cnn.CerrarConexion();
                n = 1;
                codCat = cod_nidux;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }

        private void dgvCategoriasSelect_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvCategoriasSelect.EndEdit();
            if (e.ColumnIndex == 3)
            {
                Clases.Categorias cat = new Clases.Categorias();

                cat.nombre = this.dgvCategoriasSelect.CurrentRow.Cells[0].Value.ToString();
                cat.codigo_categoria = this.dgvCategoriasSelect.CurrentRow.Cells[1].Value.ToString();
                cat.categoria_padre = this.dgvCategoriasSelect.CurrentRow.Cells[2].Value.ToString();
                cat.activo = this.dgvCategoriasSelect.CurrentRow.Cells[3].Value.ToString();


                Clases.Categorias product = lista_categorias_editados.FirstOrDefault(x => x.codigo_categoria == this.dgvCategoriasSelect.CurrentRow.Cells[1].Value.ToString());

                if (product == null)
                {
                    lista_categorias_editados.Add(cat);
                }
                else
                {
                    lista_categorias_editados.RemoveAll(x => x.codigo_categoria == (cat.codigo_categoria));
                    lista_categorias_editados.Add(cat);
                }
                dgvCategoriasSelect.EndEdit();
            }
        }

        private void Asignar_Categorias_FormClosing(object sender, FormClosingEventArgs e)
        {
            lista_categorias_editados.Clear();
        }

    }
}
