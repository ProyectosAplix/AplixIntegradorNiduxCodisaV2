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
    public partial class MantenimientoArticulos : Form
    {
        Metodos_Articulos mta = new Metodos_Articulos();

        public MantenimientoArticulos()
        {
            InitializeComponent();
        }

        private void MantenimientoArticulos_Load(object sender, EventArgs e)
        {
            CargarComboboxEstado();
        }

        //Llenado de los combobox estado de articulo
        public void CargarComboboxEstado()
        {
            DataTable dt = new DataTable();
            dt = mta.articulos_estados();

            if (dt == null)
            {
                MessageBox.Show("Error en Procedimiento de Obtener Estado de Artículos", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos de Estado de Artículos para mostrar", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        cbbEstado.Items.Add(dr["DESCRIPCION"].ToString());
                    }
                }
            }
            cbbEstado.SelectedIndex = 0;
        }

        private void btnAgregarArticulos_Click(object sender, EventArgs e)
        {
            AgregarArticulos agregarArticulos = new AgregarArticulos();
            agregarArticulos.ShowDialog();
            if (agregarArticulos.articulo != "")
            {
                txtCodigo.Text = agregarArticulos.articulo;
                txtCodigo.ReadOnly = true;
                activar_impuesto(agregarArticulos.articulo);
            }
        }

        private void txtCodigo_DoubleClick(object sender, EventArgs e)
        {
            AgregarArticulos agregarArticulos = new AgregarArticulos();
            agregarArticulos.ShowDialog();
            if (agregarArticulos.articulo != "")
            {
                txtCodigo.Text = agregarArticulos.articulo;
                txtCodigo.ReadOnly = true;
                activar_impuesto(agregarArticulos.articulo);
            }
        }

        private void dgvColecciones_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            MantenimientoColecciones colecciones = new MantenimientoColecciones();
            colecciones.ShowDialog();
            if (colecciones.dt_colecciones != null)
            {
                foreach (DataRow item in colecciones.dt_colecciones.Rows)
                {
                    int n = dgvColecciones.Rows.Add();

                    dgvColecciones.Rows[n].Cells[0].Value = item["ID"].ToString();
                    dgvColecciones.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();

                }
            }
        }

        private void dgvVariantes_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            MantenimientoVariantes variantes = new MantenimientoVariantes();
            variantes.ShowDialog();
            if (variantes.dt_Variantes_lista != null)
            {
                foreach (DataRow item in variantes.dt_Variantes_lista.Rows)
                {
                    int n = dgvVariantes.Rows.Add();

                    dgvVariantes.Rows[n].Cells[0].Value = item["ARTICULO"].ToString();
                    dgvVariantes.Rows[n].Cells[1].Value = item["CODIGO"].ToString();
                    dgvVariantes.Rows[n].Cells[2].Value = item["CANTIDAD"].ToString();
                    dgvVariantes.Rows[n].Cells[3].Value = item["PRECIO"].ToString();

                }
            }
        }

        private void btnGuardarArticulos_Click(object sender, EventArgs e)
        {
            if (txtCodigo.Text.Equals(""))
            {
                MessageBox.Show("No se ha seleccionado un Artículo aún", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //llenado de variables para el articulo
                string nombre = "";
                string descripcion = "";
                int estado = 0;
                string impuesto = "";

                //nombre
                if (txtNombre.Text != "" )
                {
                    nombre = txtNombre.Text;
                }
                //descripcion
                if (txtDescripcion.Text != "")
                {
                    descripcion = txtDescripcion.Text;
                }
                //estado
                if (cbbEstado.Text.Equals("Activo"))
                {
                    estado = 1;
                }
                else
                {
                    estado = 2;
                }
                //impuesto
                if (chbImpuesto.Checked == true)
                {
                    impuesto = "S";
                }
                else
                {
                    impuesto = "N";
                }

                //verificamos si el articulo va a tener variantes
                if (dgvVariantes.Rows.Count == 1)
                {
                    //sin variantes
                    string resultado_articulos = mta.actualizar_articulos(nombre, descripcion, estado, impuesto, txtCodigo.Text);
                    if (resultado_articulos == "exito")
                    {
                        //Verificamos si va a tener Colecciones el articulo
                        if (dgvColecciones.Rows.Count > 1)
                        {
                            //se ingresan las colecciones del producto
                            foreach (DataGridViewRow Datarow in dgvColecciones.Rows)
                            {
                                if (Datarow.Cells[0].Value != null && Datarow.Cells[1].Value != null)
                                {
                                    string coleccion = Datarow.Cells[0].Value.ToString();

                                    string resultado = mta.insertar_colecciones(coleccion, txtCodigo.Text);
                                    if (resultado.Equals("exito"))
                                    {

                                    }
                                    else
                                    {
                                        MessageBox.Show(resultado, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                }
                            }
                        }
                        MessageBox.Show("Artículo Actualizado con Éxito", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiarPantalla();
                    }
                    else
                    {
                        MessageBox.Show(resultado_articulos);
                    }
                }
                else
                {
                    //con variantes
                    string resultado_articulos = mta.actualizar_articulos(nombre, descripcion, estado, impuesto, txtCodigo.Text);
                    if (resultado_articulos == "exito")
                    {
                        //se ingresaron los datos basicos bien por lo cual se van agregar las variantes ahora
                        foreach (DataGridViewRow Datarow in dgvVariantes.Rows)
                        {
                            if (Datarow.Cells[0].Value != null && Datarow.Cells[1].Value != null)
                            {
                                string sku_variante = Datarow.Cells[0].Value.ToString();
                                string variantes = Datarow.Cells[1].Value.ToString();

                                string resultado = mta.insertar_variantes(sku_variante,txtCodigo.Text, variantes);
                                if (resultado.Equals("exito"))
                                {

                                }
                                else
                                {
                                    MessageBox.Show(resultado, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }
                        }

                        //Verificamos si va a tener Colecciones el articulo
                        if (dgvColecciones.Rows.Count > 1)
                        {
                            //se ingresan las colecciones del producto
                            foreach (DataGridViewRow Datarow in dgvColecciones.Rows)
                            {
                                if (Datarow.Cells[0].Value != null && Datarow.Cells[1].Value != null)
                                {
                                    string coleccion = Datarow.Cells[0].Value.ToString();

                                    string resultado = mta.insertar_colecciones(coleccion, txtCodigo.Text);
                                    if (resultado.Equals("exito"))
                                    {

                                    }
                                    else
                                    {
                                        MessageBox.Show(resultado, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                }
                            }
                        }

                        MessageBox.Show("Artículo Actualizado con Éxito", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiarPantalla();
                    }
                    else
                    {
                        MessageBox.Show(resultado_articulos);
                    }
                }
            }
        }

        public void limpiarPantalla()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();

            chbImpuesto.Checked = false;
            dgvColecciones.Rows.Clear();
            dgvVariantes.Rows.Clear();
        }

        public void activar_impuesto(string sku)
        {
            string impuesto = mta.articulo_impuesto(sku);
            if (impuesto.Equals("S"))
            {
                chbImpuesto.Checked = true;
            }
            else if (impuesto.Equals("N"))
            {
                chbImpuesto.Checked = false;
            }
            else
            {
                MessageBox.Show(impuesto, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
