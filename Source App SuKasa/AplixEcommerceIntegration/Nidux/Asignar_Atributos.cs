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
    public partial class Asignar_Atributos : Form
    {
        Conexion cnn = new Conexion();
        static string codigoArticulo;
        public int n = 0;
        public string atributosCodigos;
        static string com = ConfigurationManager.AppSettings["Company_Nidux"];

        static List<Clases.Valores_Atributos> lista_atributos_editados = new List<Clases.Valores_Atributos>();
        public Asignar_Atributos(string cod_articulo)
        {
            InitializeComponent();
            codigoArticulo = cod_articulo;
        }

        private void Asignar_Atributos_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT VA.ID, VA.DESCRIPCION, AT.DESCRIPCION AS DESCRIP_ATRIBUTO from "+ com +".VALORES_ATRIBUTOS AS VA" +
                    " INNER JOIN " + com + ".ATRIBUTOS AS AT ON VA.ID_ATRIBUTO = AT.ID";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.CerrarConexion();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvAtributos.Rows.Add();

                    dgvAtributos.Rows[n].Cells[0].Value = item["ID"].ToString();
                    dgvAtributos.Rows[n].Cells[1].Value = item["DESCRIPCION"].ToString();
                    dgvAtributos.Rows[n].Cells[2].Value = item["DESCRIP_ATRIBUTO"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuscarAtributos(string atributo)
        {
            try
            {
                dgvAtributos.Rows.Clear();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT VA.ID, VA.DESCRIPCION, AT.DESCRIPCION AS DESCRIP_ATRIBUTO from " + com + ".VALORES_ATRIBUTOS AS VA" +
                    " INNER JOIN " + com + ".ATRIBUTOS AS AT ON VA.ID_ATRIBUTO = AT.ID WHERE VA.ID LIKE '%" + atributo + "%' OR VA.DESCRIPCION LIKE '%" + atributo + "%'";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.CerrarConexion();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvAtributos.Rows.Add();

                    Clases.Valores_Atributos product = lista_atributos_editados.FirstOrDefault(x => x.id == Convert.ToInt32(dt.Rows[n]["ID"].ToString()));

                    if (product == null)
                    {
                        dgvAtributos.Rows[n].Cells[0].Value = item["ID"].ToString();
                        dgvAtributos.Rows[n].Cells[1].Value = item["DESCRIPCION"].ToString();
                        dgvAtributos.Rows[n].Cells[2].Value = item["DESCRIP_ATRIBUTO"].ToString();
                    }
                    else
                    {
                        dgvAtributos.Rows[n].Cells[0].Value = product.id;
                        dgvAtributos.Rows[n].Cells[1].Value = product.nombre;
                        dgvAtributos.Rows[n].Cells[2].Value = product.atributo;
                        dgvAtributos.Rows[n].Cells[3].Value = product.activo;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtAtributos_TextChanged(object sender, EventArgs e)
        {
            BuscarAtributos(txtAtributos.Text.ToString());
        }

        private void dgvAtributos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvAtributos.EndEdit();
            if (e.ColumnIndex == 3)
            {
                Clases.Valores_Atributos atributo = new Clases.Valores_Atributos();

                atributo.id = Convert.ToInt32(this.dgvAtributos.CurrentRow.Cells[0].Value.ToString());
                atributo.nombre = this.dgvAtributos.CurrentRow.Cells[1].Value.ToString();
                atributo.atributo = this.dgvAtributos.CurrentRow.Cells[2].Value.ToString();
                atributo.activo = this.dgvAtributos.CurrentRow.Cells[3].Value.ToString();


                Clases.Valores_Atributos product = lista_atributos_editados.FirstOrDefault(x => x.id == Convert.ToInt32(this.dgvAtributos.CurrentRow.Cells[0].Value.ToString()));

                if (product == null)
                {
                    lista_atributos_editados.Add(atributo);
                }
                else
                {
                    lista_atributos_editados.RemoveAll(x => x.id == (atributo.id));
                    lista_atributos_editados.Add(atributo);
                }
                dgvAtributos.EndEdit();
            }
        }

        private void Asignar_Atributos_FormClosing(object sender, FormClosingEventArgs e)
        {
            lista_atributos_editados.Clear();
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            try
            {
                string cod_variacion = "";
                List<string> list = new List<string>();
                for (int i = 0; i < lista_atributos_editados.Count; i++)
                {
                    if (lista_atributos_editados[i].activo.Equals("True"))
                    {
                        cod_variacion = lista_atributos_editados[i].id + "," + cod_variacion;//codigo de nidux
                        list.Add(lista_atributos_editados[i].atributo);
                    }
                }
                var duplicateKeys = list.GroupBy(x => x)
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key);

                if (duplicateKeys.Count() == 0)
                {
                    cod_variacion = cod_variacion.Remove(cod_variacion.Length - 1);
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "" + com + ".ACT_ATRIBUTOS_ARTICULOS_APP";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ATRIBUTOS", cod_variacion);
                    cmd.Parameters.AddWithValue("@ARTICULO", codigoArticulo);
                    cmd.ExecuteNonQuery();
                    cnn.CerrarConexion();
                    n = 1;
                    atributosCodigos = cod_variacion;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se puede escoger dos Atributos iguales para un mismo Artículo", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
