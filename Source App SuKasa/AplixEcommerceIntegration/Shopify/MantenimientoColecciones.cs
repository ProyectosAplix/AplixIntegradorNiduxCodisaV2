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
using AplixEcommerceIntegration.Shopify.Clases;


namespace AplixEcommerceIntegration.Shopify
{
    public partial class MantenimientoColecciones : Form
    {
        Metodos_Articulos mta = new Metodos_Articulos();
        static List<Coleccion> lista_coleccion = new List<Coleccion>();
        DataTable dt_coleccion;
        public DataTable dt_colecciones;
        public MantenimientoColecciones()
        {
            InitializeComponent();
        }

        private void MantenimientoColecciones_Load(object sender, EventArgs e)
        {
            CargaColeccion();
        }

        public void CargaColeccion()
        {
            dt_coleccion = mta.articulos_coleccion_mantenimiento();

            if (dt_coleccion == null)
            {
                MessageBox.Show("Error en Procedimiento de Obtener Colecciones", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (dt_coleccion.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos de Colecciones para mostrar", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //datos normales de articulos
                    foreach (DataRow item in dt_coleccion.Rows)
                    {
                        int n = dgvColeccion.Rows.Add();

                        dgvColeccion.Rows[n].Cells[0].Value = item["ID"].ToString();
                        dgvColeccion.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();

                    }
                }
            }
        }

        public void Buscar_Coleccion(string coleccion)
        {
            dgvColeccion.Rows.Clear();
            if (cbbColeccion.Text.Equals("Código"))
            {
                DataRow[] resultado = dt_coleccion.Select("CONVERT(ID, System.String) LIKE '%" + coleccion + "%'");
                foreach (DataRow item in resultado)
                {
                    int n = dgvColeccion.Rows.Add();

                    Coleccion product = lista_coleccion.FirstOrDefault(x => x.id == item["ID"].ToString());

                    if (product == null)
                    {
                        dgvColeccion.Rows[n].Cells[0].Value = item["ID"].ToString();
                        dgvColeccion.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                        dgvColeccion.Rows[n].Cells[2].Value = false;
                    }
                    else
                    {
                        dgvColeccion.Rows[n].Cells[0].Value = product.id;
                        dgvColeccion.Rows[n].Cells[1].Value = product.nombre;
                        dgvColeccion.Rows[n].Cells[2].Value = product.activo;
                    }
                }
            }
            else
            {
                DataRow[] resultado = dt_coleccion.Select("CONVERT(NOMBRE, System.String) LIKE '%" + coleccion + "%'");
                foreach (DataRow item in resultado)
                {
                    int n = dgvColeccion.Rows.Add();

                    Coleccion product = lista_coleccion.FirstOrDefault(x => x.id == item["ID"].ToString());

                    if (product == null)
                    {
                        dgvColeccion.Rows[n].Cells[0].Value = item["ID"].ToString();
                        dgvColeccion.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                        dgvColeccion.Rows[n].Cells[2].Value = false;
                    }
                    else
                    {
                        dgvColeccion.Rows[n].Cells[0].Value = product.id;
                        dgvColeccion.Rows[n].Cells[1].Value = product.nombre;
                        dgvColeccion.Rows[n].Cells[2].Value = product.activo;
                    }
                }
            }
        }

        private void txtColecciones_TextChanged(object sender, EventArgs e)
        {
            Buscar_Coleccion(txtColecciones.Text);
        }

        private void dgvColeccion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                dgvColeccion.EndEdit();
                Coleccion coleccion = new Coleccion();

                coleccion.id = this.dgvColeccion.CurrentRow.Cells[0].Value.ToString();
                coleccion.nombre = this.dgvColeccion.CurrentRow.Cells[1].Value.ToString();
                coleccion.activo = this.dgvColeccion.CurrentRow.Cells[2].Value.ToString();

                Coleccion product = lista_coleccion.FirstOrDefault(x => x.id == this.dgvColeccion.CurrentRow.Cells[0].Value.ToString());

                if (product == null)
                {
                    lista_coleccion.Add(coleccion);
                }
                else
                {
                    lista_coleccion.RemoveAll(x => x.id == (coleccion.id));
                    lista_coleccion.Add(coleccion);
                }
                dgvColeccion.EndEdit();
            }
        }

        private void btnGuardarColeccion_Click(object sender, EventArgs e)
        {
            if (lista_coleccion.Count == 0)
            {
                MessageBox.Show("No se seleccionó ninguna colección para el Artículo", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                dt_colecciones = new DataTable();
                dt_colecciones.Columns.Add("ID", typeof(string));
                dt_colecciones.Columns.Add("NOMBRE", typeof(string));
                foreach (Coleccion str in lista_coleccion)
                {
                    DataRow row = dt_colecciones.NewRow();
                    row["ID"] = str.id;
                    row["NOMBRE"] = str.nombre;
                    dt_colecciones.Rows.Add(row);
                }

                this.Close();
            }
        }

        private void MantenimientoColecciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            lista_coleccion.Clear();
        }

    }
}
