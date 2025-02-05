using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AplixEcommerceIntegration.Shopify.Metodos;

namespace AplixEcommerceIntegration.Shopify.Metodos
{
    public partial class AgregarArticulos : Form
    {
        Metodos_Articulos mta = new Metodos_Articulos();
        DataTable dt_Articulos;
        public string articulo = "";
        public string impuesto = "";
        public AgregarArticulos()
        {
            InitializeComponent();
        }

        public void CargaArticulos()
        {
            dt_Articulos = mta.articulos_nuevos();

            if (dt_Articulos == null)
            {
                MessageBox.Show("Error en Procedimiento de Obtener Artículos", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (dt_Articulos.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos de Artículos para mostrar", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //datos normales de articulos
                    foreach (DataRow item in dt_Articulos.Rows)
                    {
                        int n = dgvArticulos.Rows.Add();

                        dgvArticulos.Rows[n].Cells[0].Value = item["SKU"].ToString();
                        dgvArticulos.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();

                    }
                }
            }
        }

        private void AgregarArticulos_Load(object sender, EventArgs e)
        {
            CargaArticulos();
        }

        public void BuscarArticulos(string articulo)
        {
            dgvArticulos.Rows.Clear();
            if (cbbArticulo.Text.Equals("Código"))
            {
                DataRow[] resultado = dt_Articulos.Select("CONVERT(SKU, System.String) LIKE '%" + articulo + "%'");
                foreach (DataRow item in resultado)
                {
                    int n = dgvArticulos.Rows.Add();
                    dgvArticulos.Rows[n].Cells[0].Value = item["SKU"].ToString();
                    dgvArticulos.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                    
                }
            }
            else
            {
                DataRow[] resultado = dt_Articulos.Select("CONVERT(NOMBRE, System.String) LIKE '%" + articulo + "%'");
                foreach (DataRow item in resultado)
                {
                    int n = dgvArticulos.Rows.Add();
                    dgvArticulos.Rows[n].Cells[0].Value = item["SKU"].ToString();
                    dgvArticulos.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                }
            }
        }

        private void txtArticulo_TextChanged(object sender, EventArgs e)
        {
            BuscarArticulos(txtArticulo.Text);
        }

        private void btnGuardarArticulos_Click(object sender, EventArgs e)
        {
            articulo = dgvArticulos.SelectedCells[0].Value.ToString();
            this.Close();
        }

        private void dgvArticulos_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            articulo = dgvArticulos.SelectedCells[0].Value.ToString();
            this.Close();
        }

    }
}
