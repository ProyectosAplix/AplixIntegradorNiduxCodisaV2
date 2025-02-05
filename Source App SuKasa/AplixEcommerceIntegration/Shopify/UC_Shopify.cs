using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AplixEcommerceIntegration.Shopify.Metodos;
using AplixEcommerceIntegration.Shopify.Clases;

namespace AplixEcommerceIntegration.Shopify
{
    public partial class UC_Shopify : UserControl
    {
        public UC_Shopify()
        {
            InitializeComponent();
        }

        /*------------------------- INSTANCIAS DE METODOS --------------------------*/

        Metodos_Articulos mtd = new Metodos_Articulos();
        Metodos_Opciones mto = new Metodos_Opciones();
        Metodos_Colecciones mtc = new Metodos_Colecciones();
        Metodos_Pedidos mtp = new Metodos_Pedidos();

        /*------------------------- VARIABLES --------------------------*/

        /*---------- ARTICULOS ---------*/
        static List<Articulos> lista_articulos = new List<Articulos>();
        DataTable dt_Articulos;

        /*---------- VARIANTES ---------*/
        static List<Variantes> lista_variantes = new List<Variantes>();
        DataTable dt_Variante;

        /*---------- VARIANTES ---------*/
        static List<Coleccion> lista_coleccion = new List<Coleccion>();
        DataTable dt_Coleccion;

        /*---------- PEDIDOS ---------*/
        DataTable dt_Pedidos;

        /*------------------------- ARTICULOS --------------------------*/

        public void CargaArticulos()
        {
            dt_Articulos = mtd.articulos();

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
                        dgvArticulos.Rows[n].Cells[2].Value = item["DESCRIPCION"].ToString();
                        dgvArticulos.Rows[n].Cells[3].Value = string.Format("{0:0.00}", item["PESO"]);
                        dgvArticulos.Rows[n].Cells[4].Value = string.Format("{0:0.00}", item["CANTIDAD"]);
                        dgvArticulos.Rows[n].Cells[5].Value = string.Format("{0:0.00}", item["PRECIO"]);
                        dgvArticulos.Rows[n].Cells[6].Value = string.Format("{0:0.00}", item["DESCUENTO"]);
                        dgvArticulos.Rows[n].Cells[7].Value = item["IMPUESTO"].ToString();
                        dgvArticulos.Rows[n].Cells[8].Value = item["ESTADO"].ToString();
                        dgvArticulos.Rows[n].Cells[9].Value = item["ACTIVO"].ToString();
                        dgvArticulos.Rows[n].Cells[10].Value = item["ID_SHOPIFY"].ToString();

                    }

                    //Activacion de checkbox
                    foreach (DataGridViewRow row in dgvArticulos.Rows)
                    {
                        //Campo Impuesto
                        DataGridViewCheckBoxCell comboBoxImpuesto = (row.Cells[7] as DataGridViewCheckBoxCell);
                        if (row.Cells[7].Value.ToString() == "S")
                        {
                            comboBoxImpuesto.Value = true;
                        }
                        else
                        {
                            comboBoxImpuesto.Value = false;
                        }
                        //Campo Activo
                        DataGridViewCheckBoxCell comboBoxCell = (row.Cells[9] as DataGridViewCheckBoxCell);
                        if (row.Cells[9].Value.ToString() == "S")
                        {
                            comboBoxCell.Value = true;
                        }
                        else
                        {
                            comboBoxCell.Value = false;
                        }
                    }

                    //Campo estado
                    //DataTable dt_estado = new DataTable();
                    //dt_estado = mtd.articulos_estados();
                    //foreach (DataGridViewRow row in dgvArticulos.Rows)
                    //{
                    //    DataGridViewComboBoxCell comboBoxCell = (row.Cells[8] as DataGridViewComboBoxCell);
                    //    foreach (DataRow drow in dt_estado.Rows)
                    //    {
                    //        string id = drow[0].ToString();
                    //        comboBoxCell.Items.Add(drow[1]);
                    //        if (row.Cells[8].Value.ToString() == id)
                    //        {
                    //            comboBoxCell.Value = drow[1];
                    //        }
                    //    }
                    //}
                }
            }
        }

        public void BuscarArticulos(string articulo)
        {
            if (dt_Articulos != null)
            {
                dgvArticulos.Rows.Clear();
                if (cbbArticulos.Text.Equals("Código"))
                {
                    DataRow[] resultado = dt_Articulos.Select("CONVERT(SKU, System.String) LIKE '%" + articulo + "%'");
                    foreach (DataRow item in resultado)
                    {
                        int n = dgvArticulos.Rows.Add();

                        Articulos product = lista_articulos.FirstOrDefault(x => x.sku == item["SKU"].ToString());

                        if (product == null)
                        {
                            dgvArticulos.Rows[n].Cells[0].Value = item["SKU"].ToString();
                            dgvArticulos.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                            dgvArticulos.Rows[n].Cells[2].Value = item["DESCRIPCION"].ToString();
                            dgvArticulos.Rows[n].Cells[3].Value = string.Format("{0:0.00}", item["PESO"]);
                            dgvArticulos.Rows[n].Cells[4].Value = string.Format("{0:0.00}", item["CANTIDAD"]);
                            dgvArticulos.Rows[n].Cells[5].Value = string.Format("{0:0.00}", item["PRECIO"]);
                            dgvArticulos.Rows[n].Cells[6].Value = string.Format("{0:0.00}", item["DESCUENTO"]);
                            dgvArticulos.Rows[n].Cells[7].Value = item["IMPUESTO"].ToString();
                            dgvArticulos.Rows[n].Cells[8].Value = item["ESTADO"].ToString();
                            dgvArticulos.Rows[n].Cells[9].Value = item["ACTIVO"].ToString();
                            dgvArticulos.Rows[n].Cells[10].Value = item["ID_SHOPIFY"].ToString();
                        }
                        else
                        {
                            dgvArticulos.Rows[n].Cells[0].Value = product.sku;
                            dgvArticulos.Rows[n].Cells[1].Value = product.nombre;
                            dgvArticulos.Rows[n].Cells[2].Value = product.descripcion;
                            dgvArticulos.Rows[n].Cells[3].Value = product.peso;
                            dgvArticulos.Rows[n].Cells[4].Value = product.cantidad;
                            dgvArticulos.Rows[n].Cells[5].Value = product.precio;
                            dgvArticulos.Rows[n].Cells[6].Value = product.descuento;
                            if (product.impuesto.Equals("True"))
                            {
                                dgvArticulos.Rows[n].Cells[7].Value = "S";
                            }
                            else
                            {
                                dgvArticulos.Rows[n].Cells[7].Value = "N";
                            }
                            dgvArticulos.Rows[n].Cells[8].Value = product.estado;
                            if (product.sincronizar.Equals("True"))
                            {
                                dgvArticulos.Rows[n].Cells[9].Value = "S";
                            }
                            else
                            {
                                dgvArticulos.Rows[n].Cells[9].Value = "N";
                            }
                            dgvArticulos.Rows[n].Cells[10].Value = product.id_shopify;
                        }

                    }

                    //Activacion de checkbox
                    foreach (DataGridViewRow row in dgvArticulos.Rows)
                    {
                        //Campo Impuesto
                        DataGridViewCheckBoxCell comboBoxImpuesto = (row.Cells[7] as DataGridViewCheckBoxCell);
                        if (row.Cells[7].Value.ToString() == "S")
                        {
                            comboBoxImpuesto.Value = true;
                        }
                        else
                        {
                            comboBoxImpuesto.Value = false;
                        }
                        //Campo Activo
                        DataGridViewCheckBoxCell comboBoxCell = (row.Cells[9] as DataGridViewCheckBoxCell);
                        if (row.Cells[9].Value.ToString() == "S")
                        {
                            comboBoxCell.Value = true;
                        }
                        else
                        {
                            comboBoxCell.Value = false;
                        }
                    }
                }
                else
                {
                    DataRow[] resultado = dt_Articulos.Select("CONVERT(NOMBRE, System.String) LIKE '%" + articulo + "%'");
                    foreach (DataRow item in resultado)
                    {
                        int n = dgvArticulos.Rows.Add();
                        Articulos product = lista_articulos.FirstOrDefault(x => x.sku == item["SKU"].ToString());

                        if (product == null)
                        {
                            dgvArticulos.Rows[n].Cells[0].Value = item["SKU"].ToString();
                            dgvArticulos.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                            dgvArticulos.Rows[n].Cells[2].Value = item["DESCRIPCION"].ToString();
                            dgvArticulos.Rows[n].Cells[3].Value = string.Format("{0:0.00}", item["PESO"]);
                            dgvArticulos.Rows[n].Cells[4].Value = string.Format("{0:0.00}", item["CANTIDAD"]);
                            dgvArticulos.Rows[n].Cells[5].Value = string.Format("{0:0.00}", item["PRECIO"]);
                            dgvArticulos.Rows[n].Cells[6].Value = string.Format("{0:0.00}", item["DESCUENTO"]);
                            dgvArticulos.Rows[n].Cells[7].Value = item["IMPUESTO"].ToString();
                            dgvArticulos.Rows[n].Cells[8].Value = item["ESTADO"].ToString();
                            dgvArticulos.Rows[n].Cells[9].Value = item["ACTIVO"].ToString();
                            dgvArticulos.Rows[n].Cells[10].Value = item["ID_SHOPIFY"].ToString();
                        }
                        else
                        {
                            dgvArticulos.Rows[n].Cells[0].Value = product.sku;
                            dgvArticulos.Rows[n].Cells[1].Value = product.nombre;
                            dgvArticulos.Rows[n].Cells[2].Value = product.descripcion;
                            dgvArticulos.Rows[n].Cells[3].Value = product.peso;
                            dgvArticulos.Rows[n].Cells[4].Value = product.cantidad;
                            dgvArticulos.Rows[n].Cells[5].Value = product.precio;
                            dgvArticulos.Rows[n].Cells[6].Value = product.descuento;
                            if (product.impuesto.Equals("True"))
                            {
                                dgvArticulos.Rows[n].Cells[7].Value = "S";
                            }
                            else
                            {
                                dgvArticulos.Rows[n].Cells[7].Value = "N";
                            }
                            dgvArticulos.Rows[n].Cells[8].Value = product.estado;
                            if (product.sincronizar.Equals("True"))
                            {
                                dgvArticulos.Rows[n].Cells[9].Value = "S";
                            }
                            else
                            {
                                dgvArticulos.Rows[n].Cells[9].Value = "N";
                            }
                            dgvArticulos.Rows[n].Cells[10].Value = product.id_shopify;
                        }
                    }

                    //Activacion de checkbox
                    foreach (DataGridViewRow row in dgvArticulos.Rows)
                    {
                        //Campo Impuesto
                        DataGridViewCheckBoxCell comboBoxImpuesto = (row.Cells[7] as DataGridViewCheckBoxCell);
                        if (row.Cells[7].Value.ToString() == "S")
                        {
                            comboBoxImpuesto.Value = true;
                        }
                        else
                        {
                            comboBoxImpuesto.Value = false;
                        }
                        //Campo Activo
                        DataGridViewCheckBoxCell comboBoxCell = (row.Cells[9] as DataGridViewCheckBoxCell);
                        if (row.Cells[9].Value.ToString() == "S")
                        {
                            comboBoxCell.Value = true;
                        }
                        else
                        {
                            comboBoxCell.Value = false;
                        }
                    }
                }
            }
        }

        private void btnAgregarArticulos_Click(object sender, EventArgs e)
        {
            MantenimientoArticulos mantenimiento = new MantenimientoArticulos();
            mantenimiento.ShowDialog();
        }

        private void btnActualizarArticulos_Click(object sender, EventArgs e)
        {
            dgvArticulos.Rows.Clear();
            CargaArticulos();
        }

        private void btnSincronizarArticulos_Click(object sender, EventArgs e)
        {
            int n = 0;
            //actualizamos primero los articulos sin variantes
            string resultado = mtd.sincronizar_articulos_sin_variantes();
            if (resultado.Equals("Ok"))
            {
                n = n + 1;
            }
            else if (resultado.Equals("lista"))
            {
                //MessageBox.Show("Artículos sin variantes no disponibles para sincronizar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                n = n + 1;
            }
            else if (resultado.Equals("API"))
            {
                MessageBox.Show("Error en API, fallo detectado en el método de sincronizar artículos sin variantes", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //
            //actualizamos segundo los articulos con variantes
            string resultado2 = mtd.sincronizar_articulos_con_variantes();
            if (resultado2.Equals("Ok"))
            {
                n = n + 1;
            }
            else if (resultado2.Equals("lista"))
            {
                //MessageBox.Show("Artículos con variantes no disponibles para sincronizar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                n = n + 1;
            }
            else if (resultado2.Equals("API"))
            {
                MessageBox.Show("Error en API, fallo detectado en el método de sincronizar artículos con variantes", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado2, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //tercero actualizamos las colecciones
            string resultado3 = mtd.sincronizar_colecciones_articulos();
            if (resultado3.Equals("Ok"))
            {
                n = n + 1;
            }
            else if (resultado3.Equals("lista"))
            {
                //MessageBox.Show("Colecciones no disponibles para sincronizar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                n = n + 1;
            }
            else if (resultado3.Equals("API"))
            {
                MessageBox.Show("Error en API, fallo detectado en el método de sincronizar artículos con variantes", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado3, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //actualizamos la fecha
            if (n > 0)
            {
                mtd.actualizar_fecha();
                MessageBox.Show("Artículos sincronizados con éxito", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgvArticulos_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string sku = dgvArticulos.SelectedCells[0].Value.ToString();
                MantenimientoEditarArticulos mantenimiento = new MantenimientoEditarArticulos(sku);
                mantenimiento.ShowDialog();
            }
        }

        private void txtBuscarArticulos_TextChanged(object sender, EventArgs e)
        {
            BuscarArticulos(txtBuscarArticulos.Text);
        }

        private void btnObtenerArticulos_Click(object sender, EventArgs e)
        {
            string resultado = mtd.obtener_articulos_tienda();
            if (resultado.Equals("ok"))
            {
                MessageBox.Show("Artículos sincronizados con éxito", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: " + resultado, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarArticulos_Click(object sender, EventArgs e)
        {
            try
            {
                if (lista_articulos.Count > 0)
                {
                    string codigos = "";
                    string id = "";
                    string final = "";
                    for (int i = 0; i < lista_articulos.Count; i++)
                    {
                        if (lista_articulos[i].sincronizar.Equals("False"))
                        {
                            codigos = lista_articulos[i].sku + "," + codigos;
                            id  = lista_articulos[i].id_shopify + "," + id;
                        }
                    }

                    if (codigos == "")
                    {
                        MessageBox.Show("No se ha seleccionada ningun Artículo para Eliminar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        codigos = codigos.Remove(codigos.Length - 1);
                        id = id.Remove(id.Length - 1);
                        DialogResult result =
                        MessageBox.Show("¿Está seguro que desea Eliminar Artículos?", "Mensaje de Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            string[] codigos_opciones = codigos.Split(',');
                            string[] codigos_id = id.Split(',');
                            for (int i = 0; i < codigos_id.Length; i++)
                            {
                                if (codigos_id[i].Equals(""))
                                {
                                    string resultados = mtd.eliminar_articulos(codigos_opciones[i]);
                                    if (resultados.Equals("exito"))
                                    {
                                        final = codigos_opciones[i] + "," + final;
                                    }
                                }
                                else
                                {
                                    string resultado = mtd.eliminar_articulos_tienda(codigos_id[i]);
                                    if (resultado == "ok")
                                    {
                                        string resultados = mtd.eliminar_articulos(codigos_opciones[i]);
                                        if (resultados.Equals("exito"))
                                        {
                                            final = codigos_opciones[i] + "," + final;
                                        }
                                    }
                                }
                            }
                            final = final.Remove(final.Length - 1);
                            MessageBox.Show("Artículos Elimandos con éxito: " + final, "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lista_articulos.Clear();
                            dgvArticulos.Rows.Clear();
                            CargaArticulos();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No se ha seleccionada ningúna opción de valor para Eliminar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 9)
            {
                dgvArticulos.EndEdit();
                Articulos articulos = new Articulos();

                articulos.sku = this.dgvArticulos.CurrentRow.Cells[0].Value.ToString();
                articulos.nombre = this.dgvArticulos.CurrentRow.Cells[1].Value.ToString();
                articulos.descripcion = this.dgvArticulos.CurrentRow.Cells[2].Value.ToString();
                articulos.peso = this.dgvArticulos.CurrentRow.Cells[3].Value.ToString();
                articulos.cantidad = this.dgvArticulos.CurrentRow.Cells[4].Value.ToString();
                articulos.precio = this.dgvArticulos.CurrentRow.Cells[5].Value.ToString();
                articulos.descuento = this.dgvArticulos.CurrentRow.Cells[6].Value.ToString();
                articulos.impuesto = this.dgvArticulos.CurrentRow.Cells[7].Value.ToString();
                articulos.estado = this.dgvArticulos.CurrentRow.Cells[8].Value.ToString();
                articulos.sincronizar = this.dgvArticulos.CurrentRow.Cells[9].Value.ToString();
                articulos.id_shopify = this.dgvArticulos.CurrentRow.Cells[10].Value.ToString();


                Articulos product = lista_articulos.FirstOrDefault(x => x.sku == this.dgvArticulos.CurrentRow.Cells[0].Value.ToString());

                if (product == null)
                {
                    lista_articulos.Add(articulos);
                }
                else
                {
                    lista_articulos.RemoveAll(x => x.sku == (articulos.sku));
                    lista_articulos.Add(articulos);
                }
                dgvArticulos.EndEdit();
            }
        }

        /*------------------------- VARIANTES ARTICULOS --------------------------*/

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
                        int n = dgvOpcionesArticulos.Rows.Add();

                        dgvOpcionesArticulos.Rows[n].Cells[0].Value = item["OPCION"].ToString();
                        dgvOpcionesArticulos.Rows[n].Cells[1].Value = item["DESCRIPCION"].ToString();
                        dgvOpcionesArticulos.Rows[n].Cells[2].Value = item["CODIGO"].ToString();
                        dgvOpcionesArticulos.Rows[n].Cells[3].Value = false;

                    }
                }
            }
        }

        private void btnActualizarVariantes_Click(object sender, EventArgs e)
        {
            dgvOpcionesArticulos.Rows.Clear();
            CargaOpcionesArticulos();
        }

        private void btnAgregarVariantes_Click(object sender, EventArgs e)
        {
            AgregarVariantes agregarVariantes = new AgregarVariantes();
            agregarVariantes.ShowDialog();
            if (agregarVariantes.indicador == 1)
            {
                dgvOpcionesArticulos.Rows.Clear();
                CargaOpcionesArticulos();
            }
        }

        private void btnGuardarVariantes_Click(object sender, EventArgs e)
        {
            try
            {
                if (lista_variantes.Count > 0)
                {
                    foreach (Variantes item in lista_variantes)
                    {
                        int respuesta = mto.actualizar_opciones_valores(item.descripcion, Convert.ToInt32(item.codigo));
                        if (respuesta == 0)
                        {
                            MessageBox.Show("La opción de valor, " + item.descripcion + " no se guardó correctamente", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    MessageBox.Show("Opciones de Valores guardadas con éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se ha editado ningúna opción de valor", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                lista_variantes.Clear();
                dgvOpcionesArticulos.Rows.Clear();
                CargaOpcionesArticulos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarVariantes_Click(object sender, EventArgs e)
        {
            try
            {
                if (lista_variantes.Count > 0)
                {
                    string codigos = "";
                    string nombre = "";
                    for (int i = 0; i < lista_variantes.Count; i++)
                    {
                        if (lista_variantes[i].elimanado.Equals("True"))
                        {
                            codigos = lista_variantes[i].codigo + "," + codigos;
                            nombre = lista_variantes[i].descripcion + "," + nombre;
                        }
                    }

                    if (codigos == "")
                    {
                        MessageBox.Show("No se ha seleccionada ningúna opción de valor para Eliminar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        codigos = codigos.Remove(codigos.Length - 1);
                        nombre = nombre.Remove(nombre.Length - 1);
                        DialogResult result =
                        MessageBox.Show("¿Está seguro que desea Eliminar Valores de opciones? Los Valores que se van a Eliminar son: " + nombre, "Mensaje de Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            string[] codigos_opciones = codigos.Split(',');
                            foreach (string item in codigos_opciones)
                            {
                                mto.eliminar_opciones_valores(item);
                            }
                            lista_variantes.Clear();
                            dgvOpcionesArticulos.Rows.Clear();
                            CargaOpcionesArticulos();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No se ha seleccionada ningúna opción de valor para Eliminar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvOpcionesArticulos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Variantes variantes = new Variantes();

            variantes.opcion = this.dgvOpcionesArticulos.CurrentRow.Cells[0].Value.ToString();
            variantes.descripcion = this.dgvOpcionesArticulos.CurrentRow.Cells[1].Value.ToString();
            variantes.codigo = this.dgvOpcionesArticulos.CurrentRow.Cells[2].Value.ToString();
            variantes.elimanado = this.dgvOpcionesArticulos.CurrentRow.Cells[3].Value.ToString();

            Variantes product = lista_variantes.FirstOrDefault(x => x.codigo == this.dgvOpcionesArticulos.CurrentRow.Cells[2].Value.ToString());

            if (product == null)
            {
                lista_variantes.Add(variantes);
            }
            else
            {
                lista_variantes.RemoveAll(x => x.codigo == (variantes.codigo));
                lista_variantes.Add(variantes);
            }
        }

        private void dgvOpcionesArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                dgvOpcionesArticulos.EndEdit();
                Variantes variantes = new Variantes();

                variantes.opcion = this.dgvOpcionesArticulos.CurrentRow.Cells[0].Value.ToString();
                variantes.descripcion = this.dgvOpcionesArticulos.CurrentRow.Cells[1].Value.ToString();
                variantes.codigo = this.dgvOpcionesArticulos.CurrentRow.Cells[2].Value.ToString();
                variantes.elimanado = this.dgvOpcionesArticulos.CurrentRow.Cells[3].Value.ToString();

                Variantes product = lista_variantes.FirstOrDefault(x => x.codigo == this.dgvOpcionesArticulos.CurrentRow.Cells[2].Value.ToString());

                if (product == null)
                {
                    lista_variantes.Add(variantes);
                }
                else
                {
                    lista_variantes.RemoveAll(x => x.codigo == (variantes.codigo));
                    lista_variantes.Add(variantes);
                }
                dgvOpcionesArticulos.EndEdit();
            }
        }

        public void Buscar_Variantes(string variante)
        {
            dgvOpcionesArticulos.Rows.Clear();
            if (cbbOpcionesValores.Text.Equals("Código"))
            {
                DataRow[] resultado = dt_Variante.Select("CONVERT(CODIGO, System.String) LIKE '%" + variante + "%'");
                foreach (DataRow item in resultado)
                {
                    int n = dgvOpcionesArticulos.Rows.Add();

                    Variantes product = lista_variantes.FirstOrDefault(x => x.codigo == item["CODIGO"].ToString());

                    if (product == null)
                    {
                        dgvOpcionesArticulos.Rows[n].Cells[0].Value = item["OPCION"].ToString();
                        dgvOpcionesArticulos.Rows[n].Cells[1].Value = item["DESCRIPCION"].ToString();
                        dgvOpcionesArticulos.Rows[n].Cells[2].Value = item["CODIGO"].ToString();
                        dgvOpcionesArticulos.Rows[n].Cells[3].Value = false;
                    }
                    else
                    {
                        dgvOpcionesArticulos.Rows[n].Cells[0].Value = product.opcion;
                        dgvOpcionesArticulos.Rows[n].Cells[1].Value = product.descripcion;
                        dgvOpcionesArticulos.Rows[n].Cells[2].Value = product.codigo;
                        dgvOpcionesArticulos.Rows[n].Cells[3].Value = product.elimanado;
                    }
                }
            }
            else
            {
                DataRow[] resultado = dt_Variante.Select("CONVERT(DESCRIPCION, System.String) LIKE '%" + variante + "%'");
                foreach (DataRow item in resultado)
                {
                    int n = dgvOpcionesArticulos.Rows.Add();

                    Variantes product = lista_variantes.FirstOrDefault(x => x.codigo == item["CODIGO"].ToString());

                    if (product == null)
                    {
                        dgvOpcionesArticulos.Rows[n].Cells[0].Value = item["OPCION"].ToString();
                        dgvOpcionesArticulos.Rows[n].Cells[1].Value = item["DESCRIPCION"].ToString();
                        dgvOpcionesArticulos.Rows[n].Cells[2].Value = item["CODIGO"].ToString();
                        dgvOpcionesArticulos.Rows[n].Cells[3].Value = false;
                    }
                    else
                    {
                        dgvOpcionesArticulos.Rows[n].Cells[0].Value = product.opcion;
                        dgvOpcionesArticulos.Rows[n].Cells[1].Value = product.descripcion;
                        dgvOpcionesArticulos.Rows[n].Cells[2].Value = product.codigo;
                        dgvOpcionesArticulos.Rows[n].Cells[3].Value = product.elimanado;
                    }
                }
            }
        }

        private void txtOpcionesVariantes_TextChanged(object sender, EventArgs e)
        {
            Buscar_Variantes(txtOpcionesVariantes.Text.ToString());
        }

        /*------------------------- COLECCIONES ARTICULOS --------------------------*/

        public void CargaColecciones()
        {
            dt_Coleccion = mtc.mostar_colecciones();

            if (dt_Coleccion == null)
            {
                MessageBox.Show("Error en Procedimiento de Obtener Colecciones", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (dt_Coleccion.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos de Colecciones para mostrar", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //datos normales de variantes
                    foreach (DataRow item in dt_Coleccion.Rows)
                    {
                        int n = dgvColecciones.Rows.Add();

                        dgvColecciones.Rows[n].Cells[0].Value = item["ID"].ToString();
                        dgvColecciones.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                        dgvColecciones.Rows[n].Cells[2].Value = item["DESCRIPCION"].ToString();
                        dgvColecciones.Rows[n].Cells[3].Value = false;

                    }
                }
            }
        }

        private void btnAgregarColeccion_Click(object sender, EventArgs e)
        {
            AgregarColecciones colecciones = new AgregarColecciones();
            colecciones.ShowDialog();
            if (colecciones.indicador == 1)
            {
                dgvColecciones.Rows.Clear();
                CargaColecciones();
            }
        }

        private void btnGuardarColeccion_Click(object sender, EventArgs e)
        {
            try
            {
                if (lista_coleccion.Count > 0)
                {
                    foreach (Coleccion item in lista_coleccion)
                    {
                        int respuesta = mtc.guardar_colecciones(item.id, item.descripcion);
                        if (respuesta == 0)
                        {
                            MessageBox.Show("La Colección, " + item.descripcion + " no se guardó correctamente", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    MessageBox.Show("Colecciones guardadas con éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se ha editado ningúna Colección", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                lista_coleccion.Clear();
                dgvColecciones.Rows.Clear();
                CargaColecciones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSincronizarColecciones_Click(object sender, EventArgs e)
        {
            string resultado = mtc.actualizar_colecciones();
            if (resultado.Equals("Ok"))
            {
                MessageBox.Show("Colecciones Sincronizadas con éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (resultado.Equals("lista"))
            {
                MessageBox.Show("No hay Colecciones para sincronizar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (resultado.Equals("API"))
            {
                MessageBox.Show("Error en API, fallo detectado en el método de obtener colecciones", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizarColecciones_Click(object sender, EventArgs e)
        {
            dgvColecciones.Rows.Clear();
            CargaColecciones();
        }

        private void btnObtenerColecciones_Click(object sender, EventArgs e)
        {
            int resultado = mtc.obtener_colecciones();
            if (resultado == 1)
            {
                MessageBox.Show("Colecciones agregadas con éxito", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvColecciones.Rows.Clear();
                CargaColecciones();
            }
            else if (resultado == 3)
            {
                MessageBox.Show("Error a la hora de obtener los credenciales de Conexión", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (resultado == 2)
            {
                MessageBox.Show("Error a la hora de obtener las Colecciones de Shopify", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Error en procedimientos", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminarColecciones_Click(object sender, EventArgs e)
        {
            try
            {
                if (lista_coleccion.Count > 0)
                {
                    string codigos = "";
                    string nombre = "";
                    for (int i = 0; i < lista_coleccion.Count; i++)
                    {
                        if (lista_coleccion[i].activo.Equals("True"))
                        {
                            codigos = lista_coleccion[i].id + "," + codigos;
                            nombre = lista_coleccion[i].nombre + "," + nombre;
                        }
                    }

                    if (codigos == "")
                    {
                        MessageBox.Show("No se ha seleccionada ningúna Colección para Eliminar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        codigos = codigos.Remove(codigos.Length - 1);
                        nombre = nombre.Remove(nombre.Length - 1);
                        DialogResult result =
                        MessageBox.Show("¿Está seguro que desea Eliminar Colecciones? Las Colecciones que se van a Eliminar son: " + nombre, "Mensaje de Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            string[] codigos_opciones = codigos.Split(',');
                            foreach (string item in codigos_opciones)
                            {
                                mtc.eliminar_colecciones(item);
                            }
                            lista_coleccion.Clear();
                            dgvColecciones.Rows.Clear();
                            CargaColecciones();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No se ha seleccionada ningúna Colección para Eliminar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Buscar_Coleccion(string coleccion)
        {
            dgvColecciones.Rows.Clear();
            if (cbbColeccion.Text.Equals("Código"))
            {
                DataRow[] resultado = dt_Coleccion.Select("CONVERT(ID, System.String) LIKE '%" + coleccion + "%'");
                foreach (DataRow item in resultado)
                {
                    int n = dgvColecciones.Rows.Add();

                    Coleccion product = lista_coleccion.FirstOrDefault(x => x.id == item["ID"].ToString());

                    if (product == null)
                    {
                        dgvColecciones.Rows[n].Cells[0].Value = item["ID"].ToString();
                        dgvColecciones.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                        dgvColecciones.Rows[n].Cells[2].Value = item["DESCRIPCION"].ToString();
                        dgvColecciones.Rows[n].Cells[3].Value = false;
                    }
                    else
                    {
                        dgvColecciones.Rows[n].Cells[0].Value = product.id;
                        dgvColecciones.Rows[n].Cells[1].Value = product.nombre;
                        dgvColecciones.Rows[n].Cells[2].Value = product.descripcion;
                        dgvColecciones.Rows[n].Cells[3].Value = product.activo;
                    }
                }
            }
            else
            {
                DataRow[] resultado = dt_Coleccion.Select("CONVERT(NOMBRE, System.String) LIKE '%" + coleccion + "%'");
                foreach (DataRow item in resultado)
                {
                    int n = dgvColecciones.Rows.Add();

                    Coleccion product = lista_coleccion.FirstOrDefault(x => x.id == item["ID"].ToString());

                    if (product == null)
                    {
                        dgvColecciones.Rows[n].Cells[0].Value = item["ID"].ToString();
                        dgvColecciones.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                        dgvColecciones.Rows[n].Cells[2].Value = item["DESCRIPCION"].ToString();
                        dgvColecciones.Rows[n].Cells[3].Value = false;
                    }
                    else
                    {
                        dgvColecciones.Rows[n].Cells[0].Value = product.id;
                        dgvColecciones.Rows[n].Cells[1].Value = product.nombre;
                        dgvColecciones.Rows[n].Cells[2].Value = product.descripcion;
                        dgvColecciones.Rows[n].Cells[3].Value = product.activo;
                    }

                }
            }
        }

        private void txtBuscarColeccion_TextChanged(object sender, EventArgs e)
        {
            Buscar_Coleccion(txtBuscarColeccion.Text);
        }

        private void dgvColecciones_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Coleccion coleccion  = new Coleccion();

            coleccion.id = this.dgvColecciones.CurrentRow.Cells[0].Value.ToString();
            coleccion.nombre = this.dgvColecciones.CurrentRow.Cells[1].Value.ToString();
            coleccion.descripcion = this.dgvColecciones.CurrentRow.Cells[2].Value.ToString();
            coleccion.activo = this.dgvColecciones.CurrentRow.Cells[3].Value.ToString();

            Coleccion product = lista_coleccion.FirstOrDefault(x => x.id == this.dgvColecciones.CurrentRow.Cells[0].Value.ToString());

            if (product == null)
            {
                lista_coleccion.Add(coleccion);
            }
            else
            {
                lista_coleccion.RemoveAll(x => x.id == (coleccion.id));
                lista_coleccion.Add(coleccion);
            }
        }

        private void dgvColecciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                dgvColecciones.EndEdit();
                Coleccion coleccion = new Coleccion();

                coleccion.id = this.dgvColecciones.CurrentRow.Cells[0].Value.ToString();
                coleccion.nombre = this.dgvColecciones.CurrentRow.Cells[1].Value.ToString();
                coleccion.descripcion = this.dgvColecciones.CurrentRow.Cells[2].Value.ToString();
                coleccion.activo = this.dgvColecciones.CurrentRow.Cells[3].Value.ToString();

                Coleccion product = lista_coleccion.FirstOrDefault(x => x.id == this.dgvColecciones.CurrentRow.Cells[0].Value.ToString());

                if (product == null)
                {
                    lista_coleccion.Add(coleccion);
                }
                else
                {
                    lista_coleccion.RemoveAll(x => x.id == (coleccion.id));
                    lista_coleccion.Add(coleccion);
                }
                dgvColecciones.EndEdit();
            }
        }

        /*------------------------- PEDIDOS ARTICULOS --------------------------*/

        public void CargaPedidos()
        {
            dt_Pedidos = mtp.pedidos();

            if (dt_Pedidos == null)
            {
                MessageBox.Show("Error en Procedimiento de Obtener Pedidos", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (dt_Pedidos.Rows.Count == 0)
                {
                    MessageBox.Show("No hay Pedidos para mostrar", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //datos normales de articulos
                    foreach (DataRow item in dt_Pedidos.Rows)
                    {
                        int n = dgvPedidos.Rows.Add();

                        dgvPedidos.Rows[n].Cells[0].Value = item["ID"].ToString();
                        dgvPedidos.Rows[n].Cells[1].Value = item["ORDER_NUMBER"].ToString();
                        dgvPedidos.Rows[n].Cells[2].Value = item["CONSECUTIVO"].ToString();
                        dgvPedidos.Rows[n].Cells[3].Value = item["BROWSER_IP"].ToString();
                        dgvPedidos.Rows[n].Cells[4].Value = item["CANCEL_REASON"].ToString();
                        dgvPedidos.Rows[n].Cells[5].Value = item["CANCELLED_AT"].ToString();
                        dgvPedidos.Rows[n].Cells[6].Value = item["CONFIRMED"].ToString();
                        dgvPedidos.Rows[n].Cells[7].Value = item["CONTACT_EMAIL"].ToString();
                        dgvPedidos.Rows[n].Cells[8].Value = item["CREATED_AT"].ToString();
                        dgvPedidos.Rows[n].Cells[9].Value = item["CURRENCY"].ToString();
                        dgvPedidos.Rows[n].Cells[10].Value = item["CUSTOMER_LOCALE"].ToString();
                        dgvPedidos.Rows[n].Cells[11].Value = item["EMAIL"].ToString();
                        dgvPedidos.Rows[n].Cells[12].Value = item["FINANCIAL_STATUS"].ToString();
                        dgvPedidos.Rows[n].Cells[13].Value = item["FULFILLMENT_STATUS"].ToString();
                        dgvPedidos.Rows[n].Cells[14].Value = item["GATEWAY"].ToString();
                        dgvPedidos.Rows[n].Cells[15].Value = item["NOTE"].ToString();
                        dgvPedidos.Rows[n].Cells[16].Value = item["PHONE"].ToString();
                        dgvPedidos.Rows[n].Cells[17].Value = item["PRESENTMENT_CURRENCY"].ToString();
                        dgvPedidos.Rows[n].Cells[18].Value = item["PROCESSED_AT"].ToString();
                        dgvPedidos.Rows[n].Cells[19].Value = item["PROCESSING_METHOD"].ToString();
                        dgvPedidos.Rows[n].Cells[20].Value = item["SUBTOTAL_PRICE"].ToString();
                        dgvPedidos.Rows[n].Cells[21].Value = item["TOTAL_DISCOUNTS"].ToString();
                        dgvPedidos.Rows[n].Cells[22].Value = item["TAXES_INCLUDED"].ToString();
                        dgvPedidos.Rows[n].Cells[23].Value = item["TOTAL_LINE_ITEMS_PRICE"].ToString();
                        dgvPedidos.Rows[n].Cells[24].Value = item["TOTAL_PRICE"].ToString();
                        dgvPedidos.Rows[n].Cells[25].Value = item["TOTAL_PRICE_USD"].ToString();
                        dgvPedidos.Rows[n].Cells[26].Value = item["TOTAL_TAX"].ToString();
                        dgvPedidos.Rows[n].Cells[27].Value = item["TOTAL_TIP_RECEIVED"].ToString();
                        dgvPedidos.Rows[n].Cells[28].Value = item["TOTAL_WEIGHT"].ToString();
                        dgvPedidos.Rows[n].Cells[29].Value = item["USERS_ID"].ToString();
                        dgvPedidos.Rows[n].Cells[30].Value = item["BILLING_NAME"].ToString();
                        dgvPedidos.Rows[n].Cells[31].Value = item["BILLING_ADDRESS1"].ToString();
                        dgvPedidos.Rows[n].Cells[32].Value = item["BILLING_ADDRESS2"].ToString();
                        dgvPedidos.Rows[n].Cells[33].Value = item["BILLING_PHONE"].ToString();
                        dgvPedidos.Rows[n].Cells[34].Value = item["BILLING_CITY"].ToString();
                        dgvPedidos.Rows[n].Cells[35].Value = item["BILLING_PROVINCE"].ToString();
                        dgvPedidos.Rows[n].Cells[36].Value = item["BILLING_PROVINCE_CODE"].ToString();
                        dgvPedidos.Rows[n].Cells[37].Value = item["BILLING_COUNTRY"].ToString();
                        dgvPedidos.Rows[n].Cells[38].Value = item["BILLING_COUNTRY_CODE"].ToString();
                        dgvPedidos.Rows[n].Cells[39].Value = item["BILLING_ZIP"].ToString();
                        dgvPedidos.Rows[n].Cells[40].Value = item["BILLING_LATITUDE"].ToString();
                        dgvPedidos.Rows[n].Cells[41].Value = item["BILLING_LONGITUDE"].ToString();
                        dgvPedidos.Rows[n].Cells[42].Value = item["BILLING_COMPANY"].ToString();
                        dgvPedidos.Rows[n].Cells[43].Value = item["SHIPPING_NAME"].ToString();
                        dgvPedidos.Rows[n].Cells[44].Value = item["SHIPPING_ADDRESS1"].ToString();
                        dgvPedidos.Rows[n].Cells[45].Value = item["SHIPPING_ADDRESS2"].ToString();
                        dgvPedidos.Rows[n].Cells[46].Value = item["SHIPPING_PHONE"].ToString();
                        dgvPedidos.Rows[n].Cells[47].Value = item["SHIPPING_CITY"].ToString();
                        dgvPedidos.Rows[n].Cells[48].Value = item["SHIPPING_PROVINCE"].ToString();
                        dgvPedidos.Rows[n].Cells[49].Value = item["SHIPPING_PROVINCE_CODE"].ToString();
                        dgvPedidos.Rows[n].Cells[50].Value = item["SHIPPING_COUNTRY"].ToString();
                        dgvPedidos.Rows[n].Cells[51].Value = item["SHIPPING_ZIP"].ToString();
                        dgvPedidos.Rows[n].Cells[52].Value = item["SHIPPING_LATITUDE"].ToString();
                        dgvPedidos.Rows[n].Cells[53].Value = item["SHIPPING_LONGITUDE"].ToString();


                        //dgvPedidos.Rows[n].Cells[3].Value = string.Format("{0:0.00}", item["PESO"]);

                    }
                }
            }
        }

        public void BuscarPedidos(string pedido)
        {
            if (dt_Pedidos != null)
            {
                dgvPedidos.Rows.Clear();
                if (cbbBuscarPedidos.Text.Equals("Cliente"))
                {
                    DataRow[] resultado = dt_Pedidos.Select("CONVERT(BILLING_NAME, System.String) LIKE '%" + pedido + "%'");
                    foreach (DataRow item in resultado)
                    {
                        int n = dgvPedidos.Rows.Add();

                        dgvPedidos.Rows[n].Cells[0].Value = item["ID"].ToString();
                        dgvPedidos.Rows[n].Cells[1].Value = item["ORDER_NUMBER"].ToString();
                        dgvPedidos.Rows[n].Cells[2].Value = item["BROWSER_IP"].ToString();
                        dgvPedidos.Rows[n].Cells[3].Value = item["CANCEL_REASON"].ToString();
                        dgvPedidos.Rows[n].Cells[4].Value = item["CANCELLED_AT"].ToString();
                        dgvPedidos.Rows[n].Cells[5].Value = item["CONFIRMED"].ToString();
                        dgvPedidos.Rows[n].Cells[6].Value = item["CONTACT_EMAIL"].ToString();
                        dgvPedidos.Rows[n].Cells[7].Value = item["CREATED_AT"].ToString();
                        dgvPedidos.Rows[n].Cells[8].Value = item["CURRENCY"].ToString();
                        dgvPedidos.Rows[n].Cells[9].Value = item["CUSTOMER_LOCALE"].ToString();
                        dgvPedidos.Rows[n].Cells[10].Value = item["EMAIL"].ToString();
                        dgvPedidos.Rows[n].Cells[11].Value = item["FINANCIAL_STATUS"].ToString();
                        dgvPedidos.Rows[n].Cells[12].Value = item["FULFILLMENT_STATUS"].ToString();
                        dgvPedidos.Rows[n].Cells[13].Value = item["GATEWAY"].ToString();
                        dgvPedidos.Rows[n].Cells[14].Value = item["NOTE"].ToString();
                        dgvPedidos.Rows[n].Cells[15].Value = item["PHONE"].ToString();
                        dgvPedidos.Rows[n].Cells[16].Value = item["PRESENTMENT_CURRENCY"].ToString();
                        dgvPedidos.Rows[n].Cells[17].Value = item["PROCESSED_AT"].ToString();
                        dgvPedidos.Rows[n].Cells[18].Value = item["PROCESSING_METHOD"].ToString();
                        dgvPedidos.Rows[n].Cells[19].Value = item["SUBTOTAL_PRICE"].ToString();
                        dgvPedidos.Rows[n].Cells[20].Value = item["TOTAL_DISCOUNTS"].ToString();
                        dgvPedidos.Rows[n].Cells[21].Value = item["TAXES_INCLUDED"].ToString();
                        dgvPedidos.Rows[n].Cells[22].Value = item["TOTAL_LINE_ITEMS_PRICE"].ToString();
                        dgvPedidos.Rows[n].Cells[23].Value = item["TOTAL_PRICE"].ToString();
                        dgvPedidos.Rows[n].Cells[24].Value = item["TOTAL_PRICE_USD"].ToString();
                        dgvPedidos.Rows[n].Cells[25].Value = item["TOTAL_TAX"].ToString();
                        dgvPedidos.Rows[n].Cells[26].Value = item["TOTAL_TIP_RECEIVED"].ToString();
                        dgvPedidos.Rows[n].Cells[27].Value = item["TOTAL_WEIGHT"].ToString();
                        dgvPedidos.Rows[n].Cells[28].Value = item["USERS_ID"].ToString();
                        dgvPedidos.Rows[n].Cells[29].Value = item["CONSECUTIVO"].ToString();
                        dgvPedidos.Rows[n].Cells[30].Value = item["BILLING_NAME"].ToString();
                        dgvPedidos.Rows[n].Cells[31].Value = item["BILLING_ADDRESS1"].ToString();
                        dgvPedidos.Rows[n].Cells[32].Value = item["BILLING_ADDRESS2"].ToString();
                        dgvPedidos.Rows[n].Cells[33].Value = item["BILLING_PHONE"].ToString();
                        dgvPedidos.Rows[n].Cells[34].Value = item["BILLING_CITY"].ToString();
                        dgvPedidos.Rows[n].Cells[35].Value = item["BILLING_PROVINCE"].ToString();
                        dgvPedidos.Rows[n].Cells[36].Value = item["BILLING_PROVINCE_CODE"].ToString();
                        dgvPedidos.Rows[n].Cells[37].Value = item["BILLING_COUNTRY"].ToString();
                        dgvPedidos.Rows[n].Cells[38].Value = item["BILLING_COUNTRY_CODE"].ToString();
                        dgvPedidos.Rows[n].Cells[39].Value = item["BILLING_ZIP"].ToString();
                        dgvPedidos.Rows[n].Cells[40].Value = item["BILLING_LATITUDE"].ToString();
                        dgvPedidos.Rows[n].Cells[41].Value = item["BILLING_LONGITUDE"].ToString();
                        dgvPedidos.Rows[n].Cells[42].Value = item["BILLING_COMPANY"].ToString();
                        dgvPedidos.Rows[n].Cells[43].Value = item["SHIPPING_NAME"].ToString();
                        dgvPedidos.Rows[n].Cells[44].Value = item["SHIPPING_ADDRESS1"].ToString();
                        dgvPedidos.Rows[n].Cells[45].Value = item["SHIPPING_ADDRESS2"].ToString();
                        dgvPedidos.Rows[n].Cells[46].Value = item["SHIPPING_PHONE"].ToString();
                        dgvPedidos.Rows[n].Cells[47].Value = item["SHIPPING_CITY"].ToString();
                        dgvPedidos.Rows[n].Cells[48].Value = item["SHIPPING_PROVINCE"].ToString();
                        dgvPedidos.Rows[n].Cells[49].Value = item["SHIPPING_PROVINCE_CODE"].ToString();
                        dgvPedidos.Rows[n].Cells[50].Value = item["SHIPPING_COUNTRY"].ToString();
                        dgvPedidos.Rows[n].Cells[51].Value = item["SHIPPING_ZIP"].ToString();
                        dgvPedidos.Rows[n].Cells[52].Value = item["SHIPPING_LATITUDE"].ToString();
                        dgvPedidos.Rows[n].Cells[53].Value = item["SHIPPING_LONGITUDE"].ToString();
                    }
                }
                else
                {
                    DataRow[] resultado = dt_Pedidos.Select("CONVERT(ORDER_NUMBER, System.String) LIKE '%" + pedido + "%'");
                    foreach (DataRow item in resultado)
                    {
                        int n = dgvPedidos.Rows.Add();

                        dgvPedidos.Rows[n].Cells[0].Value = item["ID"].ToString();
                        dgvPedidos.Rows[n].Cells[1].Value = item["ORDER_NUMBER"].ToString();
                        dgvPedidos.Rows[n].Cells[2].Value = item["BROWSER_IP"].ToString();
                        dgvPedidos.Rows[n].Cells[3].Value = item["CANCEL_REASON"].ToString();
                        dgvPedidos.Rows[n].Cells[4].Value = item["CANCELLED_AT"].ToString();
                        dgvPedidos.Rows[n].Cells[5].Value = item["CONFIRMED"].ToString();
                        dgvPedidos.Rows[n].Cells[6].Value = item["CONTACT_EMAIL"].ToString();
                        dgvPedidos.Rows[n].Cells[7].Value = item["CREATED_AT"].ToString();
                        dgvPedidos.Rows[n].Cells[8].Value = item["CURRENCY"].ToString();
                        dgvPedidos.Rows[n].Cells[9].Value = item["CUSTOMER_LOCALE"].ToString();
                        dgvPedidos.Rows[n].Cells[10].Value = item["EMAIL"].ToString();
                        dgvPedidos.Rows[n].Cells[11].Value = item["FINANCIAL_STATUS"].ToString();
                        dgvPedidos.Rows[n].Cells[12].Value = item["FULFILLMENT_STATUS"].ToString();
                        dgvPedidos.Rows[n].Cells[13].Value = item["GATEWAY"].ToString();
                        dgvPedidos.Rows[n].Cells[14].Value = item["NOTE"].ToString();
                        dgvPedidos.Rows[n].Cells[15].Value = item["PHONE"].ToString();
                        dgvPedidos.Rows[n].Cells[16].Value = item["PRESENTMENT_CURRENCY"].ToString();
                        dgvPedidos.Rows[n].Cells[17].Value = item["PROCESSED_AT"].ToString();
                        dgvPedidos.Rows[n].Cells[18].Value = item["PROCESSING_METHOD"].ToString();
                        dgvPedidos.Rows[n].Cells[19].Value = item["SUBTOTAL_PRICE"].ToString();
                        dgvPedidos.Rows[n].Cells[20].Value = item["TOTAL_DISCOUNTS"].ToString();
                        dgvPedidos.Rows[n].Cells[21].Value = item["TAXES_INCLUDED"].ToString();
                        dgvPedidos.Rows[n].Cells[22].Value = item["TOTAL_LINE_ITEMS_PRICE"].ToString();
                        dgvPedidos.Rows[n].Cells[23].Value = item["TOTAL_PRICE"].ToString();
                        dgvPedidos.Rows[n].Cells[24].Value = item["TOTAL_PRICE_USD"].ToString();
                        dgvPedidos.Rows[n].Cells[25].Value = item["TOTAL_TAX"].ToString();
                        dgvPedidos.Rows[n].Cells[26].Value = item["TOTAL_TIP_RECEIVED"].ToString();
                        dgvPedidos.Rows[n].Cells[27].Value = item["TOTAL_WEIGHT"].ToString();
                        dgvPedidos.Rows[n].Cells[28].Value = item["USERS_ID"].ToString();
                        dgvPedidos.Rows[n].Cells[29].Value = item["CONSECUTIVO"].ToString();
                        dgvPedidos.Rows[n].Cells[30].Value = item["BILLING_NAME"].ToString();
                        dgvPedidos.Rows[n].Cells[31].Value = item["BILLING_ADDRESS1"].ToString();
                        dgvPedidos.Rows[n].Cells[32].Value = item["BILLING_ADDRESS2"].ToString();
                        dgvPedidos.Rows[n].Cells[33].Value = item["BILLING_PHONE"].ToString();
                        dgvPedidos.Rows[n].Cells[34].Value = item["BILLING_CITY"].ToString();
                        dgvPedidos.Rows[n].Cells[35].Value = item["BILLING_PROVINCE"].ToString();
                        dgvPedidos.Rows[n].Cells[36].Value = item["BILLING_PROVINCE_CODE"].ToString();
                        dgvPedidos.Rows[n].Cells[37].Value = item["BILLING_COUNTRY"].ToString();
                        dgvPedidos.Rows[n].Cells[38].Value = item["BILLING_COUNTRY_CODE"].ToString();
                        dgvPedidos.Rows[n].Cells[39].Value = item["BILLING_ZIP"].ToString();
                        dgvPedidos.Rows[n].Cells[40].Value = item["BILLING_LATITUDE"].ToString();
                        dgvPedidos.Rows[n].Cells[41].Value = item["BILLING_LONGITUDE"].ToString();
                        dgvPedidos.Rows[n].Cells[42].Value = item["BILLING_COMPANY"].ToString();
                        dgvPedidos.Rows[n].Cells[43].Value = item["SHIPPING_NAME"].ToString();
                        dgvPedidos.Rows[n].Cells[44].Value = item["SHIPPING_ADDRESS1"].ToString();
                        dgvPedidos.Rows[n].Cells[45].Value = item["SHIPPING_ADDRESS2"].ToString();
                        dgvPedidos.Rows[n].Cells[46].Value = item["SHIPPING_PHONE"].ToString();
                        dgvPedidos.Rows[n].Cells[47].Value = item["SHIPPING_CITY"].ToString();
                        dgvPedidos.Rows[n].Cells[48].Value = item["SHIPPING_PROVINCE"].ToString();
                        dgvPedidos.Rows[n].Cells[49].Value = item["SHIPPING_PROVINCE_CODE"].ToString();
                        dgvPedidos.Rows[n].Cells[50].Value = item["SHIPPING_COUNTRY"].ToString();
                        dgvPedidos.Rows[n].Cells[51].Value = item["SHIPPING_ZIP"].ToString();
                        dgvPedidos.Rows[n].Cells[52].Value = item["SHIPPING_LATITUDE"].ToString();
                        dgvPedidos.Rows[n].Cells[53].Value = item["SHIPPING_LONGITUDE"].ToString();

                    }
                }
            }
        }

        private void btnObtenerPedidos_Click(object sender, EventArgs e)
        {
            dgvPedidos.Rows.Clear();
            CargaPedidos();
        }

        private void dgvPedidos_DoubleClick(object sender, EventArgs e)
        {
            string orden = dgvPedidos.SelectedCells[1].Value.ToString();
            MostrarPedidoLinea pl = new MostrarPedidoLinea(orden);
            try
            {
                pl.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void txtBuscarPedidos_TextChanged(object sender, EventArgs e)
        {
            BuscarPedidos(txtBuscarPedidos.Text);
        }
    }
}
