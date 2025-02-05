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

namespace AplixEcommerceIntegration.Shopify
{
    public partial class MantenimientoEditarArticulos : Form
    {
        Metodos_Articulos metodos_articulos = new Metodos_Articulos();
        string codigo;

        public MantenimientoEditarArticulos(string sku)
        {
            InitializeComponent();
            codigo = sku;
        }

        private void MantenimientoEditarArticulos_Load(object sender, EventArgs e)
        {
            //llenado del combobox
            CargarComboboxEstado();
            //llenado de los campos txt
            txtCodigo.Text = codigo;
            CargaDatosBasicos();
            //tabla de colecion
            CargaColecciones();

            //tabla de variantes
            CargaVariantes();

        }

        public void CargarComboboxEstado()
        {
            DataTable dt = new DataTable();
            dt = metodos_articulos.articulos_estados();

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
        }

        public void CargaColecciones()
        {
            DataTable dt_colecciones = metodos_articulos.articulos_editados_colecciones(codigo);
            if (dt_colecciones == null)
            {
                MessageBox.Show("Error en Procedimiento de Obtener Colecciones", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (dt_colecciones.Rows.Count == 0)
                {
                    //MessageBox.Show("No hay datos de Colecciones para mostrar", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    foreach (DataRow item in dt_colecciones.Rows)
                    {
                        int n = dgvColecciones.Rows.Add();

                        dgvColecciones.Rows[n].Cells[0].Value = item["COLECCION"].ToString();
                        dgvColecciones.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                        if (item["ACTIVO"].ToString().Equals("S"))
                        {
                            dgvColecciones.Rows[n].Cells[2].Value = true;
                        }
                        else
                        {
                            dgvColecciones.Rows[n].Cells[2].Value = false;
                        }
                    }
                }
            }
        }

        public void CargaVariantes()
        {
            DataTable dt_variante = metodos_articulos.articulos_editados_variantes(codigo);
            if (dt_variante == null)
            {
                MessageBox.Show("Error en Procedimiento de Obtener Variantes", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (dt_variante.Rows.Count == 0)
                {
                    //MessageBox.Show("No hay datos de Variantes para mostrar", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    foreach (DataRow item in dt_variante.Rows)
                    {
                        int n = dgvVariantes.Rows.Add();

                        dgvVariantes.Rows[n].Cells[0].Value = item["ARTICULO"].ToString();
                        dgvVariantes.Rows[n].Cells[1].Value = item["VARIANTES"].ToString();
                        dgvVariantes.Rows[n].Cells[2].Value = item["CANTIDAD"].ToString().Remove(item["CANTIDAD"].ToString().Length -5);
                        dgvVariantes.Rows[n].Cells[3].Value = item["PRECIO"].ToString().Remove(item["PRECIO"].ToString().Length - 5);
                        if (item["ACTIVO"].ToString().Equals("S"))
                        {
                            dgvVariantes.Rows[n].Cells[4].Value = true;
                        }
                        else
                        {
                            dgvVariantes.Rows[n].Cells[4].Value = false;
                        }

                    }
                }
            }
        }

        public void CargaDatosBasicos()
        {
            DataTable dt_articulo = metodos_articulos.articulos_editados(codigo);
            if (dt_articulo == null)
            {
                MessageBox.Show("Error en Procedimiento de Obtener Datos del Artículo", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (dt_articulo.Rows.Count == 0)
                {
                    //MessageBox.Show("No hay datos de Variantes para mostrar", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    foreach (DataRow item in dt_articulo.Rows)
                    {

                        txtNombre.Text = item["NOMBRE_SHOPIFY"].ToString();
                        txtDescripcion.Text = item["DESCRIPCION_SHOPIFY"].ToString();

                        if (item["ESTADO"].ToString().Equals("1"))
                        {
                            //activo
                            cbbEstado.SelectedIndex = 0;
                        }
                        else
                        {
                            cbbEstado.SelectedIndex = 1;
                        }

                        if (item["IMPUESTO"].ToString().Equals("S"))
                        {
                            //activo
                            chbImpuesto.Checked = true;
                        }
                        else
                        {
                            chbImpuesto.Checked = false;
                        }

                    }
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
                if (txtNombre.Text != "")
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
                    //sin variantes, actualizamos a nivel de DB
                    string resultado_articulos = metodos_articulos.actualizar_articulos(nombre, descripcion, estado, impuesto, txtCodigo.Text);
                    if (resultado_articulos == "exito")
                    {
                        //Actualizamos a nivel de shopify los cambios
                        actualizar_articulos(txtCodigo.Text);

                        //Verificamos si va a tener Colecciones el articulo
                        if (dgvColecciones.Rows.Count > 1)
                        {
                            dgvColecciones.EndEdit();
                            //se ingresan las colecciones del producto
                            foreach (DataGridViewRow Datarow in dgvColecciones.Rows)
                            {
                                if (Datarow.Cells[0].Value != null && Datarow.Cells[1].Value != null)
                                {
                                    string coleccion = Datarow.Cells[0].Value.ToString();
                                    string activo = Datarow.Cells[2].Value.ToString();

                                    string resultado = metodos_articulos.actualizar_colecciones(coleccion, txtCodigo.Text, activo);
                                    if (resultado.Equals("exito"))
                                    {
                                        //actualizamos el id de la coleccion
                                    }
                                    else
                                    {
                                        MessageBox.Show(resultado, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }

                            //Ingresamos las nuevas Colecciones
                            actualizar_nuevas_colecciones();

                            //Eliminamos las colecciones
                            eliminar_coleccion(txtCodigo.Text);
                        }
                        MessageBox.Show("Artículo Actualizado con Éxito", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvColecciones.Rows.Clear();
                        CargaColecciones();
                        dgvVariantes.Rows.Clear();
                        CargaVariantes();
                    }
                    else
                    {
                        MessageBox.Show(resultado_articulos);
                    }
                }
                else
                {
                    //con variantes
                    string resultado_articulos = metodos_articulos.actualizar_articulos(nombre, descripcion, estado, impuesto, txtCodigo.Text);
                    if (resultado_articulos == "exito")
                    {
                        //se ingresaron los datos basicos bien por lo cual se van agregar las variantes ahora
                        foreach (DataGridViewRow Datarow in dgvVariantes.Rows)
                        {
                            dgvVariantes.EndEdit();
                            if (Datarow.Cells[0].Value != null && Datarow.Cells[1].Value != null)
                            {
                                string sku_variante = Datarow.Cells[0].Value.ToString();
                                string variantes = Datarow.Cells[1].Value.ToString();
                                string activo = Datarow.Cells[4].Value.ToString();

                                string activo_articulo = "";
                                if (activo.Equals("True"))
                                {
                                    activo_articulo = "S";
                                }
                                else
                                {
                                    activo_articulo = "N";
                                }


                                string resultado = metodos_articulos.actualizar_variantes(sku_variante, txtCodigo.Text, variantes, activo_articulo);
                                if (resultado.Equals("exito"))
                                {
                                    
                                }
                                else
                                {
                                    MessageBox.Show(resultado, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }
                        }

                        //Actualizamos a nivel de shopify los cambios, 
                        //pero ojo aquí nada más estamos sincronizando si han agregado una variante para el articulo cascaron
                        actualizar_articulos_variantes(txtCodigo.Text);

                        //Actualizamos a nivel de shopify las varaintes que se van a eliminar
                        //Borramos las variantes que ya no se van a sincronizar
                        borrar_variantes(txtCodigo.Text);

                        //Verificamos si va a tener Colecciones el articulo
                        if (dgvColecciones.Rows.Count > 1)
                        {
                            dgvColecciones.EndEdit();
                            //se ingresan las colecciones del producto
                            foreach (DataGridViewRow Datarow in dgvColecciones.Rows)
                            {
                                if (Datarow.Cells[0].Value != null && Datarow.Cells[1].Value != null)
                                {
                                    string coleccion = Datarow.Cells[0].Value.ToString();
                                    string activo = Datarow.Cells[2].Value.ToString();

                                    string resultado = metodos_articulos.actualizar_colecciones(coleccion, txtCodigo.Text, activo);
                                    if (resultado.Equals("exito"))
                                    {

                                    }
                                    else
                                    {
                                        MessageBox.Show(resultado, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                }
                            }

                            //Ingresamos las nuevas Colecciones
                            actualizar_nuevas_colecciones();

                            //Eliminamos las colecciones
                            eliminar_coleccion(txtCodigo.Text);
                        }
                        MessageBox.Show("Artículo Actualizado con Éxito", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvColecciones.Rows.Clear();
                        CargaColecciones();
                        dgvVariantes.Rows.Clear();
                        CargaVariantes();
                    }
                    else
                    {
                        MessageBox.Show(resultado_articulos);
                    }
                }
            }
        }

        private void dgvColecciones_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
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
                    dgvColecciones.Rows[n].Cells[2].Value = true;

                }
            }
        }

        private void dgvVariantes_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
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
                    dgvVariantes.Rows[n].Cells[4].Value = true;

                }
            }
        }

        public void actualizar_articulos(string sku)
        {
            string resultado_api_articulo = metodos_articulos.actualizar_articulos_sin_variantes(sku);
            if (resultado_api_articulo == "Ok")
            {

            }
            else if (resultado_api_articulo == "API")
            {
                //Error en el método de obtener el json en el api propio de actualizar el articulo
            }
            else
            {
                MessageBox.Show(resultado_api_articulo, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void actualizar_nuevas_colecciones()
        {
            string resultado = metodos_articulos.sincronizar_colecciones_articulos();
            if (resultado.Equals("Ok"))
            {
               
            }
            else if (resultado.Equals("lista"))
            {
                //MessageBox.Show("Colecciones no disponibles para sincronizar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (resultado.Equals("API"))
            {
                MessageBox.Show("Error en API, fallo detectado en el método de sincronizar artículos con variantes", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void eliminar_coleccion(string sku)
        {
            string resultado = metodos_articulos.eliminar_articulos_colecciones(sku);
            if (resultado.Equals("Ok"))
            {
                //
            }
            else
            {
                MessageBox.Show(resultado, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void actualizar_articulos_variantes(string sku)
        {
            string resultado_api_articulo = metodos_articulos.actualizar_articulos_con_variantes(sku);
            if (resultado_api_articulo == "Ok")
            {

            }
            else if (resultado_api_articulo == "API")
            {
                //Error en el método de obtener el json en el api propio de actualizar el articulo
            }
            else
            {
                MessageBox.Show(resultado_api_articulo, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void borrar_variantes(string sku)
        {
            string resultado = metodos_articulos.eliminar_variantes(sku);
            if (resultado.Equals("Ok"))
            {
                //
            }
            else
            {
                MessageBox.Show(resultado, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
    }
}
