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
    public partial class MantenimientoVariantes : Form
    {
        Metodos_Articulos mta = new Metodos_Articulos();
        Metodos_Opciones mto = new Metodos_Opciones();

        DataTable dt_Articulos;
        DataTable dt_Variante;
        public DataTable dt_Variantes_lista;

        static List<Variantes> lista_variantes = new List<Variantes>();
        public MantenimientoVariantes()
        {
            InitializeComponent();
        }

        public void CargaArticulos()
        {
            dt_Articulos = mta.articulos_variantes();

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

        public void CargaOpcionesArticulos()
        {
            dt_Variante = mto.opciones_articulos();

            if (dt_Variante == null)
            {
                MessageBox.Show("Error en Procedimiento de Obtener Opciones de Artículos", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (dt_Variante.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos de Opciones para mostrar", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //datos normales de variantes
                    foreach (DataRow item in dt_Variante.Rows)
                    {
                        int n = dgvVariantes.Rows.Add();

                        dgvVariantes.Rows[n].Cells[0].Value = item["CODIGO"].ToString();
                        dgvVariantes.Rows[n].Cells[1].Value = item["DESCRIPCION"].ToString();
                        dgvVariantes.Rows[n].Cells[2].Value = item["OPCION"].ToString();
                        dgvVariantes.Rows[n].Cells[3].Value = false;

                    }
                }
            }
        }

        private void MantenimientoVariantes_Load(object sender, EventArgs e)
        {
            CargaArticulos();
            CargaOpcionesArticulos();
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

        private void dgvArticulos_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            lblArticulo.Text = this.dgvArticulos.CurrentRow.Cells[1].Value.ToString();
            lblCodigo.Text = this.dgvArticulos.CurrentRow.Cells[0].Value.ToString();
        }

        private void txtVariante_TextChanged(object sender, EventArgs e)
        {
            Buscar_Variantes(txtVariante.Text);
        }

        public void Buscar_Variantes(string variante)
        {
            dgvVariantes.Rows.Clear();
            if (cbbVariante.Text.Equals("Código"))
            {
                DataRow[] resultado = dt_Variante.Select("CONVERT(CODIGO, System.String) LIKE '%" + variante + "%'");
                foreach (DataRow item in resultado)
                {
                    int n = dgvVariantes.Rows.Add();

                    Variantes product = lista_variantes.FirstOrDefault(x => x.codigo == item["CODIGO"].ToString());

                    if (product == null)
                    {
                        dgvVariantes.Rows[n].Cells[0].Value = item["CODIGO"].ToString();
                        dgvVariantes.Rows[n].Cells[1].Value = item["DESCRIPCION"].ToString();
                        dgvVariantes.Rows[n].Cells[2].Value = item["OPCION"].ToString();
                        dgvVariantes.Rows[n].Cells[3].Value = false;
                    }
                    else
                    {
                        dgvVariantes.Rows[n].Cells[0].Value = product.codigo;
                        dgvVariantes.Rows[n].Cells[1].Value = product.descripcion;
                        dgvVariantes.Rows[n].Cells[2].Value = product.opcion;
                        dgvVariantes.Rows[n].Cells[3].Value = product.elimanado;
                    }
                }
            }
            else
            {
                DataRow[] resultado = dt_Variante.Select("CONVERT(DESCRIPCION, System.String) LIKE '%" + variante + "%'");
                foreach (DataRow item in resultado)
                {
                    int n = dgvVariantes.Rows.Add();

                    Variantes product = lista_variantes.FirstOrDefault(x => x.codigo == item["CODIGO"].ToString());

                    if (product == null)
                    {
                        dgvVariantes.Rows[n].Cells[0].Value = item["CODIGO"].ToString();
                        dgvVariantes.Rows[n].Cells[1].Value = item["DESCRIPCION"].ToString();
                        dgvVariantes.Rows[n].Cells[2].Value = item["OPCION"].ToString();
                        dgvVariantes.Rows[n].Cells[3].Value = false;
                    }
                    else
                    {
                        dgvVariantes.Rows[n].Cells[0].Value = product.codigo;
                        dgvVariantes.Rows[n].Cells[1].Value = product.descripcion;
                        dgvVariantes.Rows[n].Cells[2].Value = product.opcion;
                        dgvVariantes.Rows[n].Cells[3].Value = product.elimanado;
                    }
                }
            }
        }

        private void dgvVariantes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                dgvVariantes.EndEdit();
                Variantes variantes = new Variantes();

                variantes.codigo = this.dgvVariantes.CurrentRow.Cells[0].Value.ToString();
                variantes.descripcion = this.dgvVariantes.CurrentRow.Cells[1].Value.ToString();
                variantes.opcion = this.dgvVariantes.CurrentRow.Cells[2].Value.ToString();
                variantes.elimanado = this.dgvVariantes.CurrentRow.Cells[3].Value.ToString();

                Variantes product = lista_variantes.FirstOrDefault(x => x.codigo == this.dgvVariantes.CurrentRow.Cells[0].Value.ToString());

                if (product == null)
                {
                    lista_variantes.Add(variantes);
                }
                else
                {
                    lista_variantes.RemoveAll(x => x.codigo == (variantes.codigo));
                    lista_variantes.Add(variantes);
                }
                dgvVariantes.EndEdit();
            }
        }

        private void btnGuardarVariantes_Click(object sender, EventArgs e)
        {
            if (lblCodigo.Text.Equals("..."))
            {
                MessageBox.Show("No se ha seleccionado un artículo para definir variantes", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    if (lista_variantes.Count == 0)
                    {
                        //lista vacia
                        MessageBox.Show("No se ha seleccionado una variante para el Artículo", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string cod_variacion = "";
                        List<string> list = new List<string>();
                        for (int i = 0; i < lista_variantes.Count; i++)
                        {
                            if (lista_variantes[i].elimanado.Equals("True"))
                            {
                                cod_variacion = lista_variantes[i].codigo + "," + cod_variacion;//codigo de nidux
                                list.Add(lista_variantes[i].opcion);
                            }
                        }

                        if (list.Count > 3)
                        {
                            MessageBox.Show("El límite de Opciones de Variantes por Artículo son tres", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            var duplicateKeys = list.GroupBy(x => x)
                                .Where(group => group.Count() > 1)
                                .Select(group => group.Key);

                            if (duplicateKeys.Count() == 0)
                            {
                                //nos traemos el precio y la cantidad
                                DataTable respuesta = mto.articulos_variantes(lblCodigo.Text);
                                string cantidad = "";
                                string precio = "";

                                foreach (DataRow item in respuesta.Rows)
                                {
                                    cantidad = item["CANTIDAD"].ToString();
                                    precio = item["PRECIO"].ToString();
                                }

                                cod_variacion = cod_variacion.Remove(cod_variacion.Length - 1);
                                cantidad = cantidad.Remove(cantidad.Length - 5);
                                precio = precio.Remove(precio.Length - 5);
                                dt_Variantes_lista = new DataTable();
                                dt_Variantes_lista.Columns.Add("ARTICULO", typeof(string));
                                dt_Variantes_lista.Columns.Add("CODIGO", typeof(string));
                                dt_Variantes_lista.Columns.Add("CANTIDAD", typeof(string));
                                dt_Variantes_lista.Columns.Add("PRECIO", typeof(string));

                                DataRow row = dt_Variantes_lista.NewRow();
                                row["ARTICULO"] = lblCodigo.Text;
                                row["CODIGO"] = cod_variacion;
                                row["CANTIDAD"] = cantidad;
                                row["PRECIO"] = precio;

                                dt_Variantes_lista.Rows.Add(row);

                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("No se puede escoger dos Opciones de Variantes iguales para un mismo Artículo", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MantenimientoVariantes_FormClosing(object sender, FormClosingEventArgs e)
        {
            lista_variantes.Clear();
        }
    }
}
