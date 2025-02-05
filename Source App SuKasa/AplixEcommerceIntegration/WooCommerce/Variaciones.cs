using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AplixEcommerceIntegration.WooCommerce
{
    public partial class Variaciones : Form
    {
        static string articulo_padre = "";
        //Lista con los terminos de atributos del articulo padre guardados 

        //esta se utiliza para asignar atributos a un articulo padre
        static List<Clases.ArticulosVariaciones> lista_llena_var = new List<Clases.ArticulosVariaciones>();

        static List<Clases.TerminosAtributosCompletoWoocomerce> lista_var = new List<Clases.TerminosAtributosCompletoWoocomerce>();

        static List<Clases.ArticulosVariaciones> lista_variaciones_editadas = new List<Clases.ArticulosVariaciones>();

        public Variaciones(string articulo, string descripcion, string variaciones)
        {
            InitializeComponent();
            lbnom.Text = articulo;
            lbdes.Text = descripcion;
            articulo_padre = articulo;
            met_cargar_variaciones();
            cargar_combo_estados_articulos();
        }

        private void panel_atributos_Click(object sender, EventArgs e)
        {

        }
        private void Variaciones_Load(object sender, EventArgs e)
        {
            met_cargar_terminos_atributos();
        }
        public void met_cargar_terminos_atributos()
        {
            dt_terninos_atributo.Rows.Clear();
            Metodos.MetodosTerminoAtributosWoocomerce met_atributos = new Metodos.MetodosTerminoAtributosWoocomerce();
            DataTable v_tabla = new DataTable();
            v_tabla = met_atributos.mostrar_terminos_atributos();

            lista_llena_var = met_cargar_lista_atributos_padre();

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                bool est = true;
                string id_termino = v_tabla.Rows[i]["ID"].ToString();
                string nombre_termino = v_tabla.Rows[i]["NOMBRE"].ToString();
                string id_atributo = v_tabla.Rows[i]["ID_ATRIBUTO"].ToString();
                string nombre_atributo = v_tabla.Rows[i]["ATRIBUTO"].ToString();

                Clases.ArticulosVariaciones product = lista_llena_var.FirstOrDefault(x => x.id_termino == Convert.ToInt32(v_tabla.Rows[i]["ID"].ToString()));

                //si ese termino de atributo viene en la lista entonces pongale el check
                if (product == null){est = false;} else {est = true;}
                dt_terninos_atributo.Rows.Add(id_atributo, nombre_atributo, id_termino, nombre_termino, est);


            }

        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
         
        }       
        private void dt_terninos_atributo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            dt_terninos_atributo.EndEdit();

            if (e.ColumnIndex == 4)
            {


                Clases.TerminosAtributosCompletoWoocomerce obj_var = new Clases.TerminosAtributosCompletoWoocomerce();
                obj_var.id_atributo = Convert.ToInt32(this.dt_terninos_atributo.CurrentRow.Cells[0].Value.ToString());
                obj_var.name_atributo = this.dt_terninos_atributo.CurrentRow.Cells[1].Value.ToString();
                obj_var.id_termino_atributo = Convert.ToInt32(this.dt_terninos_atributo.CurrentRow.Cells[2].Value.ToString());
                obj_var.name_termino_atributo = this.dt_terninos_atributo.CurrentRow.Cells[3].Value.ToString();
                obj_var.estado = this.dt_terninos_atributo.CurrentRow.Cells[4].Value.ToString();

                Clases.TerminosAtributosCompletoWoocomerce product = lista_var.FirstOrDefault(x => x.id_termino_atributo == Convert.ToInt32(this.dt_terninos_atributo.CurrentRow.Cells[2].Value.ToString()));

                if (product == null)
                {
                    lista_var.Add(obj_var);
                }
                else
                {

                    lista_var.RemoveAll(x => x.id_termino_atributo == (obj_var.id_termino_atributo));
                    lista_var.Add(obj_var);

                }

                btnsave.Enabled = true;

                //dt_categorias.EndEdit();

            }


        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
          

            if (lista_var.Count > 0) {

                int i = 0;

                while ( i < lista_var.Count) {

                    Metodos.MetodosVariacionesWoocomerce met_var = new Metodos.MetodosVariacionesWoocomerce();
                    Clases.ArtributosArticulos obj = new Clases.ArtributosArticulos();
                    obj.articulo = articulo_padre;
                    obj.id_atributo = lista_var[i].id_atributo;
                    obj.id_termino_atributo = lista_var[i].id_termino_atributo;

                    met_var.insertar_atributos_articulos(obj);

                    i++;
                }

                lista_var.Clear();
                MessageBox.Show("Términos de atributos actualizadas exitosamente para el articulo : " + articulo_padre , "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }



        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //dt_variaciones.Rows.Add(null, null, null, null, null);

            Articulos_variaciones frm_var = new Articulos_variaciones();
            AddOwnedForm(frm_var);
            frm_var.ShowDialog();
        }
        private void dt_variaciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
        private void dt_variaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == 0)
            //{
            //    Articulos_variaciones frm_var = new Articulos_variaciones( Convert.ToInt32(dt_variaciones.CurrentRow.Index));
            //    AddOwnedForm(frm_var);
            //    frm_var.ShowDialog();
            //}
        }
        public void met_asignar_valores_greed( string articulo, string desscripcon, string cantidad, string precio, string peso) {

            dt_variaciones.Rows.Add(articulo, desscripcon, null, precio, cantidad, null, peso, null, null ,true, "publish", true , null);
            //dt_variaciones.Rows[fila].Cells[0].Value = articulo;
            //dt_variaciones.Rows[fila].Cells[1].Value = desscripcon;
            //dt_variaciones.Rows[fila].Cells[2].Value = precio;
            //dt_variaciones.Rows[fila].Cells[3].Value = cantidad;
            dt_variaciones.EndEdit();

        }
        public void met_asignar_valores_atributos_greed(string atributos , int fila)
        {         
            dt_variaciones.Rows[fila].Cells[5].Value = atributos;
            dt_variaciones.EndEdit();

        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //asiganar_valores_greed();
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        private void dt_variaciones_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ////if (e.ColumnIndex == 0)
            ////{
            ////    Articulos_variaciones frm_var = new Articulos_variaciones(Convert.ToInt32(dt_variaciones.CurrentRow.Index));
            ////    AddOwnedForm(frm_var);
            ////    frm_var.ShowDialog();
            ////}

            //if (e.ColumnIndex == 4)
            //{

            //    string cadena = "";

            //    if (this.dt_variaciones.CurrentRow.Cells[4].Value != null) {

            //        cadena = this.dt_variaciones.CurrentRow.Cells[4].Value.ToString();

            //    }

            //    Asignar_Terminos_Variaciones frm_var = new Asignar_Terminos_Variaciones(
            //    Convert.ToInt32(dt_variaciones.CurrentRow.Index), articulo_padre , cadena );
            //    AddOwnedForm(frm_var);
            //    frm_var.ShowDialog();
            //}

        }
        public List<Clases.ArticulosVariaciones> met_cargar_lista_atributos_padre()
        {

            List<Clases.ArticulosVariaciones> lista = new List<Clases.ArticulosVariaciones>();

            Metodos.MetodosVariacionesWoocomerce met_atributos = new Metodos.MetodosVariacionesWoocomerce();
            DataTable v_tabla = new DataTable();

            //recibimos el articulo del data greed de articulos
            Clases.ArticulosVariaciones obj = new Clases.ArticulosVariaciones();
            obj.articulo_padre = articulo_padre;

            v_tabla = met_atributos.mostrar_variaciones_padres(obj);

            //LLenamos la ista con esa consulta
            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                //Hacemos una clase con los mismos valores de lista de data
                Clases.ArticulosVariaciones obj_var = new Clases.ArticulosVariaciones();
                bool est = true;
                obj_var.id_termino = Convert.ToInt32(v_tabla.Rows[i]["ID_TERMINO_ATRIBUTO"].ToString());
                string estado = v_tabla.Rows[i]["ESTADO"].ToString();
                if (estado == "S") { est = true; } else { est = false; }
                lista.Add(obj_var);
            }

            return lista;

        }
        private void Variaciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            lista_llena_var.Clear();
            lista_var.Clear();
        }
        private void btn_art_Click(object sender, EventArgs e)
        {

            dt_variaciones.EndEdit();


            try
            {
                foreach (DataGridViewRow row in dt_variaciones.Rows)
                {

                    Clases.ArticulosVariaciones obj = new Clases.ArticulosVariaciones();

                    if (row.Cells["sku"].Value == null || row.Cells["variaciones_woo"].Value == null ||
                        row.Cells["sku"].Value.ToString() == "" || row.Cells["variaciones_woo"].Value.ToString() == "")
                    {
                    }
                    else
                    {

                        obj.articulo       = row.Cells["sku"].Value.ToString();

                        obj.articulo_padre = articulo_padre;

                        obj.termino_lista = row.Cells["variaciones_woo"].Value.ToString();

                        if (row.Cells["descripcion_woo"].Value == null)
                        {
                            obj.nombre = "";
                        }
                        else {

                            obj.nombre = row.Cells["descripcion_woo"].Value.ToString();
                        }                      
                       
                        obj.usa_stock      = row.Cells["manage_var"].Value.ToString();

                        obj.estado         = row.Cells["status_producto"].Value.ToString();

                        obj.sincroniza     = row.Cells["sincroniza_var"].Value.ToString();

                        if (row.Cells["id_woo"].Value == null)
                        {
                            obj.id = "";
                        }
                        else
                        {

                            obj.id = row.Cells["id_woo"].Value.ToString();
                        }
                       
                                           
                        Metodos.MetodosVariacionesWoocomerce met = new Metodos.MetodosVariacionesWoocomerce();
                        met.Insertar_variaciones(obj);


                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

      

        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {

        }
        private void dt_variaciones_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            Clases.ArticulosVariaciones obj_var = new Clases.ArticulosVariaciones();



            obj_var.id = this.dt_variaciones.CurrentRow.Cells[5].Value.ToString();
            obj_var.articulo = this.dt_variaciones.CurrentRow.Cells[0].Value.ToString();
            obj_var.articulo_padre = articulo_padre;
            obj_var.cantidad = Convert.ToInt32(this.dt_variaciones.CurrentRow.Cells[2].Value.ToString());
            obj_var.precio = this.dt_variaciones.CurrentRow.Cells[3].Value.ToString();
            obj_var.termino_lista = this.dt_variaciones.CurrentRow.Cells[4].Value.ToString();



            Clases.ArticulosVariaciones product = lista_variaciones_editadas.FirstOrDefault(x => x.articulo == this.dt_variaciones.CurrentRow.Cells[0].Value.ToString());

            if (product == null)
            {
                lista_variaciones_editadas.Add(obj_var);
            }
            else
            {

                lista_variaciones_editadas.RemoveAll(x => x.articulo == (obj_var.articulo));
                lista_variaciones_editadas.Add(obj_var);

            }

            btnsave.Enabled = true;
        }
        public void met_cargar_variaciones()
        {
            dt_terninos_atributo.Rows.Clear();
            Metodos.MetodosVariacionesWoocomerce met_var = new Metodos.MetodosVariacionesWoocomerce();
            DataTable v_tabla = new DataTable();

            //Obtenemos todas las variacione del padre
            Clases.ArticulosVariaciones obj_var = new Clases.ArticulosVariaciones();
            obj_var.articulo_padre = articulo_padre;
            v_tabla = met_var.mostrar_variaciones(obj_var);

         
            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                bool est = true;
                bool est_usa_stock = true;

                string articulo = v_tabla.Rows[i]["ARTICULO"].ToString();
                string nombre = v_tabla.Rows[i]["NOMBRE"].ToString();
                string descripcion_woocomerce = v_tabla.Rows[i]["DESCRIPCION_WOOCOMERCE"].ToString();
                string precio = v_tabla.Rows[i]["PRECIO"].ToString();
                string cantidad = v_tabla.Rows[i]["CANTIDAD"].ToString();
                string id_terminos = v_tabla.Rows[i]["ID_TERMINO_ATRIBUTO"].ToString();
                string peso = v_tabla.Rows[i]["PESO"].ToString();
                string id_woocomerce = v_tabla.Rows[i]["ID_WOOCOMERCE"].ToString();
                string descuento = v_tabla.Rows[i]["DESCUENTO"].ToString();
                string usa_stock = v_tabla.Rows[i]["USA_STOCK"].ToString();
                string estado = v_tabla.Rows[i]["ESTADO"].ToString();
                string sincroniza = v_tabla.Rows[i]["SINCRONIZA"].ToString();
                string record_date = v_tabla.Rows[i]["RECORDDATE"].ToString();

                if (usa_stock == "S")
                {est_usa_stock = true;} else {est_usa_stock = false;}

                if (sincroniza == "S")
                { est = true; } else { est = false; }


                //Clases.ArticulosVariaciones product = lista_llena_var.FirstOrDefault(x => x.id_termino == Convert.ToInt32(v_tabla.Rows[i]["ID"].ToString()));

                ////si ese termino de atributo viene en la lista entonces pongale el check
                //if (product == null) { est = false; } else { est = true; }

                dt_variaciones.Rows.Add(articulo, nombre, descripcion_woocomerce, precio, cantidad, id_terminos, peso , id_woocomerce, descuento, est_usa_stock, estado, est, record_date  );


            }

        }
        private void dt_variaciones_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {

            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {

            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {

            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {

            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";
                anError.ThrowException = false;
            }
        }
        private void dt_variaciones_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dt_variaciones_CellMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (e.ColumnIndex == 0)
            //{
            //    Articulos_variaciones frm_var = new Articulos_variaciones(Convert.ToInt32(dt_variaciones.CurrentRow.Index));
            //    AddOwnedForm(frm_var);
            //    frm_var.ShowDialog();
            //}

            if (e.ColumnIndex == 5)
            {

                string cadena = "";

                if (this.dt_variaciones.CurrentRow.Cells[5].Value != null)
                {

                    cadena = this.dt_variaciones.CurrentRow.Cells[5].Value.ToString();

                }

                Asignar_Terminos_Variaciones frm_var = new Asignar_Terminos_Variaciones(
                Convert.ToInt32(dt_variaciones.CurrentRow.Index), articulo_padre, cadena);
                AddOwnedForm(frm_var);
                frm_var.ShowDialog();
            }
        }


        public void cargar_combo_estados_articulos()
        {

            Metodos.MetodosArticulosWoocomerce met_cargar_estados = new Metodos.MetodosArticulosWoocomerce();
            List<Clases.EstadosArticuloWoocomerce> lista_estados = new List<Clases.EstadosArticuloWoocomerce>();
            lista_estados = met_cargar_estados.obtener_estados_articulos();
            ComboBox CB = new ComboBox();

            int i = 0;

            while (i < lista_estados.Count)
            {
                CB.Items.Add(lista_estados[i].descripcion);
                i++;
            }

         ((DataGridViewComboBoxColumn)dt_variaciones.Columns["status_producto"]).DataSource = CB.Items;

        }


    }
}
